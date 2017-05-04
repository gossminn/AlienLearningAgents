using System.Collections.Immutable;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    internal class CategoryHypothesisSet
    {
        // For each category and for each semantic type, store a CategoryHypothesis
        private readonly ImmutableHashSet<ImmutableHashSet<WordHypothesisSet>> _hypotheses;

        private CategoryHypothesisSet(ImmutableHashSet<ImmutableHashSet<WordHypothesisSet>> hypotheses)
        {
            _hypotheses = hypotheses;
        }


    }
}