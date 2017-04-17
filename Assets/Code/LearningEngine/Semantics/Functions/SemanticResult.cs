namespace LearningEngine
{
    internal class SemanticResult
    {
        private readonly bool _success;
        private readonly ISemanticValue _value;

        private SemanticResult(bool success, ISemanticValue value)
        {
            _success = success;
            _value = value;
        }

        public static SemanticResult CreateFailure()
        {
            return new SemanticResult(false, null);
        }

        public static SemanticResult CreateSuccess(ISemanticValue value)
        {
            return new SemanticResult(true, value);
        }

        public bool Success
        {
            get { return _success; }
        }

        public ISemanticValue Value
        {
            get { return _value; }
        }
    }
}