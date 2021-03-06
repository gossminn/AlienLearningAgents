using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Agents;
using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Stores hypotheses about word meanings for a specific syntactic category
    internal class SyntaxCategoryHypotheses
    {
        // Store word meaning hypotheses per semantic type
        private readonly ImmutableHashSet<SemanticClassHypotheses> _hypotheses;

        private SyntaxCategoryHypotheses(ImmutableHashSet<SemanticClassHypotheses> hypotheses)
        {
            _hypotheses = hypotheses;
        }

        // Make initial hypotheses
        public static SyntaxCategoryHypotheses Initialize(ImmutableHashSet<string> words)
        {
            // Initialize hypothesis space for each semantic class
            var hypotheses = AtomicMeanings.GetFunctions().Select(
                m => SemanticClassHypotheses.Initialize(words, m));

            // Store in HashSet
            return MakeHypothesisSet(hypotheses);
        }

        private static SyntaxCategoryHypotheses MakeHypothesisSet(IEnumerable<SemanticClassHypotheses> hypotheses)
        {
            var hypothesisSet = hypotheses.Aggregate(
                ImmutableHashSet<SemanticClassHypotheses>.Empty,
                (acc, next) => acc.Add(next));
            return new SyntaxCategoryHypotheses(hypothesisSet);
        }

        // Set is still relevant if at least one hypothesis is still relevant
        public bool IsRelevant()
        {
            return _hypotheses.Any(h => h.IsRelevant());
        }

        // Set is fixed when only one semantic class hypothesis is left
        public bool IsFixed()
        {
            return _hypotheses.Count == 1;
        }

        // Does category contain a given word?
        public bool HasMeaningFor(string word)
        {
            return _hypotheses.Any(h => h.HasMeaningFor(word));
        }

        // Find unique meaning for a word
        public MeaningCandidate FindMeaningFor(string word)
        {
            return _hypotheses
                .Single(h => h.HasMeaningFor(word))
                .FindMeaningFor(word);
        }

        // Guess: randomly choose a hypothesis about a semantic class
        public SyntaxCategoryHypotheses Guess()
        {
            var hypotheses = _hypotheses.Single().Guess();
            var hypothesisSet = ImmutableHashSet<SemanticClassHypotheses>
                .Empty
                .Add(hypotheses);
            return new SyntaxCategoryHypotheses(hypothesisSet);
        }

        // Evaluate based on context: eliminate irrelevant hypotheses
        public SyntaxCategoryHypotheses Evaluate(LogicalModel model, ImmutableArray<string> words)
        {
            // Evaluate all hypotheses and remove irrelevant ones
            var hypotheses = _hypotheses
                .Select(h => h.Evaluate(model, words))
                .Where(h => h.IsRelevant());

            // Store new hypothesis set
            return MakeHypothesisSet(hypotheses);
        }

        public SyntaxCategoryHypotheses ProcessFeedback(Feedback feedback, ImmutableArray<string> words,
            MeaningHypothesisSet guess)
        {
            var hypotheses = _hypotheses.Select(h => h.ProcessFeedback(feedback, words, guess));
            return MakeHypothesisSet(hypotheses);
        }

        public string ToXmlString()
        {
            var entries = _hypotheses.Select(x => x.ToXmlString());
            return "<category>" + string.Join("", entries.ToArray()) + "</category>";
        }
    }
}