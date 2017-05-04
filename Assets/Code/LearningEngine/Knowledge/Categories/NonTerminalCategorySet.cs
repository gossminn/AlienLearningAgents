using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Knowledge.Categories
{
    internal class NonTerminalCategorySet
    {
        private readonly ImmutableHashSet<CategoryLabel> _categories;

        private NonTerminalCategorySet(ImmutableHashSet<CategoryLabel> categories)
        {
            _categories = categories;
        }

        public IEnumerable<CategoryLabel> Categories
        { get { return _categories.AsEnumerable(); } }

        public static NonTerminalCategorySet CreateEmpty()
        {
            return new NonTerminalCategorySet(ImmutableHashSet<CategoryLabel>.Empty);
        }

        public NonTerminalCategorySet AddCategory(CategoryLabel category)
        {
            return new NonTerminalCategorySet(_categories.Add(category));
        }
    }
}