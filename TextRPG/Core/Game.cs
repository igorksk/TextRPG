#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace TextRPG.Core;

public class Game
{
    public bool IsInMainMenu { get; private set; }
    public bool ShouldExit { get; private set; }
    public bool IsChoosingLocation { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsVictory { get; private set; }
    public int Health { get; private set; }
    public int Money { get; private set; }
    public string CurrentLocationId { get; private set; } = string.Empty;
    public int CurrentEventSceneId { get; private set; }
    public IReadOnlyDictionary<string, Location> Locations => _locationManager.Locations;

    private const int VictoryMoneyAmount = 1000; // Сумма денег для победы

    private readonly SceneManager _sceneManager;
    private readonly LocationManager _locationManager;

    public Game()
    {
        IsInMainMenu = true;
        ShouldExit = false;
        IsChoosingLocation = false;
        IsDead = false;
        IsVictory = false;
        Health = 100;
        Money = 0;
        CurrentLocationId = string.Empty;
        CurrentEventSceneId = 0;

        _sceneManager = new SceneManager();
        _locationManager = new LocationManager();
    }

    public void StartNewGame()
    {
        IsInMainMenu = false;
        ShouldExit = false;
        IsChoosingLocation = false;
        IsDead = false;
        IsVictory = false;
        Health = 100;
        Money = 0;
        CurrentLocationId = "complex17";
        CurrentEventSceneId = 0;
        _sceneManager.ClearShownScenes();
    }

    public void MakeChoice(string choice)
    {
        var currentScene = GetCurrentScene();
        
        if (IsChoosingLocation)
        {
            // Если мы в режиме выбора локации, обрабатываем выбор локации
            if (_locationManager.GetLocation(choice) != null)
            {
                TravelToLocation(choice);
            }
            return;
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

            Health = Math.Max(0, Math.Min(100, Health + result.healthChange));
            Money += result.moneyChange;

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

    public Scene GetCurrentScene()
    {
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
            return SceneManager.GetLocationSelectionScene(availableLocations);
        }

        return _sceneManager.GetScene(CurrentEventSceneId);
    }

    public void ExitGame()
    {
        ShouldExit = true;
    }
}
