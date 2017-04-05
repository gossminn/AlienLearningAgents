using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Class for storing knowledge (words plus contexts) about syntactic categories
    class CategoryKnowledge
    {
        // Allowed contexts: words on the left and on the right
        private readonly ImmutableHashSet<string> _leftContext;
        public ImmutableHashSet<string> LeftContext { get { return _leftContext; } }

        private readonly ImmutableHashSet<string> _rightContext;
        public ImmutableHashSet<string> RightContext { get { return _rightContext; } }

        // Words allowed in these contexts
        private readonly ImmutableHashSet<string> _words;
        public ImmutableHashSet<string> Words { get { return _words; } }

        private CategoryKnowledge(ImmutableHashSet<string> leftContext,
            ImmutableHashSet<string> rightContext, ImmutableHashSet<string> words)
        {
            _leftContext = leftContext;
            _rightContext = rightContext;
            _words = words;
        }

        public static CategoryKnowledge Empty()
        {
            var emptySet = ImmutableHashSet<string>.Empty;
            return new CategoryKnowledge(emptySet, emptySet, emptySet);
        }

        // Add a word to the category
        public CategoryKnowledge AddWord(string word)
        {
            return new CategoryKnowledge(_leftContext,_rightContext, _words.Add(word));
        }

        // Add a left context to the category
        public CategoryKnowledge AddLeftContext(string left)
        {
            return new CategoryKnowledge(_leftContext.Add(left), _rightContext, _words);
        }

        // Add a right context to the category
        public CategoryKnowledge AddRightContext(string right)
        {
            return new CategoryKnowledge(_leftContext, _rightContext.Add(right), _words);
        }

        // Merge two categories together
        public CategoryKnowledge Merge(CategoryKnowledge other)
        {
            // Function for adding a word to a set of strings 
            Func<ImmutableHashSet<string>, string, ImmutableHashSet<string>> addWord = 
                (acc, next) => acc.Add(next);

            // Add words of other to the current instance
            var leftContext = _leftContext.Aggregate(other.LeftContext, addWord);
            var rightContext = _rightContext.Aggregate(other.RightContext, addWord);
            var words = _words.Aggregate(other.Words, addWord);

            return new CategoryKnowledge(leftContext, rightContext, words);
        }

        // Get XML representation
        public string GetXMLString()
        {
            var leftString = String.Join(",", LeftContext.ToArray());
            var rightString = String.Join(",", RightContext.ToArray());
            var wordString = String.Join(",", Words.ToArray());

            return "<category>" + 
                "<leftContext>" + leftString + "</leftContext>" + 
                "<rightContext>" + rightString + "</rightContext>" +
                "<words>" + wordString + "</words>" + 
                "</category>";
        }
    }
}
