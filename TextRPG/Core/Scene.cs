namespace TextRPG.Core
{
    public class Scene
    {
        public string? Text { get; set; }
        public Dictionary<string, int> Choices { get; set; } = [];
    }
}
