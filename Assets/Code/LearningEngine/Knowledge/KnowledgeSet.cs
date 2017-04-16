namespace LearningEngine
{

    // Data type for storing the linguistic knowledge of an agent
    internal class KnowledgeSet
    {
        // Syntactic categories
        private readonly CategorySet _categories;

        // Learning helpers
        private readonly KnowledgeHelperSet _helperSet;

        // Rewrite rules
        private readonly RuleSet _rules;

        // Terminal nodes (vocabulary)
        private readonly VocabularySet _terminals;

        // Constructor
        private KnowledgeSet(CategorySet categories, KnowledgeHelperSet helperSet, RuleSet rules, 
            VocabularySet terminals)
        {
            _categories = categories;
            _helperSet = helperSet;
            _rules = rules;
            _terminals = terminals;
        }

        public KnowledgeHelperSet HelperSet
        { get { return _helperSet; }}

        public CategorySet Categories
        {
            get { return _categories; }
        }

        public RuleSet Rules
        {
            get { return _rules; }
        }

        public VocabularySet Terminals
        {
            get { return _terminals; }
        }

        // Factory methods
        public static KnowledgeSet CreateEmpty()
        {
            return new KnowledgeSet(
                CategorySet.CreateEmpty(), 
                KnowledgeHelperSet.CreateEmpty(),
                RuleSet.CreateEmpty(),
                VocabularySet.CreateEmpty());
        }

        // Update syntactic categories
        public KnowledgeSet UpdateCategories(CategorySet categories)
        {
            return new KnowledgeSet(
                categories,
                _helperSet,
                _rules, 
                _terminals);
        }

        public KnowledgeSet UpdateHelper(KnowledgeHelperSet helperSet)
        {
            return new KnowledgeSet(
                _categories,
                helperSet,
                _rules,
                _terminals);
        }

        public KnowledgeSet UpdateRules(RuleSet rules)
        {
            return new KnowledgeSet(
                _categories,
                _helperSet,
                rules, 
                _terminals);
        }

        public KnowledgeSet UpdateTerminals(VocabularySet terminals)
        {
            return new KnowledgeSet(
                _categories,
                _helperSet,
                _rules, 
                terminals);
        }
    }
}