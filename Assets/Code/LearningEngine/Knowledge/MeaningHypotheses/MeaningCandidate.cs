using System;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;

namespace Code.LearningEngine.Knowledge.MeaningHypotheses
{
    // Store information about a possible meaning
    internal class MeaningCandidate
    {
        private readonly Func<LogicalModel, ISemanticValue> _meaning;
        private readonly int _score;

        private MeaningCandidate(Func<LogicalModel, ISemanticValue> meaning, int score)
        {
            _meaning = meaning;
            _score = score;
        }

        public Func<LogicalModel, ISemanticValue> Meaning
        {
            get { return _meaning; }
        }

        public int Score
        {
            get { return _score; }
        }

        public static MeaningCandidate Initialize(Func<LogicalModel, ISemanticValue> meaning)
        {
            return new MeaningCandidate(meaning, 0);
        }

        public MeaningCandidate IncreaseScore()
        {
            return new MeaningCandidate(_meaning, _score + 1);
        }

        public MeaningCandidate DecreaseScore()
        {
            return new MeaningCandidate(_meaning, _score - 1);
        }

        public string ToXmlString()
        {
            return "<candidate><meaning>" + _meaning.GetHashCode() + "</meaning><score>" + _score +
                   "</score></candidate>";
        }
    }
}