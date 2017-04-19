using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal class TermRule : ISyntaxRule
    {
        private readonly CategoryLabel _left;
        private readonly ImmutableList<TermNode> _right;

        // Private constructor
        private TermRule(CategoryLabel left, ImmutableList<TermNode> right)
        {
            _left = left;
            _right = right;
        }

        public ImmutableList<TermNode> Right
        {
            get { return _right; }
        }

        public CategoryLabel Left
        {
            get { return _left; }
        }

        public IEnumerable<ITreeNode> GenerateAll(RuleSet _, LogicalModel model)
        {
            return _right.AsEnumerable().Cast<ITreeNode>();
        }

        public ParseResult Parse(ImmutableList<string> input, RuleSet rules, LogicalModel model)
        {
            return Parse(input, rules, 0, model);
        }

        public ParseResult Parse(ImmutableList<string> input, RuleSet _, int n, LogicalModel model)
        {
            Predicate<TermNode> match = x => x.GetFlatString() == input[n];

            if (_right.Exists(match))
                return ParseResult.MakeSuccess(_right.Find(match), n + 1);

            return ParseResult.MakeFailure();
        }

        public string GetXmlString()
        {
            var leftEntry = "<left>" + _left + "</left>";
            var rightEntries = "<right>"
                               + string.Join(",", _right.Select(x => x.GetFlatString()).ToArray())
                               + "</right>";
            return "<TermRule>" + leftEntry + rightEntries + "</TermRule>";
        }

        // Factory method for creating TermRule with empty right side
        public static TermRule CreateEmpty(CategoryLabel left)
        {
            return new TermRule(left, ImmutableList<TermNode>.Empty);
        }

        // Method for updating the right side of the rule
        public TermRule AddToRight(TermNode right)
        {
            return new TermRule(_left, _right.Add(right));
        }
    }
}