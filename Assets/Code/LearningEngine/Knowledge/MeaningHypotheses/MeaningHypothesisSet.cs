using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Wrapper type for the entire hypothesis space about word meanings
    internal class MeaningHypothesisSet
    {
        // For each category, store hypotheses about its semantics
        private readonly ImmutableHashSet<SyntaxCategoryHypotheses> _hypotheses;

        private MeaningHypothesisSet(ImmutableHashSet<SyntaxCategoryHypotheses> hypotheses)
        {
            _hypotheses = hypotheses;
        }

        // Generate initial hypothesis set
        public static MeaningHypothesisSet Initialize(TerminalCategorySet categories)
        {
            // Get set of words for each category
            var wordSets = categories.Categories.Values.Select(x => x.Words);

            // Generate initial hypotheses for each set
            var hypotheses = wordSets.Select(SyntaxCategoryHypotheses.Initialize);

            // Add to HashSet
            var hypothesisSet = hypotheses.Aggregate(
                ImmutableHashSet<SyntaxCategoryHypotheses>.Empty,
                (acc, next) => acc.Add(next));
            return new MeaningHypothesisSet(hypothesisSet);
        }

        // Are hypotheses still relevant (otherwise it's corrupt)?
        public bool IsRelevant()
        {
            return _hypotheses.All(h => h.IsRelevant());
        }

        // Evaluate based on context
        public MeaningHypothesisSet Evaluate(LogicalModel model, ImmutableArray<string> words)
        {
            // Evaluate all hypotheses
            var hypotheses = _hypotheses.Select(h => h.Evaluate(model, words));

            // Make new hypothesis set
            var hypothesisSet = hypotheses.Aggregate(
                ImmutableHashSet<SyntaxCategoryHypotheses>.Empty,
                (acc, next) => acc.Add(next));
            return new MeaningHypothesisSet(hypothesisSet);
        }

        public string ToXmlString()
        {
            var entries = _hypotheses.Select(x => x.ToXmlString());
            return "<meanings>" + string.Join("", entries.ToArray()) + "</meanings>";
        }
    }
}