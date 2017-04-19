using System;

namespace LearningEngine
{
    internal class TypeEEtValue : ISemanticValue
    {
        private readonly Func<TypeEValue, TypeEtValue> _value;

        private TypeEEtValue(Func<TypeEValue, TypeEtValue> value)
        {
            _value = value;
        }

        public Func<TypeEValue, TypeEtValue> Value
        {
            get { return _value; }
        }

        public static TypeEEtValue Create(Func<TypeEValue, TypeEtValue> value)
        {
            return new TypeEEtValue(value);
        }

        public SemanticResult TryApply(ISemanticValue argument)
        {
            if (argument is TypeEValue)
            {
                return SemanticResult.CreateSuccess(_value((TypeEValue) argument));
            }
            return SemanticResult.CreateFailure();
        }
    }
}