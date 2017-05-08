using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Agents;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Stores the possible meanings for a given word
    internal class WordMeaningHypotheses
    {
        // Random number generator
        private static readonly Random _random = new Random();

        // Set of meanings
        private readonly ImmutableHashSet<MeaningCandidate> _meanings;

        private WordMeaningHypotheses(ImmutableHashSet<MeaningCandidate> meanings)
        {
            _meanings = meanings;
        }

        // Generate initial hypotheses
        public static WordMeaningHypotheses Initialize(IEnumerable<Func<LogicalModel, ISemanticValue>> meanings)
        {
            // Store meanings in set
            var hypothesisSet = meanings.Aggregate(
                ImmutableHashSet<MeaningCandidate>.Empty,
                (acc, next) => acc.Add(MeaningCandidate.Initialize(next)));
            return new WordMeaningHypotheses(hypothesisSet);
        }

        // Meaning set is still relevant iff non-empty
        public bool IsRelevant()
        {
            return !_meanings.IsEmpty;
        }

        // A word's meaning is fixed if there is only one hypothesis left
        public bool IsFixed()
        {
            return _meanings.Count == 1;
        }

        // Get the single remaining hypothesis from the set (assumes set is fixed)
        public MeaningCandidate GetSingleMeaning()
        {
            return _meanings.Single();
        }


        // Guess: return one of the hypotheses at random (may throw error if no meanings left)
        public WordMeaningHypotheses Guess()
        {
            // Already fixed? Just return current instance
            if (IsFixed())
            {
                return this;
            }

            // Else: sort & group by score, pick the best group
            var bestGroup = _meanings
                .GroupBy(m => m.Score)
                .OrderBy(g => g.Key)
                .Last();

            // Randomly select one score from that
            var randomNum = _random.Next(bestGroup.Count());
            var guess = bestGroup.Skip(randomNum).First();

            // Make set with only guess
            var hypothesisSet = ImmutableHashSet<MeaningCandidate>
                .Empty
                .Add(guess);
            return new WordMeaningHypotheses(hypothesisSet);
        }

        // Evaluate and eliminate hypotheses based on context
        public WordMeaningHypotheses Evaluate(LogicalModel model)
        {
            // Remove irrelevant hypotheses
            var hypotheses = _meanings.Where(m => m.Meaning(model).AppliesToModel(model));

            // Make new set with new hypotheses
            var hypothesisSet = hypotheses.Aggregate(
                ImmutableHashSet<MeaningCandidate>.Empty,
                (acc, next) => acc.Add(next));
            return new WordMeaningHypotheses(hypothesisSet);
        }

        // Process feedback
        public WordMeaningHypotheses ProcessFeedback(Feedback feedback, MeaningCandidate guess)
        {
            // Guess does not apply to this hypothesis: don't change anything
            if (!_meanings.Contains(guess))
            {
                return this;
            }

            // Else, process feedback:
            var candidate = feedback == Feedback.Angry
                // If feedback is negative, decrease score for that meaning candidate
                ? guess.DecreaseScore()
                : feedback == Feedback.Happy
                    // Else if it's positive, increase the score
                    ? guess.IncreaseScore()
                    // Otherwise, leave as-is
                    : guess;

            // Replace guess by new version
            return new WordMeaningHypotheses(_meanings.Remove(guess).Add(candidate));
        }

        // Write as XML string
        public string ToXmlString()
        {
            return string.Join("", _meanings.Select(x => x.ToXmlString()).ToArray());
        }
    }
}