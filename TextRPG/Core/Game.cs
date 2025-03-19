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
            { 1, new Scene { Text = "Ты — житель Комплекса 17. Система фильтрации воздуха выходит из строя, и тебе нужно найти запасные детали. Куда отправишься?",
                Choices = new Dictionary<string, int> { { "Выйти на поверхность", 2 }, { "Поговорить с инженером", 3 } } } },

            { 2, new Scene { Text = "Ты выбрался наружу. Вокруг заброшенные здания и пустынная дорога. Вдалеке виднеется старая ремонтная станция и разрушенный город.",
                Choices = new Dictionary<string, int> { { "Пойти в город", 4 }, { "Осмотреть ремонтную станцию", 5 } } } },

            { 3, new Scene { Text = "Инженер даёт тебе 50 кредитов и карту местности.",
                Choices = new Dictionary<string, int> { { "Выйти на поверхность", 2 } } } },

            { 4, new Scene { Text = "На пути в город ты встречаешь группу выживших. Они требуют 20 кредитов за проход. Что делать?",
                Choices = new Dictionary<string, int> { { "Заплатить (20 кредитов)", 6 }, { "Попытаться убежать (-30 здоровья)", 7 } } } },

            { 5, new Scene { Text = "На ремонтной станции ты встречаешь старого механика.",
                Choices = new Dictionary<string, int> { { "Купить медпак (30 кредитов, +50 здоровья)", 8 }, { "Обыскать станцию", 9 } } } },

            { 6, new Scene { Text = "Ты заплатил за проход. Выжившие пропустили тебя без лишних проблем.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 7, new Scene { Text = "Ты получил несколько ранений, но сумел сбежать.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 8, new Scene { Text = "Ты купил медпак и восстановил здоровье.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 9, new Scene { Text = "Ты нашёл 20 кредитов среди обломков.",
                Choices = new Dictionary<string, int> { { "Идти дальше", 10 } } } },

            { 10, new Scene { Text = "Твоё приключение только начинается...", Choices = [] } }
        };

        CurrentScene = 1;
    }

    public Scene GetCurrentScene() => Scenes[CurrentScene];

    public void MakeChoice(string choice)
    {
        if (Scenes[CurrentScene].Choices.TryGetValue(choice, out int value))
        {
            CurrentScene = value;

            if (choice == "Заплатить (20 кредитов)") Money -= 20;
            if (choice == "Попытаться убежать (-30 здоровья)") Health -= 30;
            if (choice == "Купить медпак (30 кредитов, +50 здоровья)")
            {
                Money -= 30;
                Health += 50;
            }
            if (choice == "Обыскать станцию") Money += 20;
        }
    }
}
