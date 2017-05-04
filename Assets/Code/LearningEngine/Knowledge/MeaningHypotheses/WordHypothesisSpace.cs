using System;
using System.Collections.Immutable;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    internal class WordHypothesisSpace
    {
        private readonly ImmutableHashSet<Func<LogicalModel, ISemanticValue>> _meanings;

        private WordHypothesisSpace(ImmutableHashSet<Func<LogicalModel, ISemanticValue>> meanings)
        {
            _meanings = meanings;
        }

        public ImmutableHashSet<Func<LogicalModel, ISemanticValue>> Meanings
        {
            get { return _meanings; }
        }
    }
}