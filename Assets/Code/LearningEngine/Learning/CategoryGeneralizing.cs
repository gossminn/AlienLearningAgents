using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Algorithm for generalizing raw terminalCategories
    internal static class CategoryGeneralizing
    {
        // Merge contexts that are similar (have the same left or right part)
        public static TerminalCategorySet GeneralizeContexts(this TerminalCategorySet categorySet)
        {
            // Helper function: add new category
            Func<TerminalCategorySet, WordDistributionSet, TerminalCategorySet> addToSet =
                (acc, next) => acc.AddCategory(CategoryLabel.Create(NodeType.Terminal), next);

            // Construct initial merge helper object
            var emptyList = ImmutableList<WordDistributionSet>.Empty;
            var categories = categorySet.Categories.Values.ToImmutableList();
            var mergeState0 = new MergeStateHelper(false, categories, categories, emptyList);

            // Merge terminalCategories and add to new TerminalCategorySet
            var mergeState1 = MergeCategories(mergeState0);
            return mergeState1._done.Aggregate(TerminalCategorySet.CreateEmpty(), addToSet);
        }

        // Helper: check if the contexts of two terminalCategories are the same
        private static bool CompareContexts(WordDistributionSet cat1, WordDistributionSet cat2)
        {
            // Overlap in left contexts?
            if (cat1.LeftContext.Intersect(cat2.LeftContext).Any())
            {
                return true;
            }

            // Overlap in right contexts?
            if (cat1.RightContext.Intersect(cat2.RightContext).Any())
            {
                return true;
            }

            // Else: no match, direction undefined
            return false;
        }

        private static MergeStateHelper MergeCategories(MergeStateHelper helper)
        {
            // Get next merge state
            var transformed = NextMergeState(helper);
            return transformed._finished
                // If done, return that state
                ? transformed

                // Else: repeat
                : MergeCategories(transformed);
        }

        private static MergeStateHelper NextMergeState(MergeStateHelper state)
        {
            // _busy is empty? Return finished state
            if (state._busy.IsEmpty)
            {
                return new MergeStateHelper(true, state._busy, state._todo, state._done);
            }

            // _todo is empty? Mark first element of _busy as done, move rest to todo
            if (state._todo.IsEmpty)
            {
                var current = state._busy.First();
                var newList = state._busy.Remove(current);
                var done = state._done.Add(current);
                return new MergeStateHelper(false, newList, newList, done);
            }

            // _todo same as _busy
            if (state._todo.SequenceEqual(state._busy))
            {
                var current = state._todo.First();
                var todo = state._todo.Remove(current);
                return new MergeStateHelper(false, state._busy, todo, state._done);
            }

            // Compare and merge if appropriate
            else
            {
                var current = state._busy.First();
                var target = state._todo.First();

                // Check if the contexts of the two terminalCategories match
                if (CompareContexts(current, target))
                {
                    var merged = current.Merge(target);
                    var busy = state._busy
                        .Remove(current)
                        .Remove(target)
                        .Insert(0, merged);
                    var todo = state._todo.Remove(target);
                    return new MergeStateHelper(false, busy, todo, state._done);
                }
                else
                {
                    var todo = state._todo.Remove(target);
                    return new MergeStateHelper(false, state._busy, todo, state._done);

                }
            }
        }

        // Helper for storing intermediate merge results
        private struct MergeStateHelper
        {
            public readonly bool _finished;
            public readonly ImmutableList<WordDistributionSet> _busy;
            public readonly ImmutableList<WordDistributionSet> _todo;
            public readonly ImmutableList<WordDistributionSet> _done;

            public MergeStateHelper(bool finished, ImmutableList<WordDistributionSet> busy,
                ImmutableList<WordDistributionSet> todo, ImmutableList<WordDistributionSet> done)
            {
                _finished = finished;
                _busy = busy;
                _todo = todo;
                _done = done;
            }
        }
    }
}