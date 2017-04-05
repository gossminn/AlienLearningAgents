namespace LearningEngine
{
    abstract class LanguageAgent
    {
        // Knowledge representation
        protected readonly KnowledgeSet _knowledge;
        public KnowledgeSet Knowledge { get { return _knowledge; } }

        // Constructor
        protected LanguageAgent(KnowledgeSet knowledge)
        {
            _knowledge = knowledge;
        }

        // Print knowledge representation as XML
        public string GetXMLString()
        {
            return _knowledge.RawCategories.GetXMLString() +
                _knowledge.GeneralizedCategories.GetXMLString() + 
                _knowledge.Rules.GetXMLString() + 
                _knowledge.Terminals.GetXMLString();
        }
    }
}
