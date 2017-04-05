using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Algorithm for generalizing raw categories
    static class CategoryGeneralizing
    {
        private struct CategoryPair
        {
            // First category
            public readonly CategoryKnowledge First;

            // Second category
            public readonly CategoryKnowledge Second;

            // Constructor
            public CategoryPair(CategoryKnowledge first, CategoryKnowledge second)
            {
                First = first;
                Second = second;
            }
        }

        // Helper for keeping lists together during the merging process
        private struct MergeHelper
        {
            // Old versions of merged pairs
            public readonly ImmutableHashSet<CategoryKnowledge> Done;

            // Merged versions
            public readonly ImmutableHashSet<CategoryKnowledge> Merged;

            // Pairs that remain independent
            public readonly ImmutableHashSet<CategoryKnowledge> Singletons;

            // Constructor
            public MergeHelper(ImmutableHashSet<CategoryKnowledge> done,
                ImmutableHashSet<CategoryKnowledge> merged,
                ImmutableHashSet<CategoryKnowledge> singletons)
            {
                Done = done;
                Merged = merged;
                Singletons = singletons;
            }
        }

        // Helper: check for subset relation in a pair of CategoryKnowledge
        private static bool HasSubsetRelation(CategoryPair pair)
        {
            var firstIsSubset = pair.First.Words.All(pair.Second.Words.Contains);
            var secondIsSubset = pair.Second.Words.All(pair.First.Words.Contains);
            return firstIsSubset || secondIsSubset;
        }

        // Helper: do a merger based on a CategoryPair and a MergeHelper
        private static CategoryKnowledge MergePair(MergeHelper helper, CategoryPair pair)
        {
            // First category already in merger?
            if (helper.Done.Contains(pair.First))
            {
                // Merge second with merger containing first
                var merger = helper.Merged
                    .Where(x => pair.First.Words.All(y => x.Words.Contains(y)))
                    .First();
                return pair.Second.Merge(merger);
            }

            // Second category already in merger?
            if (helper.Done.Contains(pair.Second))
            {
                // Merge first with merger containing second
                var merger = helper.Merged
                    .Where(x => pair.Second.Words.All(y => x.Words.Contains(y)))
                    .First();
                return pair.First.Merge(merger);
            }

            // No merge exists? Merge with each other
            return pair.First.Merge(pair.Second);
        }

        // Merge or keep singletons
        private static MergeHelper ProcessPairs(MergeHelper helper, CategoryPair pair)
        {
            // Helper function: find duplicate mergers
            Predicate<CategoryKnowledge> isDuplicate =
                cat1 => helper.Merged.Any(cat2 => CategoryKnowledge.AreEquivalent(cat1, cat2));

            // If no subset relation: add both parts of pair as singletons
            if (!HasSubsetRelation(pair))
            {
                // If first part of pair is already part of merger, skip it
                var singletons1 = helper.Done.Contains(pair.First)
                    ? helper.Singletons
                    : helper.Singletons.Add(pair.First);

                // Idem dito for second part of pair
                var singletons2 = helper.Done.Contains(pair.Second)
                    ? singletons1
                    : singletons1.Add(pair.Second);

                return new MergeHelper(helper.Done, helper.Merged, singletons2);
            }

            // If there is subset relation: merge, and remove from singletons
            else
            {
                // Remove both parts of pair from singletons (if applicable)
                var singletons = helper.Singletons
                    .Remove(pair.First)
                    .Remove(pair.Second);

                // Merge the pair, add to merged if this merger does not exist already
                var merger = MergePair(helper, pair);
                var merged = isDuplicate(merger)
                    ? helper.Merged
                    : helper.Merged.Add(merger);

                // Add both parts of pair to done
                var done = helper.Done.Add(pair.First).Add(pair.Second);

                return new MergeHelper(done, merged, singletons);
            }
        }

        // Generalize similar categories
        public static CategorySet Generalize(this CategorySet categories0)
        {
            // All pairs of CategoryKnowledge
            var pairs = categories0.Categories
                .SelectMany(x => categories0.Categories
                    .Where(y => x.Value != y.Value)
                    .Select(y => new CategoryPair(x.Value, y.Value)));

            // Create empty instance of helper struct
            var emptySet = ImmutableHashSet<CategoryKnowledge>.Empty;
            var emptyHelper = new MergeHelper(emptySet, emptySet, emptySet);

            // Loop through pairs and process them
            var result = pairs.Aggregate(emptyHelper, ProcessPairs);

            // Create new categories
            var categories = result.Merged.Union(result.Singletons);

            // Generate new category set
            return categories.Aggregate(
                CategorySet.CreateEmpty(),
                (acc, next) => acc.AddCategory(CategoryLabel.Create(NodeType.Terminal), next));
        }

    }
}
