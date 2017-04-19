namespace LearningEngine
{
    internal class EmptyValue : ISemanticValue
    {
        // Empty constructor
        private EmptyValue()
        {}

        public SemanticResult TryApply(ISemanticValue argument)
        {
            return SemanticResult.CreateFailure();
        }

        public static EmptyValue Create()
        {
            return new EmptyValue();
        }
    }
}