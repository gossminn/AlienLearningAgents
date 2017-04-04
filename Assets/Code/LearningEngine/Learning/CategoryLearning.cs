using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{

    // Static class with extension methods for CategorySet
    static class CategorySetLearning
    {
        // Get the words that are to the left of each occurance of the word in a sentence
        static ImmutableHashSet<string> GetLeftContext(this string sentence, string word)
        {
            // Make array of words
            var words = sentence.Split().ToImmutableArray();

            // Function for getting word that is to the left of a given index ("#" = word boundary)
            Func<int, string> leftOf = index => index == 0 ? "#" : words[index - 1];
            
            // Loop over indexes of words in sentence
            return Enumerable.Range(0, words.Length).Aggregate(
                // Start with empty list
                ImmutableHashSet<string>.Empty,

                // If target is at next index, add word to the left of that
                (acc, next) => words[next] == word ? acc.Add(leftOf(next)) : acc
            );
        }

        // Get the words that are to the right of each occurance of the word in a sentence
        static ImmutableHashSet<string> GetRightContext(this string sentence, string word)
        {
            // Make array of words
            var words = sentence.Split().ToImmutableArray();

            // Function for getting word that is to the right of a given index ("#" = word boundary)
            Func<int, string> rightOf = index => index == words.Length - 1 ? "#" : words[index + 1];

            // Loop over indexes of words in sentence
            return Enumerable.Range(0, words.Length).Aggregate(
                // Start with empty list
                ImmutableHashSet<string>.Empty,

                // If target is at next index, add word to the right of that
                (acc, next) => words[next] == word ? acc.Add(rightOf(next)) : acc
            );
        }

        public static CategorySet UpdateCategories(this CategorySet current, SentenceMemory memory)
        {
            // Check for match between to sets of context words
            Func<ImmutableHashSet<string>, ImmutableHashSet<string>, bool> match = 
                (context0, context1) => !context0.Intersect(context1).IsEmpty;

            // Get contexts for everything in memory
            var contexts = memory.Sentences.SelectMany(
                sentence => sentence.Split().Select(
                    y => new {
                        Word = y,
                        Left = sentence.GetLeftContext(y),
                        Right = sentence.GetRightContext(y)
                    }
                ));

            // Update left contexts
            var categories0 = current.Categories
                .Select(catEntry => 
                    contexts.Where(context => 
                        match(catEntry.Value.LeftContext, context.Left))
                    .Aggregate(
                        catEntry.Value.LeftContext, 
                        (acc, next) => acc.AddRange(next.Left.AsEnumerable())
                        )
                );  

        }
    }
}
