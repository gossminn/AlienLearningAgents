using System.Linq;

namespace LearningEngine
{
    internal static class TerminalLearning
    {
        public static VocabularySet GenerateTerminals(this TerminalCategorySet categories)
        {
            // For each category, create a TermNode for every word
            var nodes =  categories.Categories.SelectMany(
                cat => cat.Value.Words.Select(
                    word => TermNode.Create(cat.Key, word, null)));

            // Add the TermNodes to an empty TerminalSet
            return VocabularySet.CreateEmpty().AddTerminals(nodes);
        }
    }
}