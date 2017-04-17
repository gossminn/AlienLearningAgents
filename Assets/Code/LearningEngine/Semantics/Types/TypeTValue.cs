namespace LearningEngine
{
    // TODO: should be dependent on Situation?
    internal class TypeTValue : ISemanticValue
    {
        private readonly bool _value;

        private TypeTValue(bool value)
        {
            _value = value;
        }

        public bool Value
        {
            get { return _value; }
        }

        public static TypeTValue Create(bool truth)
        {
            return new TypeTValue(truth);
        }

        public SemanticResult TryApply(ISemanticValue argument)
        {
            return SemanticResult.CreateFailure();
        }
    }
}