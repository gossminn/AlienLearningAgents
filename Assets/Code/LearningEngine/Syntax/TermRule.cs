using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    class TermRule : ISyntaxRule
    {
        private readonly CategoryLabel _left;
        private readonly ImmutableList<TermNode> _right;
        public CategoryLabel Left { get { return _left; } }
        public ImmutableList<TermNode> Right { get { return _right; } }

        // Private constructor
        private TermRule(CategoryLabel left, ImmutableList<TermNode> right)
        {
            _left = left;
            _right = right;
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

        public IEnumerable<ITreeNode> GenerateAll(RuleSet _)
        {
            return _right.AsEnumerable().Cast<ITreeNode>();
        }

        public ParseResult Parse(ImmutableList<string> input, RuleSet rules)
        {
            return Parse(input, rules, 0);
        }

        public ParseResult Parse(ImmutableList<string> input, RuleSet _, int n)
        {
            Predicate<TermNode> match = x => x.GetFlatString() == input[n];

            if (_right.Exists(match))
            {
                return ParseResult.MakeSuccess(_right.Find(match), n+1);
            }

            return ParseResult.MakeFailure();
        }

        public string GetXMLString()
        {
            var leftEntry = "<left>" + _left.ToString() + "</left>";
            var rightEntries = "<right>"
                + String.Join(",", _right.Select(x => x.GetFlatString()).ToArray())
                + "</right>";
            return "<TermRule>" + leftEntry + rightEntries + "</TermRule>";
        }
    }
}
