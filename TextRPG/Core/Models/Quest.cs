#nullable enable

namespace TextRPG.Core.Models;

public class Quest
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public bool IsMainQuest { get; }
    public bool IsCompleted { get; private set; }
    public int RequiredMoney { get; }
    public int RewardMoney { get; }

    public Quest(string id, string name, string description, bool isMainQuest, int rewardMoney, int requiredMoney = 0)
    {
        Id = id;
        Name = name;
        Description = description;
        IsMainQuest = isMainQuest;
        RequiredMoney = requiredMoney;
        RewardMoney = rewardMoney;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
} 