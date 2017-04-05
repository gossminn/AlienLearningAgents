using System;
using System.Linq;

namespace LearningEngine
{

    // Learning algorithms for acquiring syntactic categories
    static class CategorySetLearning
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
        // (Conservative version, may not generalize enough)
        private static CategorySet ProcessWord(CategorySet categories0, WordContext context)
        {
            // Helper: add word to a specific context
            Func<CategorySet, CategoryLabel, CategorySet> addWord =
                (categories, label) => categories
                    .UpdateWord(label, context.Word);

            // Get categories with matching left/right context
            var withLeftCtxt = categories0.FindLeftContext(context.Left);
            var withRightCtxt = categories0.FindRightContext(context.Right);
            var withBothCtxt = withLeftCtxt.Intersect(withRightCtxt);

            // If match: add word to context
            if (withBothCtxt.Any())
            {
                return withBothCtxt.Aggregate(categories0, addWord);
            }

            // Otherwise: add as new category
            var newCategory = CategoryKnowledge.Empty()
                .AddLeftContext(context.Left)
                .AddRightContext(context.Right)
                .AddWord(context.Word);

            return categories0.AddCategory(CategoryLabel.Create(NodeType.Terminal), newCategory);
        }
    }
}
