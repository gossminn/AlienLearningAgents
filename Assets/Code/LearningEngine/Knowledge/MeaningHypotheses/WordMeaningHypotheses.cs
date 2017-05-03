using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal class WordMeaningHypotheses
    {
        private readonly string _word;
        private readonly ImmutableHashSet<Func<LogicalModel, ISemanticValue>> _meanings;

        private WordMeaningHypotheses(string word,
            ImmutableHashSet<Func<LogicalModel, ISemanticValue>> meanings)
        {
            _word = word;
            _meanings = meanings;
        }

        public string Word
        {
            get { return _word; }
        }

        public ImmutableHashSet<Func<LogicalModel, ISemanticValue>> Meanings
        {
            get { return _meanings; }
        }

        public static ImmutableHashSet<WordMeaningHypotheses> MakeInitialHypotheses(
            ImmutableHashSet<string> words, IEnumerable<Func<LogicalModel, ISemanticValue>> meanings)
        {
            // For each word, generate hypothesis with all possible meanings
            return words.Select(
                w => new WordMeaningHypotheses(w,meanings.ToImmutableHashSet())
            ).ToImmutableHashSet();
        }

        public WordMeaningHypotheses RemoveHypothesis(Func<LogicalModel, ISemanticValue> meaning)
        {
            return new WordMeaningHypotheses(_word, _meanings.Remove(meaning));
        }
    }
}