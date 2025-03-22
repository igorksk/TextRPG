#nullable enable

using TextRPG.Core.Models;

namespace TextRPG.Core;

public class SceneManager
{
    public Dictionary<int, Scene> Scenes { get; }
    private HashSet<int> ShownScenes { get; }

    public SceneManager()
    {
        Scenes = new Dictionary<int, Scene>();
        ShownScenes = new HashSet<int>();
        InitializeScenes();
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

        // Экран смерти (текст будет создаваться динамически)
        Scenes.Add(-4, new Scene
        {
            Text = "", // Пустой текст, будет заменен в GetCurrentScene
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Новая игра", (-2, 0, 0) },
                { "Выход", (-3, 0, 0) }
            }
        });

        // Экран победы (текст будет создаваться динамически)
        Scenes.Add(-5, new Scene
        {
            Text = "", // Пустой текст, будет заменен в GetCurrentScene
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

        // Сцена с сообщением об ошибке
        Scenes.Add(-6, new Scene
        {
            Text = "У вас недостаточно денег для этого действия.\n\n" +
                  "Выберите другое действие или вернитесь назад.",
            Choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>
            {
                { "Вернуться назад", (0, 0, 0) }
            }
        });
    }

    public Scene GetMainMenuScene() => Scenes[-1];

    public Scene GetDeathScene(int health, int money)
    {
        return new Scene
        {
            Text = "GAME OVER\n\nВы погибли в пустоши...\n\nВаши достижения:\n" +
                  $"Здоровье: {health}\n" +
                  $"Деньги: {money}\n\n" +
                  "Выберите действие:",
            Choices = Scenes[-4].Choices
        };
    }

    public Scene GetVictoryScene(int health, int money)
    {
        return new Scene
        {
            Text = "ПОБЕДА!\n\n" +
                  "Поздравляем! Вы накопили достаточно денег, чтобы начать новую жизнь в безопасном месте.\n\n" +
                  "Ваши достижения:\n" +
                  $"Здоровье: {health}\n" +
                  $"Деньги: {money}\n\n" +
                  "Выберите действие:",
            Choices = Scenes[-5].Choices
        };
    }

    public Scene GetLocationSelectionScene(IEnumerable<Location> availableLocations)
    {
        var choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>();
        foreach (var location in availableLocations)
        {
            choices[location.Name] = (0, 0, 0);
        }
        choices["Посмотреть квесты"] = (-7, 0, 0);

        return new Scene
        {
            Text = $"Выберите локацию для путешествия:\n\n{string.Join("\n", availableLocations.Select(l => $"- {l.Name}"))}",
            Choices = choices
        };
    }

    public Scene GetScene(int sceneId)
    {
        if (!Scenes.TryGetValue(sceneId, out var scene))
        {
            scene = Scenes[0];
        }

        // Добавляем опцию просмотра квестов во все игровые сцены
        if (sceneId >= 0) // Не добавляем в системные сцены (меню, смерть, победа)
        {
            var choices = new Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)>(scene.Choices)
            {
                { "Посмотреть квесты", (-7, 0, 0) }
            };
            return new Scene
            {
                Text = scene.Text,
                Choices = choices
            };
        }

        return scene;
    }

    public void MarkSceneAsShown(int sceneId)
    {
        ShownScenes.Add(sceneId);
    }

    public bool IsSceneShown(int sceneId)
    {
        return ShownScenes.Contains(sceneId);
    }

    public void ClearShownScenes()
    {
        ShownScenes.Clear();
    }
} 