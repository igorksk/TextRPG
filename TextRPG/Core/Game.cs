namespace TextRPG.Core;

public class Game
{
    public Dictionary<int, Scene> Scenes { get; private set; }
    public int CurrentScene { get; private set; }

    public Game()
    {
        Scenes = new Dictionary<int, Scene>
        {
            { 1, new Scene { Text = "Ты — житель Убежища 27. Тебе поручено найти детали для генератора воды. Что делать?", Choices = new Dictionary<string, int> { { "Выйти в пустошь", 2 }, { "Спросить Смотрителя", 3 } } } },
            { 2, new Scene { Text = "Ты вышел наружу. Перед тобой разрушенный город и старая заправка. Куда пойдёшь?", Choices = new Dictionary<string, int> { { "Город", 4 }, { "Заправка", 5 } } } },
            { 3, new Scene { Text = "Смотритель даёт тебе 50 крышек и карту пустоши.", Choices = new Dictionary<string, int> { { "Выйти в пустошь", 2 } } } },
            { 4, new Scene { Text = "В городе тебя встречают рейдеры. Они требуют крышки. Что делать?", Choices = new Dictionary<string, int> { { "Отдать крышки", 6 }, { "Попытаться убежать", 7 } } } },
            { 5, new Scene { Text = "На заправке ты находишь торговца. Он предлагает купить лазерный пистолет за 40 крышек.", Choices = new Dictionary<string, int> { { "Купить", 8 }, { "Отказать", 9 } } } },
            { 6, new Scene { Text = "Рейдеры смеются, забирают крышки и прогоняют тебя.", Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },
            { 7, new Scene { Text = "Ты убегаешь, но получаешь ранение. Выживание стало сложнее.", Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },
            { 8, new Scene { Text = "Ты купил лазерный пистолет. Теперь можешь себя защитить!", Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },
            { 9, new Scene { Text = "Ты отказался и пошёл дальше.", Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },
            { 10, new Scene { Text = "Ты углубляешься в пустошь, зная, что приключение только начинается...", Choices = [] } }
        };

        CurrentScene = 1;
    }

    public Scene GetCurrentScene() => Scenes[CurrentScene];

    public void MakeChoice(string choice)
    {
        if (Scenes[CurrentScene].Choices.TryGetValue(choice, out int value))
        {
            CurrentScene = value;
        }
    }
}