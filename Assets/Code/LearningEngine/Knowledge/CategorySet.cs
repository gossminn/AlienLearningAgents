using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Data type for representing a set of syntactic categories
    internal class CategorySet
    {
        // Labels and contexts for terminals
        private readonly ImmutableDictionary<CategoryLabel, WordDistributionSet> _terminalCategories;


        // Labels and contexts for non-terminals

        // Root category
        private readonly CategoryLabel _root;

        // Constructor is private, public interface through factory methods
        private CategorySet(CategoryLabel root, ImmutableDictionary<CategoryLabel, WordDistributionSet> terminalCategories)
        {
            _root = root;
            _terminalCategories = terminalCategories;
        }

        public CategoryLabel Root
        {
            get { return _root; }
        }

        public ImmutableDictionary<CategoryLabel, WordDistributionSet> TerminalCategories
        {
            get { return _terminalCategories; }
        }

        // Number of terminalCategories
        public int Count
        {
            get { return _terminalCategories.Count; }
        }

        // Factory method: create empty CategorySet
        public static CategorySet CreateEmpty()
        {
            return new CategorySet(CategoryLabel.EmptyCat,
                ImmutableDictionary<CategoryLabel, WordDistributionSet>.Empty);
        }

        // Add or replace the root category of an existing CategorySet
        public CategorySet SetRootCat(CategoryLabel rootCat)
        {
            return new CategorySet(rootCat, _terminalCategories);
        }

        // Add a category to an existing CategorySet (with only a label)
        public CategorySet AddCategory(CategoryLabel category)
        {
            return new CategorySet(_root, _terminalCategories.Add(category, WordDistributionSet.Empty()));
        }

        // Add a category to an existing CategorySet (with pre-made CategoryKnowlegde)
        public CategorySet AddCategory(CategoryLabel category, WordDistributionSet knowledge)
        {
            return new CategorySet(_root, _terminalCategories.Add(category, knowledge));
        }


        // Remove a non-root category from an existing CategorySet
        public CategorySet RemoveCategory(CategoryLabel category)
        {
            return new CategorySet(_root, _terminalCategories.Remove(category));
        }

        // Add a left context to a category
        public CategorySet UpdateLeftContext(CategoryLabel category, string left)
        {
            // Add the context
            var catKnowledge0 = _terminalCategories[category];
            var catKnowledge1 = catKnowledge0.AddLeftContext(left);

            // Update the dictionary entry
            var categories1 = _terminalCategories.SetItem(category, catKnowledge1);

            return new CategorySet(_root, categories1);
        }

        // Add a right context to a category
        public CategorySet UpdateRightContext(CategoryLabel category, string right)
        {
            // Add the context
            var catKnowledge0 = _terminalCategories[category];
            var catKnowledge1 = catKnowledge0.AddRightContext(right);

            // Update the dictionary entry
            var categories1 = _terminalCategories.SetItem(category, catKnowledge1);

            return new CategorySet(_root, categories1);
        }

        // Add a word to a category
        public CategorySet UpdateWord(CategoryLabel category, string word)
        {
            // Add word to WordDistributionSet corresponding to given CategoryLabel
            var catKnowledge0 = _terminalCategories[category];
            var catKnowledge1 = catKnowledge0.AddWord(word);

            // Update entry with new version of WordDistributionSet
            var categories1 = _terminalCategories.SetItem(category, catKnowledge1);

            return new CategorySet(_root, categories1);
        }

        // Find terminalCategories containing a given word
        public IEnumerable<CategoryLabel> FindWord(string word)
        {
            // Helper function: does a category contain the word?
            Predicate<WordDistributionSet> hasWord = category => category.Words.Contains(word);

            // For each entry, if it has the word, yield its label
            return _terminalCategories
                .Where(entry => hasWord(entry.Value))
                .Select(pair => pair.Key);
        }

        // Find terminalCategories containing a given left context
        public IEnumerable<CategoryLabel>
            FindLeftContext(string left)
        {
            // Helper function: does a category contain the context?
            Predicate<WordDistributionSet> hasLeftContext =
                category => category.LeftContext.Contains(left);

            // For each entry, if it has the desired context, yield its label
            return _terminalCategories
                .Where(entry => hasLeftContext(entry.Value))
                .Select(pair => pair.Key);
        }

        // Find terminalCategories containing a given right context
        public IEnumerable<CategoryLabel>
            FindRightContext(string right)
        {
            // Helper function: does a category contain the context? 
            Predicate<WordDistributionSet> hasRightContext =
                category => category.RightContext.Contains(right);

            // For each entry, if it has the desired context, yield its label
            return _terminalCategories
                .Where(entry => hasRightContext(entry.Value))
                .Select(pair => pair.Key);
        }

        // Get XML representation of the ruleset
        public string GetXmlString()
        {
            var rootEntry = "<cat type=root>" + _root + "</cat>";
            var otherEntries = string.Join("", _terminalCategories
                .Select(x => "<cat type=nonroot>" + x.Value.GetXmlString() + "</cat>")
                .ToArray());
            return "<syntaxCats>" + rootEntry + otherEntries + "</syntaxCats>";
        }
    }
}