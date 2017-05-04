using System.Collections.Generic;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Knowledge.KnowledgeHelpers
{
    // Data type for representing pairs of consecutively occuring syntactic categories
    internal class CategoryBigram : ICategoryNGram
    {
        // Fields: first and second members of the pair
        private readonly CategoryLabel _first;
        private readonly CategoryLabel _second;

        // Private constructor
        private CategoryBigram(CategoryLabel first, CategoryLabel second)
        {
            _first = first;
            _second = second;
        }

        // Getters
        public CategoryLabel First
        {
            get { return _first; }
        }

        public CategoryLabel Second
        {
            get { return _second; }
        }

        public IEnumerable<CategoryLabel> Sequence
        {
            get { return new[] {_first, _second}; }
        }

        // Factory method
        public static CategoryBigram Create(CategoryLabel first, CategoryLabel second)
        {
            return new CategoryBigram(first, second);
        }
    }

    // Represent sequences of CategoryLabels
}