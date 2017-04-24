using System;
using System.Collections.Immutable;

namespace LearningEngine
{
    // Class for storing hypotheses about the possible meanings of a word
    internal class WordHypothesis<T> : IWordHypothesis where T : ISemanticValue
    {
        private readonly string _word;
        private readonly ImmutableHashSet<Func<LogicalModel, T>> _meanings;

        private WordHypothesis(string word, ImmutableHashSet<Func<LogicalModel, T>> meanings)
        {
            _word = word;
            _meanings = meanings;
        }

        public string Word
        {
            get { return _word; }
        }

        public ImmutableHashSet<Func<LogicalModel, T>> Meanings
        {
            get { return _meanings; }
        }

        public static WordHypothesis<T> CreateEmpty(string word)
        {
            return new WordHypothesis<T>(word, ImmutableHashSet<Func<LogicalModel, T>>.Empty);
        }

        public WordHypothesis<T> AddMeaning(Func<LogicalModel, T> meaning)
        {
            return new WordHypothesis<T>(_word, _meanings.Add(meaning));
        }

        public WordHypothesis<T> RemoveMeaning(Func<LogicalModel, T> meaning)
        {
            return new WordHypothesis<T>(_word, _meanings.Remove(meaning));
        }
    }
}