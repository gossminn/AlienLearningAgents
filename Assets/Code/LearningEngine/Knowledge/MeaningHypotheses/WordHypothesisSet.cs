using System.Collections.Immutable;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    internal class CategoryHypothesisSpace
    {
        private ImmutableHashSet<WordHypothesisSet> _hypotheses;
    }

    internal class WordHypothesisSet
    {
        // For each word in a category, store the corresponding hypothesis space
        private readonly ImmutableDictionary<string, WordHypothesisSpace> _wordHypotheses;
    }
}