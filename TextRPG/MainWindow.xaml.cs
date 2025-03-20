using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Media;
using TextRPG.Core;

namespace TextRPG
{
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
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
                txtLocationName.Text = currentLocation.Name;
                txtLocationDescription.Text = currentLocation.Description;

                // Обновляем фоновое изображение
                try
                {
                    backgroundImage.ImageSource = new BitmapImage(new Uri(currentLocation.BackgroundImage, UriKind.Relative));
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
    }
}