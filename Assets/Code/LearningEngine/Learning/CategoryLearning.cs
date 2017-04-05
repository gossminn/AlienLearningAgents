using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Learning algorithms for acquiring syntactic categories
    static class CategorySetLearning
    {
        // Adapt overall CategorySet based on new input
        public static CategorySet ProcessSentence(this CategorySet categories0, string sentence)
        {
            // Get context for each word
            var contexts = WordContext.GetContexts(sentence);

            // For each word, update the CategorySet, return final result
            return contexts.Aggregate(categories0, ProcessWord);
        }

        // Helper function: make intermediate state of CategorySet based on single word
        private static CategorySet ProcessWord(CategorySet categories0, WordContext context)
        {
            // Helper function: add both left and right context
            Func<CategorySet, CategoryLabel, CategorySet> addContext =
                (categories, label) => categories
                    .UpdateLeftContext(label, context.Left)
                    .UpdateRightContext(label, context.Right);

            // Helper function: add word
            Func<CategorySet, CategoryLabel, CategorySet> addWord =
                (categories, label) => categories
                    .UpdateWord(label, context.Word);

            // Case 1: if word is known, add the context
            var withWord = categories0.FindWord(context.Word);
            if (withWord.Any())
            {
                // For each entry, yield CategorySet with updated contexts
                return withWord.Aggregate(categories0, addContext);
            }

            // Case 2: if context is known, add the word
            var withLeftCtxt = categories0.FindLeftContext(context.Left);
            var withRightCtxt = categories0.FindRightContext(context.Right);
            var anyLeft = withLeftCtxt.Any();
            var anyRight = withRightCtxt.Any();

            if (anyLeft || anyRight)
            {
                // Add left contexts
                var categories1 = anyLeft
                    ? withLeftCtxt.Aggregate(categories0, addWord)
                    : categories0;

                // Add right contexts
                var categories2 = anyRight
                    ? withRightCtxt.Aggregate(categories1, addWord)
                    : categories1;

                return categories2;
            }

            // Case 3: otherwise, add new category
            var newCategory = CategoryKnowledge.Empty()
                .AddLeftContext(context.Left)
                .AddRightContext(context.Right)
                .AddWord(context.Word);

            return categories0.AddCategory(CategoryLabel.Create(NodeType.Terminal), newCategory);
        }

    }
}
