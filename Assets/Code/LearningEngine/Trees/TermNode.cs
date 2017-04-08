namespace LearningEngine
{
    internal class TermNode : ITreeNode
    {
        // Semantics
        private readonly SemValue _semantics;

        // Syntactic category
        private readonly CategoryLabel _synCat;

        // Written form
        private readonly string _writtenForm;

        // Private constructor
        private TermNode(CategoryLabel synCat, string writtenForm, string lambdaString)
        {
            _synCat = synCat;
            _writtenForm = writtenForm;
            _semantics = new SemValue(lambdaString);
        }

        public CategoryLabel Category
        {
            get { return _synCat; }
        }

        public SemValue Semantics
        {
            get { return _semantics; }
        }

        // Get as flat string
        public string GetFlatString()
        {
            return _writtenForm;
        }

        // Generate XML representation
        public string GetXmlString()
        {
            return "<" + _synCat + " value=" + _semantics.Value
                   + ">" + _writtenForm + "</" + _synCat + ">";
        }

        // Factory method for creating new instances
        public static TermNode Create(CategoryLabel synCat, string writtenForm, string lambdaString)
        {
            return new TermNode(synCat, writtenForm, lambdaString);
        }
    }
}