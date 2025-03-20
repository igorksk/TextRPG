#nullable enable

using System.Collections.Generic;

namespace TextRPG.Core;

public class Game
{
    public Dictionary<string, Location> Locations { get; private set; }
    public Dictionary<int, Scene> Scenes { get; private set; }
    public string CurrentLocationId { get; private set; }
    public int? CurrentEventSceneId { get; private set; }
    public int Money { get; private set; }
    public int Health { get; private set; }
    public HashSet<int> ShownScenes { get; private set; }
    public bool IsChoosingLocation { get; private set; }
    public bool IsInMainMenu { get; private set; }
    public bool ShouldExit { get; private set; }

    public Game()
    {
        IsInMainMenu = true;
        ShouldExit = false;
        Locations = new Dictionary<string, Location>();
        Scenes = new Dictionary<int, Scene>();
        ShownScenes = new HashSet<int>();
        CurrentLocationId = string.Empty;
        InitializeMainMenu();
    }

    private void InitializeMainMenu()
    {
        Scenes = new Dictionary<int, Scene>
        {
            { -1, new Scene { Text = "Текстовая RPG\n\nДобро пожаловать в игру!\nВыберите действие:",
                Choices = new Dictionary<string, int> { 
                    { "Новая игра", -2 },
                    { "Выход", -3 }
                } } },
            
            // Добавляем сцену выбора локации
            { 0, new Scene { Text = "Выберите следующую локацию:",
                Choices = new Dictionary<string, int>() } }
        };
        CurrentEventSceneId = -1;
    }

    private void StartNewGame()
    {
        Money = 50;
        Health = 100;
        ShownScenes = new HashSet<int>();
        IsChoosingLocation = true;
        IsInMainMenu = false;

        // Добавляем сцену выбора локации к существующим сценам
        Scenes[0] = new Scene { Text = "Выберите следующую локацию:",
            Choices = new Dictionary<string, int>() };

        // Добавляем остальные сцены
        Scenes[1] = new Scene { Text = "Ты — житель Комплекса 17. Система фильтрации воздуха выходит из строя, и тебе нужно найти запасные детали. Ты решаешь обратиться к инженеру комплекса.",
            Choices = new Dictionary<string, int> { { "Поговорить с инженером", 2 } } };

        Scenes[2] = new Scene { Text = "Инженер объясняет, что нужны специальные детали, которые можно найти только на поверхности. Он даёт тебе 50 кредитов и карту местности.",
            Choices = new Dictionary<string, int> { { "Выбрать следующую локацию", 0 } } };

        Scenes[3] = new Scene { Text = "Ты выбрался наружу. Вокруг заброшенные здания и пустынная дорога. Вдалеке виднеется старая ремонтная станция и разрушенный город.",
            Choices = new Dictionary<string, int> { { "Выбрать следующую локацию", 0 } } };

        Scenes[4] = new Scene { Text = "На пути в город ты встречаешь группу выживших. Они требуют 20 кредитов за проход. Что делать?",
            Choices = new Dictionary<string, int> { 
                { "Заплатить (20 кредитов)", 5 }, 
                { "Попытаться убежать (-30 здоровья)", 6 } 
            } };

        Scenes[5] = new Scene { Text = "Ты заплатил за проход. Выжившие пропустили тебя без лишних проблем. В городе ты находишь несколько полезных деталей.",
            Choices = new Dictionary<string, int> { { "Выбрать следующую локацию", 0 } } };

        Scenes[6] = new Scene { Text = "Ты получил несколько ранений, но сумел сбежать. В городе ты находишь несколько полезных деталей.",
            Choices = new Dictionary<string, int> { { "Выбрать следующую локацию", 0 } } };

        Scenes[7] = new Scene { Text = "На ремонтной станции ты встречаешь старого механика. Он предлагает тебе сделку.",
            Choices = new Dictionary<string, int> { 
                { "Купить медпак (30 кредитов, +50 здоровья)", 8 }, 
                { "Обыскать станцию", 9 } 
            } };

        Scenes[8] = new Scene { Text = "Ты купил медпак и восстановил здоровье. Механик также даёт тебе подсказку о местоположении нужных деталей.",
            Choices = new Dictionary<string, int> { { "Выбрать следующую локацию", 0 } } };

        Scenes[9] = new Scene { Text = "Ты нашёл 20 кредитов среди обломков. Механик, заметив твои действия, предлагает купить у него информацию о деталях.",
            Choices = new Dictionary<string, int> { { "Выбрать следующую локацию", 0 } } };

        // Инициализация локаций
        Locations = new Dictionary<string, Location>
        {
            { "complex", new Location("complex", "Комплекс 17", "Твой дом, подземный комплекс с системой фильтрации воздуха.", "Images/complex_hall.jpeg", 1) },
            { "engineer", new Location("engineer", "Комната инженера", "Техническое помещение с оборудованием для ремонта.", "Images/engineer_room.jpeg", 2) },
            { "surface", new Location("surface", "Поверхность", "Заброшенная поверхность с разрушенными зданиями.", "Images/desert_ruins.jpeg", 3) },
            { "city", new Location("city", "Город", "Разрушенный город, где живут выжившие.", "Images/survivors_group.jpeg", 4) },
            { "station", new Location("station", "Ремонтная станция", "Старая станция с медицинским оборудованием.", "Images/repair_station.jpeg", 7) }
        };

        // Добавляем связи между локациями
        Locations["complex"].AddConnection("engineer");
        Locations["engineer"].AddConnection("surface");
        Locations["surface"].AddConnection("city");
        Locations["surface"].AddConnection("station");
        Locations["city"].AddConnection("surface");
        Locations["station"].AddConnection("surface");

        // Начинаем в комплексе
        CurrentLocationId = "complex";
        CurrentEventSceneId = Locations["complex"].EventSceneId;
    }

