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

        public bool AppliesToModel(LogicalModel model)
        {
            return false;
        }

        public static EmptyValue Create()
        {
            return new EmptyValue();
        }
    }
}