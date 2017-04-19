using System.Collections.Generic;
using System.Collections.Immutable;

namespace LearningEngine
{
    internal interface ISyntaxRule
    {
        CategoryLabel Left { get; }
        ParseResult Parse(ImmutableList<string> input, RuleSet rules, LogicalModel model);
        ParseResult Parse(ImmutableList<string> input, RuleSet rules, int n, LogicalModel model); // recursive helper
        IEnumerable<ITreeNode> GenerateAll(RuleSet rules, LogicalModel model);

        string GetXmlString();
    }
}