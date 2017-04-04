using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Data type for representing a set of syntactic categories
    class CategorySet
    {
        // Separately store the root category (e.g. 'S') and the other categories
        private readonly SyntaxCat _root;
        public SyntaxCat Root { get { return _root; } }

        // Dictionary of context-word pairs
        private readonly ImmutableDictionary<SyntaxCat, ContextsWordsPair> _categories;
        public ImmutableDictionary<SyntaxCat, ContextsWordsPair> Categories { get { return _categories; } }

        // Constructor is private, public interface through factory methods
        private CategorySet(SyntaxCat root, ImmutableDictionary<SyntaxCat, ContextsWordsPair> categories)
        {
            _root = root;
            _categories = categories;
        }

        // Factory method: create empty CategorySet
        public static CategorySet CreateEmpty()
        {
            return new CategorySet(SyntaxCat.EmptyCat, 
                ImmutableDictionary<SyntaxCat, ContextsWordsPair>.Empty);
        }

        // Factory method: add or replace the root category of an existing CategorySet
        public CategorySet SetRootCat(SyntaxCat rootCat)
        {
            return new CategorySet(rootCat, _categories);
        }

        // Factory method: add a non-root category to an existing CategorySet
        public CategorySet AddCategory(SyntaxCat category)
        {
            return new CategorySet(_root, _categories.Add(category, ContextsWordsPair.Empty()));
        }

        // Factory method: remove a non-root category from an existing CategorySet
        public CategorySet RemoveCategory(SyntaxCat category)
        {
            return new CategorySet(_root, _categories.Remove(category));
        }

        // Get XML representation of the ruleset
        public string GetXMLString()
        {
            var rootEntry = "<cat type=root>" + _root.ToString() + "</cat>";
            var otherEntries = String.Join("", _categories
                .Select(x => "<cat type=nonroot>" + x.ToString() + "</cat>").ToArray());
            return "<syntaxCats>" + rootEntry + otherEntries + "</syntaxCats>";
        }
    }
}
