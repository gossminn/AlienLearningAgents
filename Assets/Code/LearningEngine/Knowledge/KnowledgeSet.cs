namespace LearningEngine
{
    // Data type for storing the linguistic knowledge of an agent
    class KnowledgeSet
    {
        // Fields
        private readonly CategorySet _categories;
        private readonly RuleSet _rules;
        private readonly TerminalSet _terminals;

        // Getters
        public CategorySet Categories { get { return _categories; } }
        public RuleSet Rules { get { return _rules; } }
        public TerminalSet Terminals { get { return _terminals; } }

        // Constructor
        private KnowledgeSet(
            CategorySet categories, RuleSet rules, TerminalSet terminals)
        {
            _categories = categories;
            _rules = rules;
            _terminals = terminals;
        }

        // Factory methods
        public static KnowledgeSet Initialize()
        {
            return new KnowledgeSet(
                CategorySet.CreateEmpty(),
                RuleSet.CreateEmpty(),
                TerminalSet.CreateEmpty());
        }

        public KnowledgeSet UpdateCategories(CategorySet categories)
        {
            return new KnowledgeSet(categories, _rules, _terminals);
        }

        public KnowledgeSet UpdateRules(RuleSet rules)
        {
            return new KnowledgeSet(_categories, rules, _terminals);
        }

        public KnowledgeSet UpdateTerminals(TerminalSet terminals)
        {
            return new KnowledgeSet(_categories, _rules, terminals);
        }
    }
}
