using System.Collections.Generic;
using System.Collections.Immutable;

namespace LearningEngine
{
    interface ISyntaxRule
    {
        ParseResult Parse(ImmutableList<string> input, RuleSet rules);
        ParseResult Parse(ImmutableList<string> input, RuleSet rules, int n); // recursive helper
        IEnumerable<ITreeNode> GenerateAll(RuleSet rules);
        CategoryLabel Left { get; }
        string GetXMLString();
    }
}
