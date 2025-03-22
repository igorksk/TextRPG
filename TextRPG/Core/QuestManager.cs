#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace TextRPG.Core;

public class QuestManager
{
    private readonly Dictionary<string, Quest> _quests;

    public QuestManager()
    {
        _quests = new Dictionary<string, Quest>();
        InitializeQuests();
    }

    private void InitializeQuests()
    {
        // Главный квест
        _quests.Add("main_quest", new Quest(
            "main_quest",
            "Новая жизнь",
            "Соберите 1000 монет, чтобы начать новую жизнь в безопасном месте.",
            true,
            500,
            1000
        ));

        // Второстепенные квесты
        _quests.Add("vegas_quest", new Quest(
            "vegas_quest",
            "Большая игра",
            "Заработайте 300 монет в казино Лас-Вегаса.",
            false,
            100,
            300
        ));

        _quests.Add("bunker_quest", new Quest(
            "bunker_quest",
            "Тайны бункера",
            "Исследуйте Бункер 42 и найдите секретные документы.",
            false,
            200
        ));
    }

    public IEnumerable<Quest> GetAllQuests()
    {
        return _quests.Values;
    }

    public IEnumerable<Quest> GetActiveQuests()
    {
        return _quests.Values.Where(q => !q.IsCompleted);
    }

    public void CheckQuestProgress(string locationId, int money)
    {
        // Проверяем главный квест
        var mainQuest = _quests["main_quest"];
        if (!mainQuest.IsCompleted && money >= mainQuest.RequiredMoney)
        {
            mainQuest.Complete();
        }

        // Проверяем квест казино
        var vegasQuest = _quests["vegas_quest"];
        if (!vegasQuest.IsCompleted && locationId == "lasvegas" && money >= vegasQuest.RequiredMoney)
        {
            vegasQuest.Complete();
        }

        // Проверяем квест бункера
        var bunkerQuest = _quests["bunker_quest"];
        if (!bunkerQuest.IsCompleted && locationId == "bunker42")
        {
            bunkerQuest.Complete();
        }
    }

    public int GetQuestReward(string questId)
    {
        if (_quests.TryGetValue(questId, out var quest) && quest.IsCompleted)
        {
            return quest.RewardMoney;
        }
        return 0;
    }

    public void Reset()
    {
        _quests.Clear();
        InitializeQuests();
    }
} 