using System.Collections.Generic;

namespace LearningEngine
{
    internal class CategoryMonogram : ICategoryNGram
    {
        // Single element
        private static CategoryLabel _element;

        // Constructor
        private CategoryMonogram(CategoryLabel element)
        {
            _element = element;
        }

        public CategoryLabel Element
        {
            get { return _element; }
        }

        public IEnumerable<CategoryLabel> Sequence
        {
            get { return new[] {_element}; }
        }

        public static CategoryMonogram Create(CategoryLabel element)
        {
            return new CategoryMonogram(element);
        }
    }
}