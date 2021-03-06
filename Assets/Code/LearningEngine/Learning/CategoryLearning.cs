﻿using System;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Learning.LearningHelpers;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Learning
{
    // Learning algorithm for acquiring (raw) syntactic terminalCategories
    internal static class CategoryLearning
    {
        // Wrapper function: learn raw categories and generalize
        public static KnowledgeSet LearnCategories(this KnowledgeSet knowledge, ImmutableArray<string> words)
        {
            // Old category set
            var categories0 = knowledge.Categories;

            // Update raw categories based on input
            var rawCategories = categories0.RawTerminals.ExtractRawCategories(words);

            // Generalize based on input
            var generalizedCategories = rawCategories.GeneralizeContexts();

            // New category set
            var categories1 = categories0
                .UpdateRawTerminals(rawCategories)
                .UpdateGeneralizedTerminals(generalizedCategories);

            return knowledge.UpdateCategories(categories1);
        }

        // Adapt overall TerminalCategorySet based on new input
        public static TerminalCategorySet ExtractRawCategories(this TerminalCategorySet categories0, ImmutableArray<string> sentence)
        {
            // Get context for each word
            var contexts = WordContext.GetContexts(sentence);

            // For each word, update the TerminalCategorySet, return final result
            return contexts.Aggregate(categories0, ProcessWord);
        }

        // Helper function: make intermediate state of TerminalCategorySet based on single word
        private static TerminalCategorySet ProcessWord(TerminalCategorySet categories0, WordContext context)
        {
            // Helper: add word to a specific context
            Func<TerminalCategorySet, CategoryLabel, TerminalCategorySet> addWord =
                (categories, label) => categories.UpdateWord(label, context.Word);

            // Helper: add context to a contextset
            Func<TerminalCategorySet, CategoryLabel, TerminalCategorySet> addContext =
                (categories, label) => categories
                    .UpdateLeftContext(label, context.Left)
                    .UpdateRightContext(label, context.Right);

            // Find other words with a matching left/right context
            var withLeftCtxt = categories0.FindLeftContext(context.Left).ToImmutableList();
            var withRightCtxt = categories0.FindRightContext(context.Right).ToImmutableList();


            // Relevant contexts found?
            if (withLeftCtxt.Any() && withRightCtxt.Any())
            {
                var withBothCtxt = withLeftCtxt.Intersect(withRightCtxt);

                // If match: add word to context
                var categoryLabels = withBothCtxt.ToImmutableList();
                if (categoryLabels.Any())
                {
                    return categoryLabels.Aggregate(categories0, addWord);
                }
            }

            // Find other categories with the current word in them
            var withWord = categories0.FindWord(context.Word).ToImmutableList();

            // If there are any, add the context of the current word
            if (withWord.Any())
            {
                return withWord.Aggregate(categories0, addContext);
            }

            // Otherwise: add as new category
            var newCategory = WordDistributionSet.CreateEmpty()
                .AddLeftContext(context.Left)
                .AddRightContext(context.Right)
                .AddWord(context.Word);

            return categories0.AddCategory(CategoryLabel.Create(NodeType.Terminal), newCategory);
        }
    }
}