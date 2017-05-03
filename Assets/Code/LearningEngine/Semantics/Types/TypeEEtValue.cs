using System;
using System.Collections.Immutable;
using System.Linq;

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

        public bool AppliesToModel(LogicalModel model)
        {
            // Define cartesian products of all entities times itself
            var entities = new[] {Entity.Entity1, Entity.Entity2};
            var product = entities.SelectMany(
                e1 => entities.Select(
                    e2 => new {A = TypeEValue.Create(e1), B = TypeEValue.Create(e2)}));

            // For every combination of pairs, produce a truth value
            var truthValues = product.Aggregate(
                ImmutableList<TypeTValue>.Empty,
                (acc, next) => acc.Add(_value(next.A).Value(next.B))
            );

            // Check if any boolean is true
            return truthValues.Any(x => x.Value);
        }
    }
}