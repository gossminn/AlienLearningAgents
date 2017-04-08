using System.Linq;

namespace LearningEngine
{
    internal static class AlienLanguage
    {
        public static ParentAgent MakeParentAgent()
        {
            // Categories
            var sentCat = CategoryLabel.Create(NodeType.NonTerminal);
            var npCat = CategoryLabel.Create(NodeType.NonTerminal);
            var vpCat = CategoryLabel.Create(NodeType.NonTerminal);
            var nrootCat = CategoryLabel.Create(NodeType.Terminal);
            var nmodCat = CategoryLabel.Create(NodeType.Terminal);
            var vCat = CategoryLabel.Create(NodeType.Terminal);

            var categorySet = CategorySet.CreateEmpty()
                .SetRootCat(sentCat)
                .AddCategory(npCat)
                .AddCategory(vpCat)
                .AddCategory(nrootCat)
                .AddCategory(nmodCat)
                .AddCategory(vCat);

            // Terminals
            var terminalSet = TerminalSet.CreateEmpty()
                .AddTerminal(TermNode.Create(nrootCat, "wu", "WU"))
                .AddTerminal(TermNode.Create(nrootCat, "zu", "ZU"))
                .AddTerminal(TermNode.Create(nrootCat, "lu", "LU"))
                .AddTerminal(TermNode.Create(nrootCat, "du", "DU"))
                .AddTerminal(TermNode.Create(nmodCat, "ba", "/x[BA(x)]"))
                .AddTerminal(TermNode.Create(nmodCat, "pa", "/x[PA(x)]"))
                .AddTerminal(TermNode.Create(nmodCat, "bo", "/x[BO(x)]"))
                .AddTerminal(TermNode.Create(nmodCat, "po", "/x[PO(x)]"))
                .AddTerminal(TermNode.Create(vCat, "ba", "/y[/x[BA(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "pa", "/y[/x[PA(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "bo", "/y[/x[BO(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "po", "/y[/x[PO(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "keba", "/y[/x[KEBA(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "kepa", "/y[/x[KEPA(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "kebo", "/y[/x[KEBO(x, y)]]"))
                .AddTerminal(TermNode.Create(vCat, "kepo", "/y[/x[KEPO(x, y)]]"));

            // Ruleset
            var terminalRules = terminalSet.ExtractRules();
            var nonTermSet = RuleSet.CreateEmpty()
                .AddRule(NonTermRule.CreateBinary(sentCat, npCat, vpCat, FunctorLoc.Right))
                .AddRule(NonTermRule.CreateBinary(npCat, nrootCat, nmodCat, FunctorLoc.Right))
                .AddRule(NonTermRule.CreateBinary(vpCat, vCat, npCat, FunctorLoc.Left));
            var rules = terminalRules.Aggregate(nonTermSet, (acc, next) => acc.AddRule(next));

            // Parent agent
            return ParentAgent.Create(categorySet, rules, terminalSet);
        }
    }
}