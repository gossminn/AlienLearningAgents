namespace LearningEngine
{
    class TermNode : ITreeNode
    {
        // Syntactic category and written form
        private readonly SyntaxCat _synCat;
        private readonly string _writtenForm;
        public SyntaxCat Category { get { return _synCat; } }

        // Semantics
        private readonly SemValue _semantics;
        public SemValue Semantics { get { return _semantics; } }

        public TermNode(SyntaxCat synCat, string writtenForm, string lambdaString)
        {
            _synCat = synCat;
            _writtenForm = writtenForm;
            _semantics = new SemValue(lambdaString);
        }

        public string GetFlatString()
        {
            return _writtenForm;
        }

        public string GetXMLString()
        {
            return "<" + _synCat + " value=" + _semantics.Value 
                + ">" + _writtenForm + "</{_synCat}>";
        }
    }
}
