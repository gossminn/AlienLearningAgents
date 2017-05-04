using Code.LearningEngine.Semantics;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Trees
{
    internal class NonTermNode : ITreeNode
    {
        // Branches
        private readonly ITreeNode _left;

        private readonly ITreeNode _right;

        // Semantic value
        private readonly ISemanticValue _semantics;

        // Syntactic type
        private readonly CategoryLabel _synCat;

        // Constructor
        private NonTermNode(CategoryLabel synCat, ITreeNode left, ITreeNode right,
            FunctorLoc functor, LogicalModel model)
        {
            _synCat = synCat;
            _left = left;
            _right = right;

            if (functor == FunctorLoc.Left)
                _semantics = _left.GetSemantics(model).TryApply(right.GetSemantics(model)).Value;
            else
                _semantics = _right.GetSemantics(model).TryApply(_left.GetSemantics(model)).Value;
        }

        public ISemanticValue Semantics
        {
            get { return _semantics; }
        }

        // TODO: is LogicModel parameter really necessary?
        public ISemanticValue GetSemantics(LogicalModel model)
        {
            return _semantics;
        }

        public bool GetTruthValue()
        {
            if (_semantics is TypeTValue)
            {
                var typeT = (TypeTValue) _semantics;
                return typeT.Value;
            }
            return false;
        }

        public CategoryLabel Category
        {
            get { return _synCat; }
        }

        public string GetXmlString()
        {
            return "<" + _synCat + " value={" + _semantics
                   + ">" + _left.GetXmlString() + _right.GetXmlString() + "</{_synCat}>";
        }

        public string GetFlatString()
        {
            var entries = _left.GetFlatString() + " " + _right.GetFlatString();
            return entries.Trim();
        }

        public static NonTermNode Create(CategoryLabel synCat, ITreeNode left, ITreeNode right,
            FunctorLoc functor, LogicalModel model)
        {
            return new NonTermNode(synCat, left, right, functor, model);
        }
    }
}