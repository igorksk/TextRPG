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
    public int Health { get; private set; }
    public int Money { get; private set; }
    public string CurrentLocationId { get; private set; } = string.Empty;
    public int CurrentEventSceneId { get; private set; }
    public Dictionary<string, Location> Locations { get; }
    public Dictionary<int, Scene> Scenes { get; }
    private HashSet<int> ShownScenes { get; }

    public Game()
    {
        IsInMainMenu = true;
        ShouldExit = false;
        IsChoosingLocation = false;
        IsDead = false;
        Health = 100;
        Money = 0;
        CurrentLocationId = string.Empty;
        CurrentEventSceneId = 0;
        Locations = new Dictionary<string, Location>();
        Scenes = new Dictionary<int, Scene>();
        ShownScenes = new HashSet<int>();

        InitializeLocations();
        InitializeScenes();
    }

    private void InitializeLocations()
    {
        // Подземный комплекс
        var complex17 = new Location("complex17", "Комплекс 17", 
            "Подземный бункер, построенный для защиты от ядерной войны. Теперь здесь находится ваша база.", 
            "Images/complex_hall.jpeg");
        complex17.AddConnection("surface");

        // Поверхность
        var surface = new Location("surface", "Поверхность", 
            "Пустошь, оставшаяся после ядерной войны. Радиация и мутанты сделали её опасной.", 
            "Images/desert_ruins.jpeg");
        surface.AddConnection("complex17");
        surface.AddConnection("lasvegas");
        surface.AddConnection("sanfrancisco");
        surface.AddConnection("repair_station");
        surface.AddConnection("trading_post");
        surface.AddConnection("bunker42");

        // Города
        var lasVegas = new Location("lasvegas", "Лас-Вегас", 
            "Оазис в пустоши, город греха и развлечений. Здесь можно найти всё, что душе угодно.", 
            "Images/lasvegas.jpeg");
        lasVegas.AddConnection("surface");

        var sanFrancisco = new Location("sanfrancisco", "Сан-Франциско", 
            "Город, переживший ядерную войну. Здесь процветает торговля и технологические инновации.", 
            "Images/sanfrancisco.jpeg");
        sanFrancisco.AddConnection("surface");

        // Технические локации
        var repairStation = new Location("repair_station", "Ремонтная станция", 
            "Заброшенная станция технического обслуживания. Здесь можно починить снаряжение и купить запчасти.", 
            "Images/repair_station.jpeg");
        repairStation.AddConnection("surface");

        var tradingPost = new Location("trading_post", "Торговая застава", 
            "Караванная стоянка, где можно купить и продать товары. Здесь всегда много путешественников.", 
            "Images/trading_post.jpeg");
        tradingPost.AddConnection("surface");

        var bunker42 = new Location("bunker42", "Бункер 42", 
            "Заброшенный подземный бункер. Говорят, здесь проводились секретные эксперименты.", 
            "Images/bunker42.jpeg");
        bunker42.AddConnection("surface");

        Locations.Add(complex17.Id, complex17);
        Locations.Add(surface.Id, surface);
        Locations.Add(lasVegas.Id, lasVegas);
        Locations.Add(sanFrancisco.Id, sanFrancisco);
        Locations.Add(repairStation.Id, repairStation);
        Locations.Add(tradingPost.Id, tradingPost);
        Locations.Add(bunker42.Id, bunker42);
    }

    private void InitializeScenes()
    {
        // Главное меню
        Scenes.Add(-1, new Scene
        {
            Text = "Текстовая RPG\n\nДобро пожаловать в игру!\nВыберите действие:",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Новая игра", (-2, 0, 0) },
                { "Выход", (-3, 0, 0) }
            }
        });

        // Экран смерти
        Scenes.Add(-4, new Scene
        {
            Text = "GAME OVER\n\nВы погибли в пустоши...\n\nВаши достижения:\n" +
                  $"Здоровье: {Health}\n" +
                  $"Деньги: {Money}\n\n" +
                  "Выберите действие:",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Новая игра", (-2, 0, 0) },
                { "Выход", (-3, 0, 0) }
            }
        });

        // Сцена выбора локации
        Scenes.Add(0, new Scene
        {
            Text = "Куда вы хотите отправиться?",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Выбрать локацию", (0, 0, 0) }
            }
        });

        // Сцены для Лас-Вегаса
        Scenes.Add(1, new Scene
        {
            Text = "Вы входите в Лас-Вегас. Город сияет неоновыми огнями, а вокруг слышны звуки музыки и смеха. " +
                  "Здесь можно найти всё, что душе угодно - от редких предметов до опасных заданий.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Посетить казино", (2, -10, 50) },
                { "Найти работу", (3, -5, 30) },
                { "Вернуться на поверхность", (0, 0, 0) }
            }
        });

        Scenes.Add(2, new Scene
        {
            Text = "В казино вы играете в рулетку и выигрываете немного денег. " +
                  "Но азарт заставляет вас играть дальше, и вы теряете часть здоровья.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Продолжить играть", (2, -15, 100) },
                { "Уйти", (0, 0, 0) }
            }
        });

        Scenes.Add(3, new Scene
        {
            Text = "Вы находите работу охранника в одном из казино. " +
                  "Работа опасная, но хорошо оплачивается.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Принять работу", (4, -20, 100) },
                { "Отказаться", (0, 0, 0) }
            }
        });

        Scenes.Add(4, new Scene
        {
            Text = "Во время работы вы сталкиваетесь с вооружёнными грабителями. " +
                  "Вам удаётся отбиться, но вы получаете ранения.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Получить бонус", (0, -10, 50) },
                { "Уйти с работы", (0, 0, 0) }
            }
        });

        // Сцены для Сан-Франциско
        Scenes.Add(5, new Scene
        {
            Text = "Сан-Франциско встретил вас шумом рынка и запахом моря. " +
                  "Здесь можно найти редкие технологии и ценные артефакты.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Исследовать рынок", (6, 0, -20) },
                { "Найти контакты", (7, -5, 30) },
                { "Вернуться на поверхность", (0, 0, 0) }
            }
        });

        Scenes.Add(6, new Scene
        {
            Text = "На рынке вы находите редкие запчасти и технологии. " +
                  "Торговец предлагает выгодную сделку.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Купить запчасти", (0, 0, -50) },
                { "Продать находки", (0, 0, 100) }
            }
        });

        Scenes.Add(7, new Scene
        {
            Text = "Вы находите контакты местных торговцев. " +
                  "Они предлагают вам участие в прибыльной сделке.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Согласиться", (8, -15, 150) },
                { "Отказаться", (0, 0, 0) }
            }
        });

        Scenes.Add(8, new Scene
        {
            Text = "Сделка оказывается опасной, но прибыльной. " +
                  "Вы получаете хорошую награду, но теряете часть здоровья.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Получить награду", (0, -10, 100) },
                { "Уйти", (0, 0, 0) }
            }
        });

        // Сцены для ремонтной станции
        Scenes.Add(9, new Scene
        {
            Text = "Ремонтная станция выглядит заброшенной, но внутри вы находите работающее оборудование. " +
                  "Механик предлагает вам свои услуги.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Починить снаряжение", (10, 0, -30) },
                { "Купить запчасти", (11, 0, -20) },
                { "Уйти", (0, 0, 0) }
            }
        });

        Scenes.Add(10, new Scene
        {
            Text = "Механик чинит ваше снаряжение. " +
                  "Теперь оно в отличном состоянии.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Поблагодарить", (0, 0, 0) }
            }
        });

        Scenes.Add(11, new Scene
        {
            Text = "Вы покупаете полезные запчасти. " +
                  "Они могут пригодиться в будущем.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Уйти", (0, 0, 0) }
            }
        });

        // Сцены для торговой заставы
        Scenes.Add(12, new Scene
        {
            Text = "На торговой заставе кипит жизнь. " +
                  "Здесь можно найти редкие товары и полезную информацию.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Торговать", (13, 0, 0) },
                { "Собрать информацию", (14, 0, 0) },
                { "Уйти", (0, 0, 0) }
            }
        });

        Scenes.Add(13, new Scene
        {
            Text = "Вы успешно торгуете с караванщиками. " +
                  "Получаете хорошую прибыль.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Продолжить", (13, 0, 50) },
                { "Уйти", (0, 0, 0) }
            }
        });

        Scenes.Add(14, new Scene
        {
            Text = "Вы узнаёте о ценных находках в окрестностях. " +
                  "Эта информация может пригодиться.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Заплатить за подробности", (0, 0, -20) },
                { "Уйти", (0, 0, 0) }
            }
        });

        // Сцены для Бункера 42
        Scenes.Add(15, new Scene
        {
            Text = "Бункер 42 выглядит заброшенным, но внутри вы находите следы недавней активности. " +
                  "Здесь может быть опасно, но и награда может быть велика.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Исследовать", (16, -10, 0) },
                { "Вернуться", (0, 0, 0) }
            }
        });

        Scenes.Add(16, new Scene
        {
            Text = "Внутри бункера вы находите ценные артефакты и документы. " +
                  "Но также сталкиваетесь с опасными мутантами.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Собрать находки", (0, -20, 200) },
                { "Убежать", (0, -10, 0) }
            }
        });
    }

    public void StartNewGame()
    {
        IsInMainMenu = false;
        ShouldExit = false;
        IsChoosingLocation = false;
        IsDead = false;
        Health = 100;
        Money = 0;
        CurrentLocationId = "complex17";
        CurrentEventSceneId = 0;
        ShownScenes.Clear();
    }

    public void MakeChoice(string choice)
    {
        var currentScene = GetCurrentScene();
        
        if (IsChoosingLocation)
        {
            // Если мы в режиме выбора локации, обрабатываем выбор локации
            if (Locations.TryGetValue(choice, out var location))
            {
                TravelToLocation(choice);
            }
            return;
        }

        if (currentScene.Choices.TryGetValue(choice, out var result))
        {
            if (IsInMainMenu || IsDead)
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
        if (Locations.TryGetValue(locationId, out var location))
        {
            CurrentLocationId = locationId;
            IsChoosingLocation = false;

            // Устанавливаем начальную сцену для локации
            switch (locationId)
            {
                case "lasvegas":
                    CurrentEventSceneId = 1;
                    break;
                case "sanfrancisco":
                    CurrentEventSceneId = 5;
                    break;
                case "repair_station":
                    CurrentEventSceneId = 9;
                    break;
                case "trading_post":
                    CurrentEventSceneId = 12;
                    break;
                case "bunker42":
                    CurrentEventSceneId = 15;
                    break;
                default:
                    CurrentEventSceneId = 0;
                    break;
            }
        }
    }

    public bool CanTravelTo(string locationId)
    {
        if (CurrentLocationId == "surface")
        {
            return true;
        }

        return Locations.TryGetValue(CurrentLocationId, out var currentLocation) &&
               currentLocation.ConnectedLocations.Contains(locationId);
    }

    public Location? GetCurrentLocation()
    {
        return Locations.TryGetValue(CurrentLocationId, out var location) ? location : null;
    }

    public Scene GetCurrentScene()
    {
        if (IsDead)
        {
            return Scenes[-4];
        }

        if (IsInMainMenu)
        {
            return Scenes[-1];
        }

        if (IsChoosingLocation)
        {
            var currentLocation = GetCurrentLocation();
            if (currentLocation != null)
            {
                var availableLocations = currentLocation.ConnectedLocations
                    .Where(id => Locations.ContainsKey(id))
                    .Select(id => Locations[id])
                    .ToList();

                var choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>();
                foreach (var location in availableLocations)
                {
                    choices[location.Name] = (0, 0, 0);
                }

                return new Scene
                {
                    Text = $"Выберите локацию для путешествия:\n\n{string.Join("\n", availableLocations.Select(l => $"- {l.Name}"))}",
                    Choices = choices
                };
            }
        }

        return Scenes.TryGetValue(CurrentEventSceneId, out var scene) ? scene : Scenes[0];
    }

    public void ExitGame()
    {
        ShouldExit = true;
    }
}
