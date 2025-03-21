using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using NAudio.Wave;
using System.IO;
using TextRPG.Core;

namespace TextRPG
{
    public partial class MainWindow : Window
    {
        private readonly Game game;
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            PlayBackgroundMusic("Sounds/wasteland.mp3");
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Очищаем панель выбора
            choicesPanel.Children.Clear();

            if (game.ShouldExit)
            {
                Close();
                return;
            }

            var currentScene = game.GetCurrentScene();
            var currentLocation = game.GetCurrentLocation();

            // Обновляем текст сцены
            sceneText.Text = currentScene.Text;

            // Обновляем информацию о локации
            if (game.IsInMainMenu)
            {
                txtLocationName.Text = "Главное меню";
                txtLocationDescription.Text = "";
                // Устанавливаем фоновое изображение для главного меню
                try
                {
                    backgroundImage.ImageSource = new BitmapImage(new Uri("Images/complex_hall.jpeg", UriKind.Relative));
                }
                catch
                {
                    // Если изображение не найдено, используем пустое изображение
                    backgroundImage.ImageSource = null;
                }
            }
            else
            {
                txtLocationName.Text = currentLocation?.Name ?? string.Empty;
                txtLocationDescription.Text = currentLocation?.Description ?? string.Empty;

                // Обновляем фоновое изображение
                try
                {
                    backgroundImage.ImageSource = new BitmapImage(new Uri(currentLocation?.ImagePath ?? string.Empty, UriKind.Relative));
                }
                catch
                {
                    // Если изображение не найдено, используем пустое изображение
                    backgroundImage.ImageSource = null;
                }
            }

            // Обновляем статус игрока (только если не в главном меню)
            if (!game.IsInMainMenu)
            {
                statusText.Text = $"Здоровье: {game.Health} | Деньги: {game.Money} кредитов";
            }
            else
            {
                statusText.Text = "";
            }

            // Если выбираем локацию и не в главном меню
            if (game.IsChoosingLocation && !game.IsInMainMenu)
            {
                foreach (var location in game.Locations.Values)
                {
                    if (game.CanTravelTo(location.Id))
                    {
                        var button = new Button
                        {
                            Content = location.Name,
                            Margin = new Thickness(5),
                            Padding = new Thickness(5),
                            MinWidth = 100
                        };
                        button.Click += (s, e) => LocationButton_Click(location.Id);
                        choicesPanel.Children.Add(button);
                    }
                }
            }
            // Иначе показываем варианты выбора из сцены
            else
            {
                foreach (var choice in currentScene.Choices)
                {
                    var button = new Button
                    {
                        Content = choice.Key,
                        Margin = new Thickness(5),
                        Padding = new Thickness(5),
                        MinWidth = 100
                    };
                    button.Click += (s, e) => ChoiceButton_Click(choice.Key);
                    choicesPanel.Children.Add(button);
                }
            }
        }

        private void PlayBackgroundMusic(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Файл {filePath} не найден!");
                    return;
                }

                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(filePath);
                outputDevice.Init(audioFile);
                outputDevice.Volume = 0.1f;
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения: {ex.Message}");
            }
        }

        private void StopMusic()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
        }

        private void LocationButton_Click(string locationId)
        {
            game.TravelToLocation(locationId);
            UpdateUI();
        }

        private void ChoiceButton_Click(string choice)
        {
            game.MakeChoice(choice);
            UpdateUI();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            StopMusic();
        }
    }
}