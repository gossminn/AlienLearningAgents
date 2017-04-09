using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Helper class for storing a single constituent and its context
    internal class ConstituentContext
    {
        // Category of constituent itself
        private readonly CategoryLabel _constituent;

        // Category of constituent to the left
        private readonly CategoryLabel _left;

        // Category of constituent to the right
        private readonly CategoryLabel _right;

        // Private constructor
        private ConstituentContext(CategoryLabel left, CategoryLabel right,
            CategoryLabel constituent)
        {
            _left = left;
            _right = right;
            _constituent = constituent;
        }


        public CategoryLabel Left
        {
            get { return _left; }
        }

        public CategoryLabel Right
        {
            get { return _right; }
        }

        public CategoryLabel Constituent
        {
            get { return _constituent; }
        }

        // Factory method: make array of ConstituentContext from sequence of CategoryLabels
        public static ImmutableArray<ConstituentContext> GetContexts(
            ImmutableArray<CategoryLabel> constituents)
        {
            var empty = CategoryLabel.EmptyCat;

            // Functions for getting word to the left or to the right, respectively
            Func<int, CategoryLabel> leftOf =
                index => index == 0 ? empty : constituents[index - 1];
            Func<int, CategoryLabel> rightOf =
                index => index == constituents.Length - 1 ? empty : constituents[index + 1];

            return Enumerable.Range(0, constituents.Length)
                .Aggregate(
                    ImmutableArray<ConstituentContext>.Empty,
                    (acc, next) => acc.Add(
                        new ConstituentContext(leftOf(next), rightOf(next), constituents[next])));
        }
    }
}