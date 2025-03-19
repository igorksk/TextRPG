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
        private readonly Dictionary<int, string> sceneBackgrounds = new()
        {
            { 1, "Images/complex_hall.jpeg" },   
            { 2, "Images/desert_ruins.jpeg" },   
            { 3, "Images/engineer_room.jpeg" },  
            { 4, "Images/survivors_group.jpeg" },
            { 5, "Images/repair_station.jpeg" }, 
            { 6, "Images/trade_post.jpeg" }, 
            { 7, "Images/med_post.jpeg" } 
        };

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
            txtMoney.Text = game.Money.ToString();
            txtHealth.Text = game.Health.ToString();
            choicesPanel.Children.Clear();

            if (sceneBackgrounds.TryGetValue(game.CurrentScene, out string? value))
            {
                BackgroundImage.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/{value}"));
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