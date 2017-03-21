namespace LearningEngine
{
    class ParseResult
    {
        // Static instance: failure state
        private static readonly ParseResult _failure =
            new ParseResult(false, new EmptyNode(), 0);

        // Object attributes
        private readonly bool _success;
        private readonly ITreeNode _tree;
        private readonly int _index;
        public bool Success { get { return _success; } }
        public ITreeNode Tree { get { return _tree; } }
        public int Index { get { return _index; } }

        private ParseResult(bool success, ITreeNode tree, int index)
        {
            _success = success;
            _tree = tree;
            _index = index;
        }

        public static ParseResult MakeSuccess(ITreeNode tree, int index)
        {
            return new ParseResult(true, tree, index);
        }

        public static ParseResult MakeFailure()
        {
            return _failure;
        }

    }
}
