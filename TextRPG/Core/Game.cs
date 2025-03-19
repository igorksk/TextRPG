namespace TextRPG.Core
{
    public class Game
    {
        public Dictionary<int, Scene> Scenes { get; private set; }
        public int CurrentScene { get; private set; }

        public Game()
        {
            Scenes = new Dictionary<int, Scene>
        {
            { 1, new Scene
                {
                    Text = "Вы — житель Убежища 27. Вам поручено починить генератор воды, но для этого нужно покинуть Убежище. Что вы делаете?",
                    Choices = new Dictionary<string, int>
                    {
                        { "Выйти наружу", 2 },
                        { "Остаться и спросить совета у Смотрителя", 3 }
                    }
                }
            },
            { 2, new Scene
                {
                    Text = "Вы выходите на поверхность. Солнце слепит глаза, а радиация делает воздух плотным. Вдалеке виднеется разрушенный город и старая заправка. Куда пойдёте?",
                    Choices = new Dictionary<string, int>
                    {
                        { "Идти в город", 4 },
                        { "Проверить заправку", 5 }
                    }
                }
            },
            { 3, new Scene
                {
                    Text = "Смотритель даёт вам 50 крышек и карту окрестностей. Вы чувствуете себя увереннее.",
                    Choices = new Dictionary<string, int>
                    {
                        { "Выйти наружу", 2 }
                    }
                }
            },
            { 4, new Scene
                {
                    Text = "В городе вас встречает банда рейдеров. Они требуют все ваши припасы. Что делать?",
                    Choices = new Dictionary<string, int>
                    {
                        { "Отдать крышки", 6 },
                        { "Попытаться убежать", 7 }
                    }
                }
            },
            { 5, new Scene
                {
                    Text = "На заправке вы находите старого торговца. Он предлагает купить у него лазерный пистолет за 40 крышек.",
                    Choices = new Dictionary<string, int>
                    {
                        { "Купить пистолет", 8 },
                        { "Отказать и уйти", 9 }
                    }
                }
            },
            { 6, new Scene
                {
                    Text = "Рейдеры смеются, забирают крышки и прогоняют вас. Без денег вам будет труднее выжить.",
                    Choices = new Dictionary<string, int>
                    {
                        { "Продолжить путь в город", 10 }
                    }
                }
            },
            { 7, new Scene
                {
                    Text = "Вы бросаетесь бежать, но рейдеры открывают огонь. Вас ранило, но вы смогли скрыться.",
                    Choices = new Dictionary<string, int>
                    {
                        { "Идти дальше в город", 10 }
                    }
                }
            },
            { 8, new Scene
                {
                    Text = "Вы покупаете лазерный пистолет. Теперь у вас есть чем защититься!",
                    Choices = new Dictionary<string, int>
                    {
                        { "Продолжить путь", 10 }
                    }
                }
            },
            { 9, new Scene
                {
                    Text = "Вы решаете не тратить крышки и уходите дальше.",
                    Choices = new Dictionary<string, int>
                    {
                        { "Идти в город", 10 }
                    }
                }
            },
            { 10, new Scene
                {
                    Text = "Вы углубляетесь в пустошь, зная, что приключение только начинается...",
                    Choices = []
                }
            }
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
}
