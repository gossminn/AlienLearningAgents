namespace LearningEngine
{
    class NonTermNode : ITreeNode
    {
        // Syntactic type
        private readonly SyntaxCat _synCat;

        // Semantic value
        private readonly SemValue _semantics;
        public SemValue Semantics { get { return _semantics; } }

        // Branches
        ITreeNode _left;
        ITreeNode _right;

        // Constructor
        public NonTermNode(SyntaxCat synCat, ITreeNode left, ITreeNode right, FunctorLoc functor)
        {
            _synCat = synCat;
            _left = left;
            _right = right;

            if (functor == FunctorLoc.Left)
            {
                _semantics = _left.Semantics.LambdaApply(_right.Semantics);
            }
            else
            {
                _semantics = _right.Semantics.LambdaApply(_left.Semantics);
            }
        }

        public string GetXMLString()
        {
            return "<" + _synCat + " value={" + _semantics.Value 
                + ">" + _left.GetXMLString() + _right.GetXMLString() + "</{_synCat}>";
        }

        public string GetFlatString()
        {
            var entries = _left.GetFlatString() + " " + _right.GetFlatString();
            return entries.Trim();
        }
    }
}
