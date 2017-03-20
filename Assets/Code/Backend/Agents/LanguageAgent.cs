namespace LearningEngine
{
    abstract class LanguageAgent
    {
        // Knowledge representation
        protected readonly CategorySet _categories;
        protected readonly RuleSet _rules;
        protected readonly TerminalSet _terminals;
        public CategorySet Categories { get { return _categories; } }
        public RuleSet Rules { get { return _rules; } }
        public TerminalSet Terminals { get { return _terminals; } }

        // Constructor
        protected LanguageAgent(CategorySet categories, RuleSet rules, TerminalSet terminals)
        {
            _categories = categories;
            _rules = rules;
            _terminals = terminals;
        }

        // Print knowledge representation as XML
        public string GetXMLString()
        {
            return _categories.GetXMLString() + _rules.GetXMLString() + _terminals.GetXMLString();
        }
    }
}
