using Code.LearningEngine.Knowledge;

namespace Code.LearningEngine.Agents
{
    internal abstract class LanguageAgent
    {
        // Knowledge representation
        protected readonly KnowledgeSet KnowledgeSet;

        // Constructor
        protected LanguageAgent(KnowledgeSet knowledgeSet)
        {
            KnowledgeSet = knowledgeSet;
        }

        public KnowledgeSet Knowledge
        {
            get { return KnowledgeSet; }
        }

        // Print knowledge representation as XML
        public string GetXmlString()
        {
            return KnowledgeSet.Categories.RawTerminals.GetXmlString() +
                   KnowledgeSet.Categories.GeneralizedTerminals.GetXmlString() +
                   KnowledgeSet.Rules.GetXmlString() +
                   KnowledgeSet.Terminals.GetXmlString() +
                   KnowledgeSet.Hypotheses.ToXmlString();
        }
    }
}