using System;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Semantics.Types
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

        public bool AppliesToModel(LogicalModel model)
        {
            // Define set of possible <e,t> values
            var species = AtomicMeanings.Species.Select(x => (TypeEtValue) x(model));

            // Make list of entities that current value can refer to in model
            var values = species.Aggregate(ImmutableList<Entity>.Empty,
                (acc, next) => acc.Add(_value(next).Value));

            // Check if list contains anything that is not 'Nothing'
            return values.Any(x => x != Entity.Nothing);
        }
    }
}