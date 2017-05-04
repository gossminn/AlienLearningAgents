using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Trees
{
    internal class EmptyNode : ITreeNode
    {
        private static int _counter;
        private readonly int _identifier;
        private readonly ISemanticValue _semantics;

        private EmptyNode()
        {
            _identifier = _counter++;
            _semantics = EmptyValue.Create();
        }

        public CategoryLabel Category
        {
            get { return CategoryLabel.EmptyCat; }
        }

        public int Identifier
        {
            get { return _identifier; }
        }

        public ISemanticValue GetSemantics(LogicalModel model)
        {
            return _semantics;
        }

        public bool GetTruthValue()
        {
            return false;
        }

        public string GetFlatString()
        {
            return "";
        }

        public string GetXmlString()
        {
            return "";
        }

        public static EmptyNode Create()
        {
            return new EmptyNode();
        }
    }
}