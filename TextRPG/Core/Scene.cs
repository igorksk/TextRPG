#nullable enable

using System.Collections.Generic;

namespace TextRPG.Core
{
    public class Scene
    {
        public required string Text { get; init; }
        public required Dictionary<string, int> Choices { get; init; }
    }
}
