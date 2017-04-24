using System.Linq;

namespace LearningEngine
{
    internal static class TerminalLearning
    {
        // Wrapper function: learn terminals and return knowledge set
        public static KnowledgeSet LearnTerminals(this KnowledgeSet knowledge)
        {
            // Generalized terminal categories
            var categories = knowledge.Categories.GeneralizedTerminals;

            // Generate terminal nodes for each category
            var nodes = categories.GenerateTerminals();

            // Extract terminal rules based on these nodes
            var rules = nodes.ExtractRules();

            // Add rules to empty rule set and return new knowledge set
            return knowledge.UpdateRules(RuleSet.CreateEmpty().AddRules(rules));
        }

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