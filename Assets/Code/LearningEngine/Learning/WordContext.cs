using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Helper class for storing a single word and its context
    class WordContext
    {
        // The word itself
        private readonly string _word;
        public string Word { get { return _word; } }

        // Word that occured to the left of the word
        private readonly string _left;
        public string Left { get { return _left; } }

        // Word that occured to the right of the word
        private readonly string _right;
        public string Right { get { return _right; } }

        // Private constructor
        private WordContext(string word, string left, string right)
        {
            _word = word;
            _left = left;
            _right = right;
        }

        // Factory method: create contexts for each word in a sentence
        public static ImmutableArray<WordContext> GetContexts(string sentence)
        {
            // Split sentence into array of words
            var words = sentence.Split();

            // Functions for getting word to the left or to the right, respectively
            Func<int, string> leftOf =
                index => index == 0 ? "#" : words[index - 1];
            Func<int, string> rightOf =
                index => index == words.Length - 1 ? "#" : words[index + 1];

            // For every word, add context of that word to array
            return Enumerable.Range(0, words.Length)
                .Aggregate(
                    ImmutableArray<WordContext>.Empty,
                    (acc, next) => acc.Add(
                        new WordContext(words[next], leftOf(next), rightOf(next))
                ));
        }
    }
}
