using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Stores the possible meanings for a given word
    internal class WordMeaningHypotheses
    {
        // Random number generator
        private static readonly Random _randomNumbers = new Random();

        // Set of meanings
        private readonly ImmutableHashSet<Func<LogicalModel, ISemanticValue>> _meanings;

        private WordMeaningHypotheses(ImmutableHashSet<Func<LogicalModel, ISemanticValue>> meanings)
        {
            _meanings = meanings;
        }

        // Generate initial hypotheses
        public static WordMeaningHypotheses Initialize(IEnumerable<Func<LogicalModel, ISemanticValue>> meanings)
        {
            // Store meanings in set
            var hypothesisSet = meanings.Aggregate(
                ImmutableHashSet<Func<LogicalModel, ISemanticValue>>.Empty,
                (acc, next) => acc.Add(next));
            return new WordMeaningHypotheses(hypothesisSet);
        }

        // Meaning set is still relevant iff non-empty
        public bool IsRelevant()
        {
            return !_meanings.IsEmpty;
        }

        // Guess: return one of the hypotheses at random (may throw error if no meanings left)
        public Func<LogicalModel, ISemanticValue> Guess()
        {
            var randomNum = _randomNumbers.Next(_meanings.Count);
            return _meanings.Skip(randomNum).First();
        }

        // Evaluate and eliminate hypotheses based on context
        public WordMeaningHypotheses Evaluate(LogicalModel model)
        {
            // Remove irrelevant hypotheses
            var hypotheses = _meanings.Where(m => m(model).AppliesToModel(model));

            // Make new set with new hypotheses
            var hypothesisSet = hypotheses.Aggregate(
                ImmutableHashSet<Func<LogicalModel, ISemanticValue>>.Empty,
                (acc, next) => acc.Add(next));
            return new WordMeaningHypotheses(hypothesisSet);
        }

        // Write as XML string
        public string ToXmlString()
        {
            return string.Join(",", _meanings.Select(x => x.GetHashCode().ToString()).ToArray());
        }
    }
}