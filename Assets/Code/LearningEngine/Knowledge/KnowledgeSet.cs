namespace LearningEngine
{
    // Data type for storing the linguistic knowledge of an agent
    internal class KnowledgeSet
    {
        // Generalized syntactic terminalCategories
        private readonly CategorySet _generalizedCategories;

        // 'Raw' (ungeneralized) syntactic terminalCategories
        private readonly CategorySet _rawCategories;

        // Rewrite rules
        private readonly RuleSet _rules;

        // Terminal nodes (vocabulary)
        private readonly TerminalSet _terminals;

        // Constructor
        private KnowledgeSet(CategorySet rawCategories, CategorySet generalizedCategories,
            RuleSet rules, TerminalSet terminals)
        {
            _rawCategories = rawCategories;
            _generalizedCategories = generalizedCategories;
            _rules = rules;
            _terminals = terminals;
        }

        public CategorySet RawCategories
        {
            get { return _rawCategories; }
        }

        public CategorySet GeneralizedCategories
        {
            get { return _generalizedCategories; }
        }

        public RuleSet Rules
        {
            get { return _rules; }
        }

        public TerminalSet Terminals
        {
            get { return _terminals; }
        }

        // Factory methods
        public static KnowledgeSet Initialize()
        {
            return new KnowledgeSet(
                CategorySet.CreateEmpty(),
                CategorySet.CreateEmpty(),
                RuleSet.CreateEmpty(),
                TerminalSet.CreateEmpty());
        }

        // Replace current CategorySet by a new one
        public KnowledgeSet UpdateRawCategories(CategorySet rawCategories)
        {
            return new KnowledgeSet(rawCategories, _generalizedCategories, _rules, _terminals);
        }

        public KnowledgeSet UpdateGeneralizedCategories(CategorySet generalizedCategories)
        {
            return new KnowledgeSet(_rawCategories, generalizedCategories, _rules, _terminals);
        }

        public KnowledgeSet UpdateRules(RuleSet rules)
        {
            return new KnowledgeSet(_rawCategories, _generalizedCategories, rules, _terminals);
        }

        public KnowledgeSet UpdateTerminals(TerminalSet terminals)
        {
            return new KnowledgeSet(_rawCategories, _generalizedCategories, _rules, terminals);
        }
    }
}