namespace LearningEngine
{
    internal interface ISemanticValue
    {
        SemanticResult TryApply(ISemanticValue argument);
    }
}