using System;
using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Semantics.Types
{
    internal static class Evaluations
    {
        public static Func<LogicalModel, ComposeResult> TryCompose(
            Func<LogicalModel, ISemanticValue> f, Func<LogicalModel, ISemanticValue> g)
        {
            // Given a model, try to compose f and g
            Func<LogicalModel, SemanticResult> composed = m => f(m).TryApply(g(m));

            // Return a function that, given a model, decides whether composition is succesful
            return m1 => composed(m1).Success
                ? ComposeResult.CreateSuccess(m2 => composed(m2).Value)
                : ComposeResult.CreateFailure();
        }
    }
}