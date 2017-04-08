using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal class NonTermRule : ISyntaxRule
    {
        private readonly FunctorLoc _functor;
        private readonly CategoryLabel _left;
        private readonly CategoryLabel _right1;
        private readonly CategoryLabel _right2;

        // Private constructor
        private NonTermRule(CategoryLabel left, CategoryLabel right1, CategoryLabel right2, FunctorLoc functor)
        {
            _left = left;
            _right1 = right1;
            _right2 = right2;
            _functor = functor;
        }

        public CategoryLabel Right1
        {
            get { return _right1; }
        }

        public CategoryLabel Right2
        {
            get { return _right2; }
        }

        public CategoryLabel Left
        {
            get { return _left; }
        }

        public ParseResult Parse(ImmutableList<string> input, RuleSet rules)
        {
            return Parse(input, rules, 0);
        }

        public ParseResult Parse(ImmutableList<string> input, RuleSet rules, int n)
        {
            // Find rule for right1 and try to apply it
            var rule1 = rules.FindWithLeftSide(_right1);
            var result1 = rule1.Parse(input, rules, n);

            // If unsuccesful, return failure
            if (!result1.Success)
                return ParseResult.MakeFailure();

            // If rule is unary: return success
            if (_right2 == CategoryLabel.EmptyCat)
                return ParseResult.MakeSuccess(
                    NonTermNode.Create(_left, result1.Tree, EmptyNode.Create(), _functor),
                    result1.Index
                );

            // Find rule for right2 and try to apply
            var rule2 = rules.FindWithLeftSide(_right2);
            var result2 = rule2.Parse(input, rules, result1.Index);

            // If unsuccesful, return failure
            if (!result2.Success)
                return ParseResult.MakeFailure();

            // Return final result
            return ParseResult.MakeSuccess(
                NonTermNode.Create(_left, result1.Tree, result2.Tree, _functor),
                result2.Index
            );
        }

        public IEnumerable<ITreeNode> GenerateAll(RuleSet rules)
        {
            // If rule is unary: return all possible values for right1
            if (_right2 == CategoryLabel.EmptyCat)
                return rules.FindWithLeftSide(_right1)
                    .GenerateAll(rules)
                    .Select(child => NonTermNode.Create(_left, child, EmptyNode.Create(), _functor))
                    .Cast<ITreeNode>();

            // If rule is binary: return combinations of right1 and right2
            return rules.FindWithLeftSide(_right1)
                .GenerateAll(rules)
                .SelectMany(child1 => rules.FindWithLeftSide(_right2)
                    .GenerateAll(rules)
                    .Select(child2 => NonTermNode.Create(_left, child1, child2, _functor))
                )
                .Cast<ITreeNode>();
        }

        public string GetXmlString()
        {
            var leftEntry = "<left>" + _left + "</left>";
            var rightEntry = "<right>" + _right1 + ","
                             + _right2 + "</right>";
            return "<NonTermRule>" + leftEntry + rightEntry + "</NonTermRule>";
        }

        // Factory methods for unary and binary rules
        public static NonTermRule CreateUnary(CategoryLabel left, CategoryLabel right)
        {
            return new NonTermRule(left, right, CategoryLabel.EmptyCat, FunctorLoc.Left);
        }

        public static NonTermRule CreateBinary
            (CategoryLabel left, CategoryLabel right1, CategoryLabel right2, FunctorLoc functor)
        {
            return new NonTermRule(left, right1, right2, functor);
        }
    }
}