using System.Collections.Generic;
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

        // Constructor
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

            // Make set from enumerable
            return MakeHypothesisSet(hypotheses);
        }

        // Produce a guess
        public MeaningHypothesisSet Guess()
        {
            // Make a guess for each hypothesis
            var hypotheses = _hypotheses.Select(h => h.Guess());

            // Return as hypothesis set
            return MakeHypothesisSet(hypotheses);
        }

        // Evaluate based on context
        public MeaningHypothesisSet Evaluate(LogicalModel model, ImmutableArray<string> words)
        {
            // Evaluate all hypotheses
            var hypotheses = _hypotheses.Select(h => h.Evaluate(model, words));

            // Make new hypothesis set
            return MakeHypothesisSet(hypotheses);
        }


        // Helper method for adding hypotheses to a set
        private static MeaningHypothesisSet MakeHypothesisSet(IEnumerable<SyntaxCategoryHypotheses> hypotheses)
        {
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

        // Overall set is fixed when every category is fixed
        public bool IsFixed()
        {
            return _hypotheses.All(h => h.IsFixed());
        }

        // Find unique meaning for a given word (assumes one and only one meaning is present)
        public MeaningCandidate FindMeaningFor(string word)
        {
            return _hypotheses
                .Single(h => h.HasMeaningFor(word))
                .FindMeaningFor(word);
        }


        public string ToXmlString()
        {
            var entries = _hypotheses.Select(x => x.ToXmlString());
            return "<meanings>" + string.Join("", entries.ToArray()) + "</meanings>";
        }
    }
}