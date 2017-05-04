using System.Collections.Generic;
using System.Collections.Immutable;
using Code.LearningEngine.Knowledge.Rules;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Trees;

namespace Code.LearningEngine.Syntax
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