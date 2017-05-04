using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Semantics.Types
{
    internal class TypeEValue : ISemanticValue
    {
        private readonly Entity _value;

        private TypeEValue(Entity value)
        {
            _value = value;
        }

        public Entity Value
        {
            get { return _value; }
        }

        public static TypeEValue Create(Entity entity)
        {
            return new TypeEValue(entity);
        }

        public SemanticResult TryApply(ISemanticValue argument)
        {
            return SemanticResult.CreateFailure();
        }

        public bool AppliesToModel(LogicalModel model)
        {
            return true;
        }
    }
}