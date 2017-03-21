using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    static class TerminalLearning
    {
        public static KnowledgeSet UpdateTerminals(
            KnowledgeSet knowledge, SentenceMemory memory, string sentence)
        {
            /* Simplistic algorithm: add new category
             * for each word in sentence */
            var words = sentence.Split().ToImmutableList();

            // Terminal node with new category for each word
            var nodes = words.Select(x => 
                new TermNode(SyntaxCat.Create(CatType.Terminal), x, ""));

            // Add terminals to terminalset
            var terminals = nodes.Aggregate(
                knowledge.Terminals,
                (acc, next) => acc.AddTerminal(next));

            // Create terminal rules
            var rules = nodes
                // New rule with single node on right side
                .Select(x => TermRule.CreateEmpty(x.Category).AddToRight(x))
                // Add rules to RuleSet in knowledge base
                .Aggregate(knowledge.Rules,(acc, next) => acc.AddRule(next));

            // Add categories to categoryset
            var categories = nodes
                .Select(x => x.Category)
                .Aggregate(
                    knowledge.Categories, 
                    (acc, next) => acc.AddCategory(next));

            // Define new knowledge set
            var updatedKnowledge = knowledge
                .UpdateTerminals(terminals)
                .UpdateCategories(categories)
                .UpdateRules(rules);

            return updatedKnowledge;
        }
    }
}
