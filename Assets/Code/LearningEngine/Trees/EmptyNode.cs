namespace LearningEngine
{
    class EmptyNode : ITreeNode
    {
        private static int _counter = 0;
        private readonly int _identifier;
        private readonly SemValue _semantics = new SemValue("");
        public int Identifier { get { return _identifier; } }
        public SemValue Semantics { get { return _semantics; } }

        public EmptyNode()
        {
            _identifier = _counter++;
        }

        public string GetFlatString()
        {
            return "";
        }

        public string GetXMLString()
        {
            return "";
        }
    }
}
