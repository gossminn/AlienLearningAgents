//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using Code.LearningEngine.Knowledge;
//using Code.LearningEngine.Knowledge.Categories;
//using Code.LearningEngine.Knowledge.Rules;
//using Code.LearningEngine.Learning.LearningHelpers;
//using Code.LearningEngine.Semantics;
//using Code.LearningEngine.Semantics.Model;
//using Code.LearningEngine.Syntax;
//
//namespace Code.LearningEngine.Learning
//{
//    internal static class ConstituentLearning
//    {
//        // Random number generator
//        private static readonly Random _random = new Random();
//
//        // Wrapper function
//        public static KnowledgeSet LearnConstituents(this KnowledgeSet knowledge,
//            IEnumerable<string> words)
//        {
//            // Old rules
//            var rules = knowledge.Rules;
//
//            // Infer new rules, store in helper object
//            var helper = rules.InferFromInput(words);
//
//            // Old categories
//            var categories0 = knowledge.Categories;
//
//            // Update categories
//            var categories1 = categories0.UpdateRawNonTerminals(helper.Categories);
//
//            // Update rule set
//            var ruleSet = rules.ClearNonterminals().AddRules(helper.Rules);
//
//            // New knowledge set with updated categories and new rules
//            return knowledge
//                .UpdateCategories(categories1)
//                .UpdateRules(ruleSet);
//        }
//

//
//        // Infer non-terminal categories and rules
//        public static NonTerminalHelper InferFromInput(this RuleSet rules,
//            IEnumerable<string> words)
//        {
//            // Parse words to get sequence of CategoryLabels, then of Monograms
//            var sequence = GetSequence(words, rules);
//
//            // Infer categories from sequence
//            return InferFromCatSequence(NonTerminalCategorySet.CreateEmpty(),
//                ImmutableHashSet<NonTermRule>.Empty, sequence);
//        }
//
//        private static NonTerminalHelper InferFromCatSequence(NonTerminalCategorySet newCategories0,
//            ImmutableHashSet<NonTermRule> newRules0, ImmutableArray<CategoryLabel> sequence0)
//        {
//            // Step 1: randomly group two categories together
//            var num = _random.Next(sequence0.Length - 1);
//            var firstElem = sequence0[num];
//            var secondElem = sequence0[num + 1];
//
//            // Step 2: create a new category, and a rule relating that category to the pair
//            var category = CategoryLabel.Create(NodeType.NonTerminal);
//            var rule = NonTermRule.CreateBinary(category, firstElem, secondElem, FunctorLoc.Left);
//            var newCategories1 = newCategories0.AddCategory(category);
//            var newRules1 = newRules0.Add(rule);
//
//            // Step 3: get rest of the sequence (everything up until num, then the rest from num + 2)
//            var before = sequence0.Take(num);
//            var after = sequence0.Skip(num + 2);
//
//            // Step 4: make new sequence, with grouped categories replaced by new category
//            var sequence1 = before
//                .Concat(ImmutableArray<CategoryLabel>.Empty.Add(category))
//                .Concat(after)
//                .ToImmutableArray();
//
//            // Step 5: if only one element left, return, else do recursive call
//            if (sequence1.Length > 1)
//            {
//                return InferFromCatSequence(newCategories1, newRules1, sequence1);
//            }
//            return NonTerminalHelper.Create(newCategories1, newRules1);
//        }
//    }
//}