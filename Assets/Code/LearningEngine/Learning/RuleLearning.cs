using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    static class NonTerminalLearning
    {
        public static KnowledgeSet UpdateNonTerms(
            KnowledgeSet knowledge, SentenceMemory memory, string sentence)
        {
            /* Simplistic algorithm: 'flat' binary structure */
            // Try to parse each individual word in the sentence
            var words = sentence.Split().ToImmutableList();
            var n = words.Count;

            var termNodes = words
                .Select(x => knowledge
                    .Terminals.Collection
                    .Where(y => y.GetFlatString() == x)
                    .First())
                .ToImmutableList();

            var nonTermCats = Enumerable.Range(0, n - 2)
                .Select(x => CategoryLabel.Create(NodeType.NonTerminal))
                .ToImmutableList().Insert(0, knowledge.Categories.Root);

            var categories = nonTermCats
                .Aggregate(knowledge.Categories,
                (acc, next) => acc.AddCategory(next));

            var rules = Enumerable.Range(0, n - 2)
                .Select(i => NonTermRule.CreateBinary(
                    nonTermCats[i], termNodes[i].Category, 
                    nonTermCats[i + 1], FunctorLoc.Left))
                .ToImmutableList()
                .Add(NonTermRule.CreateBinary(
                    nonTermCats[n - 2], termNodes[n - 2].Category,
                    termNodes[n - 1].Category, FunctorLoc.Left))
                .AsEnumerable()
                .Aggregate(knowledge.Rules, 
                    (acc, next) => acc.AddRule(next));

            return knowledge
                .UpdateCategories(categories)
                .UpdateRules(rules);

        }
    }
}
