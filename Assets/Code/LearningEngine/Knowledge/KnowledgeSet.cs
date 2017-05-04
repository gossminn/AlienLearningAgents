using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Knowledge.KnowledgeHelpers;
using Code.LearningEngine.Knowledge.Rules;

namespace Code.LearningEngine.Knowledge
{
    // Data type for storing the linguistic knowledge of an agent
    internal class KnowledgeSet
    {
        // Syntactic categories
        private readonly CategorySet _categories;

        // Rewrite rules
        private readonly RuleSet _rules;

        // Terminal nodes (vocabulary)
        private readonly VocabularySet _terminals;

        // Hypotheses about work mea

        // Constructor
        private KnowledgeSet(CategorySet categories, RuleSet rules, VocabularySet terminals)
        {
            _categories = categories;
            _rules = rules;
            _terminals = terminals;
        }

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
                RuleSet.CreateEmpty(),
                VocabularySet.CreateEmpty());
        }

        // Update syntactic categories
        public KnowledgeSet UpdateCategories(CategorySet categories)
        {
            return new KnowledgeSet(
                categories,
                _rules, 
                _terminals);
        }

        public KnowledgeSet UpdateHelper(KnowledgeHelperSet helperSet)
        {
            return new KnowledgeSet(
                _categories,
                _rules,
                _terminals);
        }

        public KnowledgeSet UpdateRules(RuleSet rules)
        {
            return new KnowledgeSet(
                _categories,
                rules, 
                _terminals);
        }

        public KnowledgeSet UpdateTerminals(VocabularySet terminals)
        {
            return new KnowledgeSet(
                _categories,
                _rules, 
                terminals);
        }
    }
}