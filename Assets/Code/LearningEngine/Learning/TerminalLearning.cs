using System.Linq;

namespace LearningEngine
{
    internal static class TerminalLearning
    {
        public static TerminalSet GenerateTerminals(this CategorySet categories)
        {
            // For each category, create a TermNode for every word
            var nodes =  categories.TerminalCategories.SelectMany(
                cat => cat.Value.Words.Select(
                    word => TermNode.Create(cat.Key, word, "")));

            // Add the TermNodes to an empty TerminalSet
            return TerminalSet.CreateEmpty().AddTerminals(nodes);
        }
    }
}