using System;
using System.Collections.Immutable;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Learning
{
    internal static class WordSemanticsLearning
    {
        public static KnowledgeSet LearnWordSemantics(this KnowledgeSet knowledgeSet, LogicalModel model,
            ImmutableArray<string> words)
        {
            // Evaluate hypothesisset against model
            var evaluated = knowledgeSet.Hypotheses.Evaluate(model, words);

            // If hypotheses have been corrupted: print error message
            if (!evaluated.IsRelevant())
            {
                throw new Exception("SemanticLearningError!");
            }

            // Return knowledge set with evaluated hypotheses
            return knowledgeSet.UpdateHypotheses(evaluated);
        }
    }
}