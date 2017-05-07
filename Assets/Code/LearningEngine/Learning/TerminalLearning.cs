using System.Linq;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Knowledge.MeaningHypotheses;
using Code.LearningEngine.Knowledge.Rules;
using Code.LearningEngine.Trees;

namespace Code.LearningEngine.Learning
{
    internal static class TerminalLearning
    {
        // Wrapper function: learn terminals and return knowledge set
        public static KnowledgeSet LearnTerminals(this KnowledgeSet knowledge, MeaningHypothesisSet guess)
        {
            // Generalized terminal categories
            var categories = knowledge.Categories.GeneralizedTerminals;

            // Generate terminal nodes for each category
            var nodes = categories.GenerateTerminals(guess);

            // Extract terminal rules based on these nodes
            var rules = nodes.ExtractRules();

            // Add rules to empty rule set and return new knowledge set
            return knowledge.UpdateRules(RuleSet.CreateEmpty().AddRules(rules));
        }

        public static VocabularySet GenerateTerminals(this TerminalCategorySet categories, MeaningHypothesisSet guess)
        {
            // For each category, create a TermNode for every word (with empty meaning)
            var nodes =  categories.Categories.SelectMany(
                cat => cat.Value.Words.Select(
                    word => TermNode.Create(cat.Key, word, guess.FindMeaningFor(word).Meaning)));

            // Add the TermNodes to an empty TerminalSet
            return VocabularySet.CreateEmpty().AddTerminals(nodes);
        }
    }
}