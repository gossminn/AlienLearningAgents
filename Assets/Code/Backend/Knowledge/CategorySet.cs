using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Data type for representing a set of syntactic categories
    class CategorySet
    {
        // Separately store the root category (e.g. 'S') and the other categories
        private readonly SyntaxCat _rootCategory;
        private readonly ImmutableHashSet<SyntaxCat> _otherCategories;
        public SyntaxCat Root { get { return _rootCategory; } }

        // Constructor is private, public interface through factory methods
        private CategorySet(SyntaxCat rootCat, ImmutableHashSet<SyntaxCat> otherCats)
        {
            _rootCategory = rootCat;
            _otherCategories = otherCats;
        }

        // Factory method: create empty CategorySet
        public static CategorySet CreateEmpty()
        {
            return new CategorySet(SyntaxCat.EmptyCat, ImmutableHashSet<SyntaxCat>.Empty);
        }

        // Factory method: add or replace the root category of an existing CategorySet
        public CategorySet SetRootCat(SyntaxCat rootCat)
        {
            return new CategorySet(rootCat, _otherCategories);
        }

        // Factory method: add a non-root category to an existing CategorySet
        public CategorySet AddCategory(SyntaxCat category)
        {
            return new CategorySet(_rootCategory, _otherCategories.Add(category));
        }

        // Factory method: remove a non-root category from an existing CategorySet
        public CategorySet RemoveCategory(SyntaxCat category)
        {
            return new CategorySet(_rootCategory, _otherCategories.Remove(category));
        }

        // Get XML representation of the ruleset
        public string GetXMLString()
        {
            var rootEntry = "<cat type=root>" + _rootCategory.ToString() + "</cat>";
            var otherEntries = String.Join("", _otherCategories
                .Select(x => "<cat type=nonroot>" + x.ToString() + "</cat").ToArray());
            return "<syntaxCats>" + rootEntry + otherEntries + "</syntaxCats>";
        }
    }
}
