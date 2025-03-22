#nullable enable

namespace TextRPG.Core.Models;

public class Scene
{
    public string Text { get; set; } = string.Empty;
    public Dictionary<string, (int nextSceneId, int healthChange, int moneyChange)> Choices { get; set; } = new();
} 