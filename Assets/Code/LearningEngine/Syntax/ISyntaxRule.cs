using System.Collections.Generic;
using System.Collections.Immutable;

namespace LearningEngine
{
    internal interface ISyntaxRule
    {
        CategoryLabel Left { get; }
        ParseResult Parse(ImmutableList<string> input, RuleSet rules);
        ParseResult Parse(ImmutableList<string> input, RuleSet rules, int n); // recursive helper
        IEnumerable<ITreeNode> GenerateAll(RuleSet rules);

        string GetXmlString();
    }
}