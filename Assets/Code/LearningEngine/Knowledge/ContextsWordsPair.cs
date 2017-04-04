using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    class ContextsWordsPair
    {
        // Allowed contexts: words on the left and on the right
        private readonly ImmutableHashSet<string> _leftContext;
        public ImmutableHashSet<string> LeftContext { get { return _leftContext; } }

        private readonly ImmutableHashSet<string> _rightContext;
        public ImmutableHashSet<string> RightContext { get { return _rightContext; } }

        // Words allowed in these contexts
        private readonly ImmutableHashSet<string> _words;
        public ImmutableHashSet<string> Words { get { return _words; } }

        private ContextsWordsPair(ImmutableHashSet<string> leftContext,
            ImmutableHashSet<string> rightContext, ImmutableHashSet<string> words)
        {
            _leftContext = leftContext;
            _rightContext = rightContext;
            _words = words;
        }

        public static ContextsWordsPair Empty()
        {
            var emptySet = ImmutableHashSet<string>.Empty;
            return new ContextsWordsPair(emptySet, emptySet, emptySet);
        }

        public ContextsWordsPair AddWord(string left, string right, string word)
        {
            return new ContextsWordsPair(
                _leftContext.Add(left),
                _rightContext.Add(right), _words.Add(word));
        }

        public ContextsWordsPair Merge(ContextsWordsPair other)
        {
            // Function for adding a word to a set of strings 
            Func<ImmutableHashSet<string>, string, ImmutableHashSet<string>> addWord = 
                (acc, next) => acc.Add(next);

            // Add words of other to the current instance
            var leftContext = _leftContext.Aggregate(other.LeftContext, addWord);
            var rightContext = _rightContext.Aggregate(other.RightContext, addWord);
            var words = _words.Aggregate(other.Words, addWord);

            return new ContextsWordsPair(leftContext, rightContext, words);
        }
    }
}
