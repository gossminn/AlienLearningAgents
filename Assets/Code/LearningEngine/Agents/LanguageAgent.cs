namespace LearningEngine
{
    internal abstract class LanguageAgent
    {
        // Knowledge representation
        protected readonly KnowledgeSet _knowledge;

        // Constructor
        protected LanguageAgent(KnowledgeSet knowledge)
        {
            _knowledge = knowledge;
        }

        public KnowledgeSet Knowledge
        {
            get { return _knowledge; }
        }

        // Print knowledge representation as XML
        public string GetXmlString()
        {
            return _knowledge.Categories.RawTerminals.GetXmlString() +
                   _knowledge.Categories.GeneralizedTerminals.GetXmlString() +
                   _knowledge.Rules.GetXmlString() +
                   _knowledge.Terminals.GetXmlString();
        }
    }
}