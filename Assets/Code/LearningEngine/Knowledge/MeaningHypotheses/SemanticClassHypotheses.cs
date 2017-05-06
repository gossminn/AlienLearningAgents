using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Stores meaning hypotheses (within a semantic class) for each word in a syntactic category
    internal class SemanticClassHypotheses
    {
        // For each word in a category, store the corresponding hypothesis space
        private readonly ImmutableDictionary<string, WordMeaningHypotheses> _hypotheses;

        private SemanticClassHypotheses(ImmutableDictionary<string, WordMeaningHypotheses> hypotheses)
        {
            _hypotheses = hypotheses;
        }

        // Generate initial hypotheses
        public static SemanticClassHypotheses Initialize(ImmutableHashSet<string> words,
            IEnumerable<Func<LogicalModel, ISemanticValue>> meanings)
        {
            // Generate initial hypotheses for each word and store in dictionary
            var hypothesisDict = words.Aggregate(
                ImmutableDictionary<string, WordMeaningHypotheses>.Empty,
                (acc, next) => acc.Add(next, WordMeaningHypotheses.Initialize(meanings)));
            return new SemanticClassHypotheses(hypothesisDict);
        }

        // Meanings are still relevant iff every word has a relevant meaning set
        public bool IsRelevant()
        {
            return _hypotheses.All(h => h.Value.IsRelevant());
        }

        // Does dictionary contain meaning for a given word?
        public bool HasMeaningFor(string word)
        {
            return _hypotheses.Keys.Contains(word);
        }

        // Produce a guess for a given word
        public Func<LogicalModel, ISemanticValue> Guess(string word)
        {
            return _hypotheses[word].Guess();
        }

        // Evaluate and eliminate hypotheses based on contexts
        public SemanticClassHypotheses Evaluate(LogicalModel model, ImmutableArray<string> words)
        {
            // Evaluate hypothesis space for each word
            var hypotheses = _hypotheses.Select(
                h => words.Contains(h.Key)
                    ? new {W = h.Key, M = h.Value.Evaluate(model)}
                    : new {W = h.Key, M = h.Value}); // leave non-relevant words intact

            // Store new hypotheses in dictionary
            var hypothesisDict = hypotheses.Aggregate(
                ImmutableDictionary<string, WordMeaningHypotheses>.Empty,
                (acc, next) => acc.Add(next.W, next.M));
            return new SemanticClassHypotheses(hypothesisDict);
        }

        public string ToXmlString()
        {
            var entries = _hypotheses.Select(x => "<word=" + x.Key + ">" + x.Value.ToXmlString() + "</word>");
            return "<words>" + string.Join("", entries.ToArray()) + "</words>";
        }
    }
}