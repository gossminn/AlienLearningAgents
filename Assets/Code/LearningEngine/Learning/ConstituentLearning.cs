using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal static class ConstituentLearning
    {
        public static CategorySet ProcessInput(this RuleSet rules, 
            CategorySet categories, string input)
        {
            // Split input into 1-element lists
            var words = input.Split()
                .Select(word => ImmutableList<string>.Empty.Add(word));

            // Get category labels
            var parsed = words
                .SelectMany(word => rules.Rules
                    .Select(rule => rule.Parse(word, rules)))
                .Where(result => result.Success)
                .Select(result => result.Tree.Category)
                .ToImmutableArray();

            // Get contexts
            var contexts = ConstituentContext.GetContexts(parsed);

            return categories;
        }
    }
}