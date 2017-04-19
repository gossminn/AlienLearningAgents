using System;

namespace LearningEngine
{
    internal class ComposeResult
    {
        private readonly bool _success;
        private readonly Func<LogicalModel, ISemanticValue> _value;

        private ComposeResult(bool success, Func<LogicalModel, ISemanticValue> value)
        {
            _success = success;
            _value = value;
        }

        public bool Success
        {
            get { return _success; }
        }

        public Func<LogicalModel, ISemanticValue> Value
        {
            get { return _value; }
        }

        public static ComposeResult CreateFailure()
        {
            return new ComposeResult(false, null);
        }

        public static ComposeResult CreateSuccess(Func<LogicalModel, ISemanticValue> value)
        {
            return new ComposeResult(true, value);
        }
    }
}