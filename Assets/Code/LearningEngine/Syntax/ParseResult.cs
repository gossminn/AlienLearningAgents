namespace LearningEngine
{
    internal class ParseResult
    {
        // Static instance: failure state
        private static readonly ParseResult _failure =
            new ParseResult(false, EmptyNode.Create(), 0);

        // Index: where to start parsing?
        private readonly int _index;

        // Was parsing succesful?
        private readonly bool _success;

        // Store parsing result as tree structure
        private readonly ITreeNode _tree;

        private ParseResult(bool success, ITreeNode tree, int index)
        {
            _success = success;
            _tree = tree;
            _index = index;
        }

        public bool Success
        {
            get { return _success; }
        }

        public ITreeNode Tree
        {
            get { return _tree; }
        }

        public int Index
        {
            get { return _index; }
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