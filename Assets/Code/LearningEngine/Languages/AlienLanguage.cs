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

            // Set root category
            var rootCat = sentCat;

            var categorySet = TerminalCategorySet.CreateEmpty()
                .AddCategory(npCat)
                .AddCategory(vpCat)
                .AddCategory(nrootCat)
                .AddCategory(nmodCat)
                .AddCategory(vCat);

            // Atomic functions
            var speciesAtoms = AtomicFunctions.Species.ToArray();
            var directionAtoms = AtomicFunctions.Directions.ToArray();
            var spatialAtoms = AtomicFunctions.SpatialRelations.ToArray();

            // Terminals
            var terminalSet = VocabularySet.CreateEmpty()
                .AddTerminal(TermNode.Create(nrootCat, "zu", speciesAtoms[0]))
                .AddTerminal(TermNode.Create(nrootCat, "lu", speciesAtoms[1]))
                .AddTerminal(TermNode.Create(nrootCat, "wu", speciesAtoms[2]))
                .AddTerminal(TermNode.Create(nrootCat, "du", speciesAtoms[3]))
                .AddTerminal(TermNode.Create(nmodCat, "ba", directionAtoms[0]))
                .AddTerminal(TermNode.Create(nmodCat, "pa", directionAtoms[1]))
                .AddTerminal(TermNode.Create(nmodCat, "bo", directionAtoms[2]))
                .AddTerminal(TermNode.Create(nmodCat, "po", directionAtoms[3]))
                .AddTerminal(TermNode.Create(vCat, "keba", spatialAtoms[0]))
                .AddTerminal(TermNode.Create(vCat, "kaba", spatialAtoms[1]))
                .AddTerminal(TermNode.Create(vCat, "kepa", spatialAtoms[2]))
                .AddTerminal(TermNode.Create(vCat, "kapa", spatialAtoms[3]))
                .AddTerminal(TermNode.Create(vCat, "kebo", spatialAtoms[4]))
                .AddTerminal(TermNode.Create(vCat, "kabo", spatialAtoms[5]))
                .AddTerminal(TermNode.Create(vCat, "kepo", spatialAtoms[6]))
                .AddTerminal(TermNode.Create(vCat, "kapo", spatialAtoms[7]));

            // Ruleset
            var terminalRules = terminalSet.ExtractRules();
            var nonTermSet = RuleSet.CreateEmpty()
                .AddRule(NonTermRule.CreateBinary(sentCat, npCat, vpCat, FunctorLoc.Right))
                .AddRule(NonTermRule.CreateBinary(npCat, nrootCat, nmodCat, FunctorLoc.Right))
                .AddRule(NonTermRule.CreateBinary(vpCat, vCat, npCat, FunctorLoc.Left));
            var rules = terminalRules.Aggregate(nonTermSet, (acc, next) => acc.AddRule(next));

            // Parent agent
            return ParentAgent.Create(categorySet, rules, terminalSet,
                rootCat, LogicalModel.CreateEmpty());
        }
    }
}