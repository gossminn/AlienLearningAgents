namespace LearningEngine
{
    internal class NonTermNode : ITreeNode
    {
        // Branches
        private readonly ITreeNode _left;
        private readonly ITreeNode _right;

        // Semantic value
        private readonly SemValue _semantics;

        // Syntactic type
        private readonly CategoryLabel _synCat;

        // Constructor
        private NonTermNode(CategoryLabel synCat, ITreeNode left, ITreeNode right, FunctorLoc functor)
        {
            _synCat = synCat;
            _left = left;
            _right = right;

            if (functor == FunctorLoc.Left)
                _semantics = _left.Semantics.LambdaApply(_right.Semantics);
            else
                _semantics = _right.Semantics.LambdaApply(_left.Semantics);
        }

        public SemValue Semantics
        {
            get { return _semantics; }
        }

        public string GetXmlString()
        {
            return "<" + _synCat + " value={" + _semantics.Value
                   + ">" + _left.GetXmlString() + _right.GetXmlString() + "</{_synCat}>";
        }

        public string GetFlatString()
        {
            var entries = _left.GetFlatString() + " " + _right.GetFlatString();
            return entries.Trim();
        }

        public static NonTermNode Create(CategoryLabel synCat, ITreeNode left, ITreeNode right, FunctorLoc functor)
        {
            return new NonTermNode(synCat, left, right, functor);
        }
    }
}