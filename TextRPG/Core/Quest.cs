#nullable enable

namespace TextRPG.Core;

public class Quest
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public bool IsMainQuest { get; }
    public bool IsCompleted { get; private set; }
    public int RewardMoney { get; }
    public int RequiredMoney { get; }

    public Quest(string id, string name, string description, bool isMainQuest, int rewardMoney, int requiredMoney = 0)
    {
        Id = id;
        Name = name;
        Description = description;
        IsMainQuest = isMainQuest;
        IsCompleted = false;
        RewardMoney = rewardMoney;
        RequiredMoney = requiredMoney;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
} 