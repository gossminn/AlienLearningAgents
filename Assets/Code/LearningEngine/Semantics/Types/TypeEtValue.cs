using System;

namespace LearningEngine
{
    internal class TypeEtValue : ISemanticValue
    {
        private readonly Func<TypeEValue, TypeTValue> _value;

        private TypeEtValue(Func<TypeEValue, TypeTValue> value)
        {
            _value = value;
        }

        public Func<TypeEValue, TypeTValue> Value
        {
            get { return _value; }
        }

        public SemanticResult TryApply(ISemanticValue argument)
        {
            if (argument is TypeEValue)
            {
                return SemanticResult.CreateSuccess(_value((TypeEValue) argument));
            }
            return SemanticResult.CreateFailure();
        }

        public static TypeEtValue Create(Func<TypeEValue, TypeTValue> value)
        {
            return new TypeEtValue(value);
        }
    }
}