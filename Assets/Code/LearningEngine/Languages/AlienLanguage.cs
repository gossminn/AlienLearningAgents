using System.Linq;

namespace LearningEngine
{
    static class AlienLanguage
    {
        public static ParentAgent MakeParentAgent()
        {
            // Categories
            var sentCat = SyntaxCat.Create(CatType.NonTerminal);
            var npCat = SyntaxCat.Create(CatType.NonTerminal);
            var vpCat = SyntaxCat.Create(CatType.NonTerminal);
            var nrootCat = SyntaxCat.Create(CatType.Terminal);
            var nmodCat = SyntaxCat.Create(CatType.Terminal);
            var vCat = SyntaxCat.Create(CatType.Terminal);

            var categorySet = CategorySet.CreateEmpty()
                .SetRootCat(sentCat)
                .AddCategory(npCat)
                .AddCategory(vpCat)
                .AddCategory(nrootCat)
                .AddCategory(nmodCat)
                .AddCategory(vCat);

            // Terminals
            var terminalSet = TerminalSet.CreateEmpty()
                .AddTerminal(new TermNode(nrootCat, "wu", "WU"))
                .AddTerminal(new TermNode(nrootCat, "zu", "ZU"))
                .AddTerminal(new TermNode(nrootCat, "lu", "LU"))
                .AddTerminal(new TermNode(nrootCat, "du", "DU"))
                .AddTerminal(new TermNode(nmodCat, "ba", "/x[BA(x)]"))
                .AddTerminal(new TermNode(nmodCat, "pa", "/x[PA(x)]"))
                .AddTerminal(new TermNode(nmodCat, "bo", "/x[BO(x)]"))
                .AddTerminal(new TermNode(nmodCat, "po", "/x[PO(x)]"))
                .AddTerminal(new TermNode(vCat, "ba", "/y[/x[BA(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "pa", "/y[/x[PA(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "bo", "/y[/x[BO(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "po", "/y[/x[PO(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "keba", "/y[/x[KEBA(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "kepa", "/y[/x[KEPA(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "kebo", "/y[/x[KEBO(x, y)]]"))
                .AddTerminal(new TermNode(vCat, "kepo", "/y[/x[KEPO(x, y)]]"));

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
