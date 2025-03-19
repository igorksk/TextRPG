using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextRPG.Core;

namespace TextRPG;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Game game;

    public MainWindow()
    {
        InitializeComponent();
        game = new Game();
        UpdateScene();
    }

    private void UpdateScene()
    {
        txtStory.Text = game.GetCurrentScene().Text;
        choicesPanel.Children.Clear();

        foreach (var choice in game.GetCurrentScene().Choices)
        {
            Button btn = new() { Content = choice.Key, Margin = new Thickness(5) };
            btn.Click += (s, e) => { game.MakeChoice(choice.Key); UpdateScene(); };
            choicesPanel.Children.Add(btn);
        }
    }
}