using System.Collections.Immutable;
using System.Linq;
using Random = System.Random;
using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Stores hypotheses about word meanings for a specific syntactic category
    internal class SyntaxCategoryHypotheses
    {
        // Store word meaning hypotheses per semantic type
        private readonly ImmutableHashSet<SemanticClassHypotheses> _hypotheses;

        // Random number generator
        private static readonly Random _randomNumbers = new Random();

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

        // Does category contain a given word?
        public bool HasMeaningFor(string word)
        {
            return _hypotheses.Any(h => h.HasMeaningFor(word));
        }

        // Guess: randomly choose a hypothesis about a semantic class
        public SemanticClassHypotheses Guess()
        {
            var randomNumber = _randomNumbers.Next(_hypotheses.Count);
            return _hypotheses.Skip(randomNumber).First();
        }

        // Evaluate based on context: eliminate irrelevant hypotheses
        public SyntaxCategoryHypotheses Evaluate(LogicalModel model, ImmutableArray<string> words)
        {
            // Evaluate all hypotheses and remove irrelevant ones
            var hypotheses = _hypotheses
                .Select(h => h.Evaluate(model, words))
                .Where(h => h.IsRelevant());

            // Store new hypothesis set
            var hypothesisSet = hypotheses.Aggregate(
                ImmutableHashSet<SemanticClassHypotheses>.Empty,
                (acc, next) => acc.Add(next));
            return new SyntaxCategoryHypotheses(hypothesisSet);
        }

        public string ToXmlString()
        {
            var entries = _hypotheses.Select(x => x.ToXmlString());
            return "<category>" + string.Join("", entries.ToArray()) + "</category>";
        }
    }
}