using System;
using System.Collections.Generic;
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
        private static readonly Dictionary<int, string> dictionary = new()
        {
            { 1, "Images/vault.jpg" },
            { 2, "Images/wasteland.jpg" },
            { 3, "Images/overseer.jpg" },
            { 4, "Images/raiders.jpg" },
            { 5, "Images/gasstation.jpg" }
        };
        private readonly Dictionary<int, string> sceneBackgrounds = dictionary;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            UpdateScene();
        }

        private void UpdateScene()
        {
            var scene = game.GetCurrentScene();
            txtStory.Text = scene.Text;
            choicesPanel.Children.Clear();

            // Обновление фонового изображения
            if (sceneBackgrounds.TryGetValue(game.CurrentScene, out string? value))
            {
                BackgroundImage.Source = new BitmapImage(new Uri(value, UriKind.Relative));
            }

            foreach (var choice in scene.Choices)
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
                    UpdateScene();
                };

                choicesPanel.Children.Add(btn);
            }
        }
    }
}