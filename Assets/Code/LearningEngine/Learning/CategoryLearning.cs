using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Learning algorithm for acquiring (raw) syntactic terminalCategories
    internal static class CategoryLearning
    {
        // Adapt overall CategorySet based on new input
        public static CategorySet ProcessInput(this CategorySet categories0, string sentence)
        {
            // Get context for each word
            var contexts = WordContext.GetContexts(sentence);

            // For each word, update the CategorySet, return final result
            return contexts.Aggregate(categories0, ProcessWord);
        }

        // Helper function: make intermediate state of CategorySet based on single word
        private static CategorySet ProcessWord(CategorySet categories0, WordContext context)
        {
            // Helper: add word to a specific context
            Func<CategorySet, CategoryLabel, CategorySet> addWord =
                (categories, label) => categories.UpdateWord(label, context.Word);

            // Helper: add context to a contextset
            Func<CategorySet, CategoryLabel, CategorySet> addContext =
                (categories, label) => categories
                    .UpdateLeftContext(label, context.Left)
                    .UpdateRightContext(label, context.Right);

            // Get terminalCategories with matching left/right context
            var withLeftCtxt = categories0.FindLeftContext(context.Left);
            var withRightCtxt = categories0.FindRightContext(context.Right);
            var withBothCtxt = withLeftCtxt.Intersect(withRightCtxt);

            // If match: add word to context
            var categoryLabels = withBothCtxt.ToImmutableList();
            if (categoryLabels.Any())
            {
                return categoryLabels.Aggregate(categories0, addWord);
            }

            // No match but word known: add context
            var withWord = categories0.FindWord(context.Word).ToImmutableList();
            if (withWord.Any())
            {
                return withWord.Aggregate(categories0, addContext);
            }

            // Otherwise: add as new category
            var newCategory = WordDistributionSet.Empty()
                .AddLeftContext(context.Left)
                .AddRightContext(context.Right)
                .AddWord(context.Word);

            return categories0.AddCategory(CategoryLabel.Create(NodeType.Terminal), newCategory);
        }
    }
}