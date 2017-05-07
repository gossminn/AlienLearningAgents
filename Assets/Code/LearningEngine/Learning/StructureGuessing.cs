using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Knowledge.MeaningHypotheses;
using Code.LearningEngine.Knowledge.Rules;
using Code.LearningEngine.Learning.LearningHelpers;
using Code.LearningEngine.Semantics;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;
using Code.LearningEngine.Syntax;
using Code.LearningEngine.Trees;

namespace Code.LearningEngine.Learning
{
    internal static class StructureGuessing
    {
        public static KnowledgeSet GuessStructure(this KnowledgeSet knowledge, MeaningHypothesisSet guess,
            ImmutableArray<string> words, LogicalModel model)
        {
            // Find and evaluate meanings
            var meaningSeq = words.Select(w => guess.FindMeaningFor(w).Meaning(model)).ToImmutableArray();

            // Get syntactic categories of words
            var termNodeSeq = GetTermNodeSequence(words, knowledge.Rules);
            var categorySeq = termNodeSeq.Select(x => x.Category).ToImmutableArray();

            // Construct rules from meaning sequence
            var structure = ConstructRules(NonTerminalCategorySet.CreateEmpty(), knowledge.Rules.ClearNonTerminals(),
                meaningSeq, categorySeq, 0, 1);

            // Return new version of knowledge set
            var rules = structure.Rules;
            var categories = structure.Categories;
            return knowledge.UpdateRules(rules).UpdateCategories(
                knowledge.Categories.UpdateNonTerminals(categories));
        }

        private static NonTerminalHelper ConstructRules(NonTerminalCategorySet categorySet, RuleSet ruleSet,
            ImmutableArray<ISemanticValue> meaningSeq, ImmutableArray<CategoryLabel> categorySeq,
                int firstIndex, int secondIndex)
        {
            // Associate two meanings
            var first = meaningSeq[firstIndex];
            var second = meaningSeq[secondIndex];

            // Try to apply first as function to second and vice versa
            var leftApply = first.TryApply(second);
            var rightApply = second.TryApply(first);

            // If neither application is succesful, try again with next words
            if (!leftApply.Success && !rightApply.Success)
            {
                return ConstructRules(categorySet, ruleSet, meaningSeq, categorySeq, firstIndex + 1, secondIndex + 1);
            }

            // Resulting semantic value for new constituent
            var meaning = leftApply.Success ? leftApply.Value : rightApply.Value;

            // Construct new nonterminal category and rule
            var category = CategoryLabel.Create(NodeType.NonTerminal);
            var functorLoc = leftApply.Success ? FunctorLoc.Left : FunctorLoc.Right;
            var rule = NonTermRule.CreateBinary(category, categorySeq[firstIndex], categorySeq[secondIndex],
                functorLoc);

            // Make new category and rule sets
            var categorySet1 = categorySet.AddCategory(category);
            var ruleSet1 = ruleSet.AddRule(rule);

            // Produce new meaning and category sequences
            var meaningSeq1 = meaningSeq.RemoveRange(firstIndex, 2).Insert(firstIndex, meaning);
            var categorySeq1 = categorySeq.RemoveRange(firstIndex, 2).Insert(firstIndex, category);

            // Only one element left in sequences? Stop and return result
            if (meaningSeq1.Length == 1)
            {
                // Add current rule as root
                var ruleSet2 = ruleSet1.SetRoot(rule);

                // Return result
                return NonTerminalHelper.Create(categorySet1, ruleSet2);
            }
            return ConstructRules(categorySet1, ruleSet1, meaningSeq1, categorySeq1, 0, 1);
        }


        // Parse a word to get the category label
        private static ITreeNode GetTermNodeOf(this RuleSet rules, string word)
        {
            // Check for empty string
            if (word == "")
                return EmptyNode.Create();

            var parseInput = ImmutableList<string>.Empty.Add(word);
            var parseResults = rules.TerminalRules
                .Select(rule => rule.Parse(parseInput, rules, LogicalModel.CreateEmpty()))
                .ToList();
            return parseResults.First(result => result.Success).Tree;
        }

        // Get sequence of category labels
        private static ImmutableArray<ITreeNode> GetTermNodeSequence(IEnumerable<string> words,
            RuleSet rules)
        {
            return words.Select(word => rules.GetTermNodeOf(word)).ToImmutableArray();
        }

    }
}