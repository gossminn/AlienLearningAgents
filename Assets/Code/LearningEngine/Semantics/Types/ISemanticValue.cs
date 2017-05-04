using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Semantics.Types
{
    // Helper function for composing evaluation functions

    internal interface ISemanticValue
    {
        // Try to apply another value this current value as function
        SemanticResult TryApply(ISemanticValue argument);

        // Determine whether value is relevant in current model
        bool AppliesToModel(LogicalModel model);
    }
}