    public Location? GetCurrentLocation() => IsInMainMenu ? null : Locations[CurrentLocationId];
    
    public Scene GetCurrentScene()
    {
        if (CurrentEventSceneId.HasValue)
        {
            return Scenes[CurrentEventSceneId.Value];
        }
        // Если нет текущей сцены, возвращаем сцену выбора локации
        return Scenes[0];
    }

    public bool CanTravelTo(string locationId)
    {
        if (IsInMainMenu) return false;
        var currentLocation = GetCurrentLocation();
        return currentLocation != null && currentLocation.CanTravelTo(locationId);
    }

    public void TravelToLocation(string locationId)
    {
        if (CanTravelTo(locationId))
        {
            CurrentLocationId = locationId;
            var location = Locations[locationId];
            
            // Показываем событие только если оно еще не было показано
            if (!ShownScenes.Contains(location.EventSceneId ?? 0))
            {
                CurrentEventSceneId = location.EventSceneId;
                IsChoosingLocation = false;
            }
            else
            {
                // Если событие уже было показано, сразу показываем выбор локации
                IsChoosingLocation = true;
                CurrentEventSceneId = null;
            }
        }
    }

    public void MakeChoice(string choice)
    {
        var currentScene = GetCurrentScene();
        if (currentScene.Choices.TryGetValue(choice, out int nextScene))
        {
            if (IsInMainMenu)
            {
                switch (nextScene)
                {
                    case -2: // Новая игра
                        StartNewGame();
                        return;
                    case -3: // Выход
                        ShouldExit = true;
                        return;
                }
            }

            // Обработка последствий выбора
            if (choice == "Заплатить (20 кредитов)") Money -= 20;
            if (choice == "Попытаться убежать (-30 здоровья)") Health -= 30;
            if (choice == "Купить медпак (30 кредитов, +50 здоровья)")
            {
                Money -= 30;
                Health += 50;
            }
            if (choice == "Обыскать станцию") Money += 20;

            // Если это выбор локации или последний выбор в сцене
            if (nextScene == 0)
            {
                // Отмечаем, что текущая сцена была показана
                if (CurrentEventSceneId.HasValue)
                {
                    ShownScenes.Add(CurrentEventSceneId.Value);
                }
                IsChoosingLocation = true;
                CurrentEventSceneId = null;
            }
            else
            {
                CurrentEventSceneId = nextScene;
            }
        }
    }
}
