using System;

namespace LearningEngine
{
    internal class TypeEtEValue : ISemanticValue
    {
        private readonly Func<TypeEtValue, TypeEValue> _value;

        private TypeEtEValue(Func<TypeEtValue, TypeEValue> value)
        {
           _value = value;
        }

        public Func<TypeEtValue, TypeEValue> Value
        {
            get { return _value; }
        }

        public static TypeEtEValue Create(Func<TypeEtValue, TypeEValue> value)
        {
            return new TypeEtEValue(value);
        }

        public SemanticResult TryApply(ISemanticValue argument)
        {
            if (argument is TypeEtValue)
            {
                return SemanticResult.CreateSuccess(_value((TypeEtValue) argument));
            }
            return SemanticResult.CreateFailure();
        }
    }
}