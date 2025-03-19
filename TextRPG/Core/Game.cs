namespace TextRPG.Core;

public class Game
{
    public Dictionary<int, Scene> Scenes { get; private set; }
    public int CurrentScene { get; private set; }
    public int Money { get; private set; }
    public int Health { get; private set; }

    public Game()
    {
        Money = 50;
        Health = 100;

        Scenes = new Dictionary<int, Scene>
        {
            { 1, new Scene { Text = "Ты — житель Убежища 27. Вода заканчивается, и тебе нужно найти детали для генератора. Куда пойдёшь?",
                Choices = new Dictionary<string, int> { { "Выйти в пустошь", 2 }, { "Спросить Смотрителя", 3 } } } },

            { 2, new Scene { Text = "Ты вышел в пустошь. Перед тобой разрушенный город и старая заправка. Куда пойдёшь?",
                Choices = new Dictionary<string, int> { { "Город", 4 }, { "Заправка", 5 } } } },

            { 3, new Scene { Text = "Смотритель даёт тебе 50 крышек и карту пустоши.",
                Choices = new Dictionary<string, int> { { "Выйти в пустошь", 2 } } } },

            { 4, new Scene { Text = "Ты встречаешь рейдеров, они требуют 20 крышек. Что делать?",
                Choices = new Dictionary<string, int> { { "Заплатить (20 крышек)", 6 }, { "Попытаться убежать (-30 здоровья)", 7 } } } },

            { 5, new Scene { Text = "На заправке ты находишь торговца.",
                Choices = new Dictionary<string, int> { { "Купить стимпак (30 крышек, +50 здоровья)", 8 }, { "Обыскать заправку", 9 } } } },

            { 6, new Scene { Text = "Ты заплатил рейдерам. Они отпустили тебя.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 7, new Scene { Text = "Ты ранен, но убежал.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 8, new Scene { Text = "Ты купил стимпак и восстановил здоровье.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 9, new Scene { Text = "Ты нашёл 20 крышек!",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 10, new Scene { Text = "Приключение только начинается...", Choices = [] } }
        };

        CurrentScene = 1;
    }

    public Scene GetCurrentScene() => Scenes[CurrentScene];

    public void MakeChoice(string choice)
    {
        if (Scenes[CurrentScene].Choices.TryGetValue(choice, out int value))
        {
            CurrentScene = value;

            if (choice == "Заплатить (20 крышек)") Money -= 20;
            if (choice == "Попытаться убежать (-30 здоровья)") Health -= 30;
            if (choice == "Купить стимпак (30 крышек, +50 здоровья)")
            {
                Money -= 30;
                Health += 50;
            }
            if (choice == "Обыскать заправку") Money += 20;
        }
    }
}