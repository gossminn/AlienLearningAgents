using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal class MeaningHypothesisSet
    {
        private readonly ImmutableDictionary<CategoryMeaningHypotheses,
                ImmutableHashSet<WordMeaningHypotheses>> _meanings;

        private MeaningHypothesisSet(ImmutableDictionary<CategoryMeaningHypotheses,
            ImmutableHashSet<WordMeaningHypotheses>> meanings)
        {
            _meanings = meanings;
        }

        public IEnumerable<CategoryMeaningHypotheses> Hypotheses
        {
            get { return _meanings.Keys; }
        }

        public static MeaningHypothesisSet MakeInitialHypotheses(
            IEnumerable<ImmutableHashSet<string>> words)
        {
            // Define pairs
            var pairs = CategoryMeaningHypotheses.MakeInitialHypotheses(words)
                .SelectMany(h => h.Meanings.Select(
                    m => new {C=h, W=WordMeaningHypotheses.MakeInitialHypotheses(h.Words, m)}));

            // New dictionary entry for each pair
            return new MeaningHypothesisSet(pairs.Aggregate(
                ImmutableDictionary<CategoryMeaningHypotheses,
                    ImmutableHashSet<WordMeaningHypotheses>>.Empty,
                (acc, next) => acc.Add(next.C, next.W)));
        }

    }
}