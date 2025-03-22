#nullable enable

using TextRPG.Core.Models;

namespace TextRPG.Core;

public class Game
{
    public bool IsInMainMenu { get; private set; }
    public bool ShouldExit { get; private set; }
    public bool IsChoosingLocation { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsVictory { get; private set; }
    public bool IsViewingQuests { get; private set; }
    public int Health { get; private set; }
    public int Money { get; private set; }
    public string CurrentLocationId { get; private set; } = string.Empty;
    public int CurrentEventSceneId { get; private set; }
    public IReadOnlyDictionary<string, Location> Locations => _locationManager.Locations;

    private const int VictoryMoneyAmount = 1000; // Сумма денег для победы

    private readonly SceneManager _sceneManager;
    private readonly LocationManager _locationManager;
    private readonly QuestManager _questManager;

    public Game()
    {
        IsInMainMenu = true;
        ShouldExit = false;
        IsChoosingLocation = false;
        IsDead = false;
        IsVictory = false;
        IsViewingQuests = false;
        Health = 100;
        Money = 0;
        CurrentLocationId = string.Empty;
        CurrentEventSceneId = 0;

        _sceneManager = new SceneManager();
        _locationManager = new LocationManager();
        _questManager = new QuestManager();
    }

    public void StartNewGame()
    {
        IsInMainMenu = false;
        ShouldExit = false;
        IsChoosingLocation = false;
        IsDead = false;
        IsVictory = false;
        IsViewingQuests = false;
        Health = 100;
        Money = 0;
        CurrentLocationId = "complex17";
        CurrentEventSceneId = 0;
        _sceneManager.ClearShownScenes();
        _questManager.Reset();
    }

    public void MakeChoice(string choice)
    {
        var currentScene = GetCurrentScene();
        
        if (IsChoosingLocation)
        {
            if (choice == "Посмотреть квесты")
            {
                IsViewingQuests = true;
                return;
            }

            // Если мы в режиме выбора локации, обрабатываем выбор локации
            if (_locationManager.GetLocation(choice) != null)
            {
                TravelToLocation(choice);
            }
            return;
        }

        if (IsViewingQuests)
        {
            if (choice == "Вернуться")
            {
                IsViewingQuests = false;
                return;
            }
        }

        if (currentScene.Choices.TryGetValue(choice, out var result))
        {
            if (IsInMainMenu || IsDead || IsVictory)
            {
                switch (result.nextSceneId)
                {
                    case -2: // Новая игра
                        StartNewGame();
                        return;
                    case -3: // Выход
                        ShouldExit = true;
                        return;
                }
            }

            if (choice == "Посмотреть квесты")
            {
                IsViewingQuests = true;
                return;
            }

            // Проверяем, достаточно ли денег для действия
            if (result.moneyChange < 0 && Money + result.moneyChange < 0)
            {
                // Если денег не хватает, показываем сообщение об ошибке
                CurrentEventSceneId = -6; // ID сцены с сообщением об ошибке
                return;
            }

            Health = Math.Max(0, Math.Min(100, Health + result.healthChange));
            Money += result.moneyChange;

            // Проверяем прогресс квестов
            _questManager.CheckQuestProgress(CurrentLocationId, Money);

            // Проверяем смерть
            if (Health <= 0)
            {
                IsDead = true;
                CurrentEventSceneId = -4;
                return;
            }

            // Проверяем победу
            if (Money >= VictoryMoneyAmount)
            {
                IsVictory = true;
                CurrentEventSceneId = -5;
                return;
            }

            if (result.nextSceneId == 0)
            {
                IsChoosingLocation = true;
            }
            else
            {
                CurrentEventSceneId = result.nextSceneId;
            }
        }
    }

    public void TravelToLocation(string locationId)
    {
        if (_locationManager.GetLocation(locationId) != null)
        {
            CurrentLocationId = locationId;
            IsChoosingLocation = false;
            CurrentEventSceneId = LocationManager.GetInitialSceneId(locationId);
        }
    }

    public bool CanTravelTo(string locationId)
    {
        return _locationManager.CanTravelTo(CurrentLocationId, locationId);
    }

    public Location? GetCurrentLocation()
    {
        return _locationManager.GetLocation(CurrentLocationId);
    }

    public void ViewQuests()
    {
        IsViewingQuests = true;
    }

    public Scene GetCurrentScene()
    {
        if (IsViewingQuests)
        {
            var quests = _questManager.GetAllQuests();
            var questText = "Список квестов:\n\n";
            
            foreach (var quest in quests)
            {
                var status = quest.IsCompleted ? "[Выполнен]" : "[Активен]";
                var type = quest.IsMainQuest ? "[Главный]" : "[Второстепенный]";
                questText += $"{type} {status} {quest.Name}\n";
                questText += $"Описание: {quest.Description}\n";
                if (quest.RequiredMoney > 0)
                {
                    questText += $"Требуется денег: {quest.RequiredMoney}\n";
                }
                questText += $"Награда: {quest.RewardMoney} монет\n\n";
            }

            return new Scene
            {
                Text = questText,
                Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
                {
                    { "Вернуться", (CurrentEventSceneId, 0, 0) }
                }
            };
        }

        if (IsVictory)
        {
            return _sceneManager.GetVictoryScene(Health, Money);
        }

        if (IsDead)
        {
            return _sceneManager.GetDeathScene(Health, Money);
        }

        if (IsInMainMenu)
        {
            return _sceneManager.GetMainMenuScene();
        }

        if (IsChoosingLocation)
        {
            var availableLocations = _locationManager.GetAvailableLocations(CurrentLocationId);
            return _sceneManager.GetLocationSelectionScene(availableLocations);
        }

        var scene = _sceneManager.GetScene(CurrentEventSceneId);

        // Проверяем, есть ли уведомления о выполненных квестах
        var questNotifications = _questManager.GetAndClearCompletedQuestNotifications();
        if (questNotifications.Any())
        {
            var notificationText = string.Join("\n", questNotifications);
            return new Scene
            {
                Text = $"{notificationText}\n\n{scene.Text}",
                Choices = scene.Choices
            };
        }

        return scene;
    }

    public void ExitGame()
    {
        ShouldExit = true;
    }
}
