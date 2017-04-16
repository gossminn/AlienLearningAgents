using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Data type for representing a set of terminal syntactic categories
    internal class TerminalCategorySet
    {
        // Map of category labels to their corresponding contexts
        private readonly ImmutableDictionary<CategoryLabel, WordDistributionSet> _categories;

        // Constructor is private, public interface through factory methods
        private TerminalCategorySet(ImmutableDictionary<CategoryLabel, WordDistributionSet> categories)
        {
            _categories = categories;
        }

        public ImmutableDictionary<CategoryLabel, WordDistributionSet> Categories
        {
            get { return _categories; }
        }

        // Number of categories
        public int Count
        {
            get { return _categories.Count; }
        }

        // Factory method: create empty TerminalCategorySet
        public static TerminalCategorySet CreateEmpty()
        {
            return new TerminalCategorySet(ImmutableDictionary<CategoryLabel, WordDistributionSet>.Empty);
        }

        // Add a category to an existing TerminalCategorySet (with only a label)
        public TerminalCategorySet AddCategory(CategoryLabel category)
        {
            return new TerminalCategorySet(_categories.Add(category, WordDistributionSet.CreateEmpty()));
        }

        // Add a category to an existing TerminalCategorySet (with pre-made CategoryKnowlegde)
        public TerminalCategorySet AddCategory(CategoryLabel category, WordDistributionSet knowledge)
        {
            return new TerminalCategorySet(_categories.Add(category, knowledge));
        }

        // Add a left context to a category
        public TerminalCategorySet UpdateLeftContext(CategoryLabel category, string left)
        {
            // Add the context
            var catKnowledge0 = _categories[category];
            var catKnowledge1 = catKnowledge0.AddLeftContext(left);

            // Update the dictionary entry
            var categories1 = _categories.SetItem(category, catKnowledge1);

            return new TerminalCategorySet(categories1);
        }

        // Add a right context to a category
        public TerminalCategorySet UpdateRightContext(CategoryLabel category, string right)
        {
            // Add the context
            var catKnowledge0 = _categories[category];
            var catKnowledge1 = catKnowledge0.AddRightContext(right);

            // Update the dictionary entry
            var categories1 = _categories.SetItem(category, catKnowledge1);

            return new TerminalCategorySet(categories1);
        }

        // Add a word to a category
        public TerminalCategorySet UpdateWord(CategoryLabel category, string word)
        {
            // Add word to WordDistributionSet corresponding to given CategoryLabel
            var catKnowledge0 = _categories[category];
            var catKnowledge1 = catKnowledge0.AddWord(word);

            // Update entry with new version of WordDistributionSet
            var categories1 = _categories.SetItem(category, catKnowledge1);

            return new TerminalCategorySet(categories1);
        }

        // Find categories containing a given word
        public IEnumerable<CategoryLabel> FindWord(string word)
        {
            // Helper function: does a category contain the word?
            Predicate<WordDistributionSet> hasWord = category => category.Words.Contains(word);

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
            Predicate<WordDistributionSet> hasLeftContext =
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
            Predicate<WordDistributionSet> hasRightContext =
                category => category.RightContext.Contains(right);

            // For each entry, if it has the desired context, yield its label
            return _categories
                .Where(entry => hasRightContext(entry.Value))
                .Select(pair => pair.Key);
        }

        // Get XML representation of the ruleset
        public string GetXmlString()
        {
            var entries = string.Join("", _categories
                .Select(x => "<cat type=nonroot>" + x.Value.GetXmlString() + "</cat>")
                .ToArray());
            return "<syntaxCats>" + entries + "</syntaxCats>";
        }
    }
}