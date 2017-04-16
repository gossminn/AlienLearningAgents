namespace LearningEngine
{
    internal class EmptyNode : ITreeNode
    {
        private static int _counter;
        private readonly int _identifier;
        private readonly LambdaExpression _semantics = new LambdaExpression("");

        private EmptyNode()
        {
            _identifier = _counter++;
        }

        public CategoryLabel Category
        {
            get { return CategoryLabel.EmptyCat; }
        }

        public int Identifier
        {
            get { return _identifier; }
        }

        public LambdaExpression Semantics
        {
            get { return _semantics; }
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