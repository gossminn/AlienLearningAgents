using System;
using System.Collections.Immutable;
using System.Linq;

namespace Code.LearningEngine.Knowledge.Categories
{
    // Class for storing knowledge (words plus contexts) about syntactic (terminal) terminalCategories
    internal class WordDistributionSet
    {
        // Allowed contexts: words on the left and on the right
        private readonly ImmutableHashSet<string> _leftContext;

        private readonly ImmutableHashSet<string> _rightContext;

        // Words allowed in these contexts
        private readonly ImmutableHashSet<string> _words;

        private WordDistributionSet(ImmutableHashSet<string> leftContext,
            ImmutableHashSet<string> rightContext, ImmutableHashSet<string> words)
        {
            _leftContext = leftContext;
            _rightContext = rightContext;
            _words = words;
        }

        public ImmutableHashSet<string> LeftContext
        {
            get { return _leftContext; }
        }

        public ImmutableHashSet<string> RightContext
        {
            get { return _rightContext; }
        }

        public ImmutableHashSet<string> Words
        {
            get { return _words; }
        }

        // Factory method: create empty set
        public static WordDistributionSet CreateEmpty()
        {
            var emptySet = ImmutableHashSet<string>.Empty;
            return new WordDistributionSet(emptySet, emptySet, emptySet);
        }

        // Add a word to the category
        public WordDistributionSet AddWord(string word)
        {
            return new WordDistributionSet(_leftContext, _rightContext, _words.Add(word));
        }

        // Add a left context to the category
        public WordDistributionSet AddLeftContext(string left)
        {
            return new WordDistributionSet(_leftContext.Add(left), _rightContext, _words);
        }

        // Add a right context to the category
        public WordDistributionSet AddRightContext(string right)
        {
            return new WordDistributionSet(_leftContext, _rightContext.Add(right), _words);
        }

        // Merge two terminalCategories together
        public WordDistributionSet Merge(WordDistributionSet other)
        {
            // Function for adding a word to a set of strings 
            Func<ImmutableHashSet<string>, string, ImmutableHashSet<string>> addWord =
                (acc, next) => acc.Add(next);

            // Add words of other to the current instance
            var leftContext = _leftContext.Aggregate(other.LeftContext, addWord);
            var rightContext = _rightContext.Aggregate(other.RightContext, addWord);
            var words = _words.Aggregate(other.Words, addWord);

            return new WordDistributionSet(leftContext, rightContext, words);
        }

        // Get XML representation
        public string GetXmlString()
        {
            var leftString = string.Join(",", LeftContext.ToArray());
            var rightString = string.Join(",", RightContext.ToArray());
            var wordString = string.Join(",", Words.ToArray());

            return "<category>" +
                   "<leftContext>" + leftString + "</leftContext>" +
                   "<rightContext>" + rightString + "</rightContext>" +
                   "<words>" + wordString + "</words>" +
                   "</category>";
        }
    }
}