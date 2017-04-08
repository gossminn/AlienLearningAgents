using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Data type for representing a set of syntactic categories
    internal class CategorySet
    {
        // Dictionary of context-word pairs
        private readonly ImmutableDictionary<CategoryLabel, CategoryKnowledge> _categories;

        // Root category
        private readonly CategoryLabel _root;

        // Constructor is private, public interface through factory methods
        private CategorySet(CategoryLabel root, ImmutableDictionary<CategoryLabel, CategoryKnowledge> categories)
        {
            _root = root;
            _categories = categories;
        }

        public CategoryLabel Root
        {
            get { return _root; }
        }

        public ImmutableDictionary<CategoryLabel, CategoryKnowledge> Categories
        {
            get { return _categories; }
        }

        // Number of categories
        public int Count
        {
            get { return _categories.Count; }
        }

        // Factory method: create empty CategorySet
        public static CategorySet CreateEmpty()
        {
            return new CategorySet(CategoryLabel.EmptyCat,
                ImmutableDictionary<CategoryLabel, CategoryKnowledge>.Empty);
        }

        // Add or replace the root category of an existing CategorySet
        public CategorySet SetRootCat(CategoryLabel rootCat)
        {
            return new CategorySet(rootCat, _categories);
        }

        // Add a category to an existing CategorySet (with only a label)
        public CategorySet AddCategory(CategoryLabel category)
        {
            return new CategorySet(_root, _categories.Add(category, CategoryKnowledge.Empty()));
        }

        // Add a category to an existing CategorySet (with pre-made CategoryKnowlegde)
        public CategorySet AddCategory(CategoryLabel category, CategoryKnowledge knowledge)
        {
            return new CategorySet(_root, _categories.Add(category, knowledge));
        }


        // Remove a non-root category from an existing CategorySet
        public CategorySet RemoveCategory(CategoryLabel category)
        {
            return new CategorySet(_root, _categories.Remove(category));
        }

        // Add a left context to a category
        public CategorySet UpdateLeftContext(CategoryLabel category, string left)
        {
            // Add the context
            var catKnowledge0 = _categories[category];
            var catKnowledge1 = catKnowledge0.AddLeftContext(left);

            // Update the dictionary entry
            var categories1 = _categories.SetItem(category, catKnowledge1);

            return new CategorySet(_root, categories1);
        }

        // Add a right context to a category
        public CategorySet UpdateRightContext(CategoryLabel category, string right)
        {
            // Add the context
            var catKnowledge0 = _categories[category];
            var catKnowledge1 = catKnowledge0.AddRightContext(right);

            // Update the dictionary entry
            var categories1 = _categories.SetItem(category, catKnowledge1);

            return new CategorySet(_root, categories1);
        }

        // Add a word to a category
        public CategorySet UpdateWord(CategoryLabel category, string word)
        {
            // Add word to CategoryKnowledge corresponding to given CategoryLabel
            var catKnowledge0 = _categories[category];
            var catKnowledge1 = catKnowledge0.AddWord(word);

            // Update entry with new version of CategoryKnowledge
            var categories1 = _categories.SetItem(category, catKnowledge1);

            return new CategorySet(_root, categories1);
        }

        // Find categories containing a given word
        public IEnumerable<CategoryLabel> FindWord(string word)
        {
            // Helper function: does a category contain the word?
            Predicate<CategoryKnowledge> hasWord = category => category.Words.Contains(word);

            // For each entry, if it has the word, yield its label
            return _categories
                .Where(entry => hasWord(entry.Value))
                .Select(pair => pair.Key);
        }

        // Find categories containing a given left context
        public IEnumerable<CategoryLabel>
            FindLeftContext(string left)
        {
            // Helper function: does a category contain the context?
            Predicate<CategoryKnowledge> hasLeftContext =
                category => category.LeftContext.Contains(left);

            // For each entry, if it has the desired context, yield its label
            return _categories
                .Where(entry => hasLeftContext(entry.Value))
                .Select(pair => pair.Key);
        }

        // Find categories containing a given right context
        public IEnumerable<CategoryLabel>
            FindRightContext(string right)
        {
            // Helper function: does a category contain the context? 
            Predicate<CategoryKnowledge> hasRightContext =
                category => category.RightContext.Contains(right);

            // For each entry, if it has the desired context, yield its label
            return _categories
                .Where(entry => hasRightContext(entry.Value))
                .Select(pair => pair.Key);
        }

        // Get XML representation of the ruleset
        public string GetXmlString()
        {
            var rootEntry = "<cat type=root>" + _root + "</cat>";
            var otherEntries = string.Join("", _categories
                .Select(x => "<cat type=nonroot>" + x.Value.GetXmlString() + "</cat>")
                .ToArray());
            return "<syntaxCats>" + rootEntry + otherEntries + "</syntaxCats>";
        }
    }
}