using NAudio.Wave;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            var currentLocation = game.GetCurrentLocation();
            var currentScene = game.GetCurrentScene();

            // Обновляем информацию о локации
            txtLocationName.Text = currentLocation.Name;
            txtLocationDescription.Text = currentLocation.Description;
            txtStory.Text = currentScene.Text;

            // Обновляем статусы
            txtMoney.Text = game.Money.ToString();
            txtHealth.Text = game.Health.ToString();

            // Обновляем фоновое изображение
            if (!string.IsNullOrEmpty(currentLocation.BackgroundImage))
            {
                BackgroundImage.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/{currentLocation.BackgroundImage}"));
            }

            // Очищаем и обновляем кнопки выбора
            choicesPanel.Children.Clear();

            if (game.CurrentEventSceneId.HasValue)
            {
                // Если есть активное событие, показываем кнопки выбора для события
                foreach (var choice in currentScene.Choices)
                {
                    Button btn = new()
                    {
                        Content = choice.Key,
                        FontSize = 16,
                        Padding = new Thickness(10),
                        Background = new SolidColorBrush(Color.FromRgb(34, 150, 243)),
                        Foreground = Brushes.White,
                        Margin = new Thickness(5),
                        BorderThickness = new Thickness(0),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    btn.Click += (s, e) =>
                    {
                        game.MakeChoice(choice.Key);
                        UpdateUI();
                    };

                    choicesPanel.Children.Add(btn);
                }
            }
            else
            {
                // Если нет активного события, показываем кнопки для перемещения между локациями
                foreach (var locationId in currentLocation.ConnectedLocations)
                {
                    var location = game.Locations[locationId];
                    Button btn = new()
                    {
                        Content = $"Перейти в {location.Name}",
                        FontSize = 16,
                        Padding = new Thickness(10),
                        Background = new SolidColorBrush(Color.FromRgb(34, 150, 243)),
                        Foreground = Brushes.White,
                        Margin = new Thickness(5),
                        BorderThickness = new Thickness(0),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    btn.Click += (s, e) =>
                    {
                        game.TravelToLocation(locationId);
                        UpdateUI();
                    };

                    choicesPanel.Children.Add(btn);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopMusic();
        }
    }
}