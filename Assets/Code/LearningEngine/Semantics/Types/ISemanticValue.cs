namespace LearningEngine
{
    // Helper function for composing evaluation functions

    internal interface ISemanticValue
    {
        SemanticResult TryApply(ISemanticValue argument);
    }
}