using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal static class ConstituentLearning
    {
        // Random number generator
        private static readonly Random _random = new Random();

        // Parse a word to get the category label
        private static CategoryLabel ParseTerminal(this RuleSet rules, string word)
        {
            // Check for empty string
            if (word == "")
                return CategoryLabel.EmptyCat;

            var parseInput = ImmutableList<string>.Empty.Add(word);
            var parseResults = rules.TerminalRules
                .Select(rule =>
                    rule.Parse(parseInput, rules, LogicalModel.CreateEmpty()))
                .ToList();
            return parseResults.First(result => result.Success).Tree.Category;
        }

        // Get sequence of category labels
        private static ImmutableArray<CategoryLabel> GetSequence(IEnumerable<string> words,
            RuleSet rules)
        {
            return words.Select(word => rules.ParseTerminal(word)).ToImmutableArray();
        }

        // Infer non-terminal categories and rules
        public static NonTerminalHelper InferFromInput(RuleSet rules, string[] words)
        {
            // Parse words to get sequence of CategoryLabels, then of Monograms
            var sequence = GetSequence(words, rules);

            // Infer categories from sequence
            return InferFromCatSequence(NonTerminalCategorySet.CreateEmpty(), 
                ImmutableHashSet<NonTermRule>.Empty, sequence);
        }

        private static NonTerminalHelper InferFromCatSequence(NonTerminalCategorySet newCategories0, 
            ImmutableHashSet<NonTermRule> newRules0, ImmutableArray<CategoryLabel> sequence0)
        {
            // Step 1: randomly group two categories together
            var num = _random.Next(sequence0.Length - 1);
            var firstElem = sequence0[num];
            var secondElem = sequence0[num + 1];

            // Step 2: create a new category, and a rule relating that category to the pair
            var category = CategoryLabel.Create(NodeType.NonTerminal);
            var rule = NonTermRule.CreateBinary(category, firstElem, secondElem, FunctorLoc.Left);
            var newCategories1 = newCategories0.AddCategory(category);
            var newRules1 = newRules0.Add(rule);

            // Step 3: get rest of the sequence (everything up until num, then the rest from num + 2)
            var before = sequence0.Take(num);
            var after = sequence0.Skip(num + 2);

            // Step 4: make new sequence, with grouped categories replaced by new category
            var sequence1 = before
                .Concat(ImmutableArray<CategoryLabel>.Empty.Add(category))
                .Concat(after)
                .ToImmutableArray();

            // Step 5: if only one element left, return, else do recursive call
            if (sequence1.Length > 1)
            {
                return InferFromCatSequence(newCategories1, newRules1, sequence1);
            }
            return NonTerminalHelper.Create(newCategories1, newRules1);
        }
    }
}