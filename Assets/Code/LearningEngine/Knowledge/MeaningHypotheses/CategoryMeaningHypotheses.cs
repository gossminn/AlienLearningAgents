using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal class CategoryMeaningHypotheses
    {
        private readonly ImmutableHashSet<WordHypothesis>

        private CategoryMeaningHypotheses(ImmutableHashSet<string> words,
            ImmutableHashSet<IEnumerable<Func<LogicalModel, ISemanticValue>>> meanings)
        {
            _words = words;
            _meanings = meanings;
        }

        public ImmutableHashSet<string> Words
        {
            get { return _words; }
        }

        public IEnumerable<IEnumerable<Func<LogicalModel, ISemanticValue>>> Meanings
        {
            get { return _meanings; }
        }

        public static IEnumerable<CategoryMeaningHypotheses> MakeInitialHypotheses(
            IEnumerable<ImmutableHashSet<string>> wordSets)
        {
            // Associate every word set with all possible meanings
            return wordSets.Select(
                w => new CategoryMeaningHypotheses(
                    w, AtomicMeanings.GetFunctions().ToImmutableHashSet()));
        }

        public CategoryMeaningHypotheses RemoveHypothesis(
            IEnumerable<Func<LogicalModel, ISemanticValue>> meaning)
        {
            return new CategoryMeaningHypotheses(_words, _meanings.Remove(meaning));
        }
    }
}