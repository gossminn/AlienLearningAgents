using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Knowledge.Rules;

namespace Code.LearningEngine.Learning.LearningHelpers
{
    // Helper class for temporarily storing a NonTerminalCategorySet and a RuleSet
    internal class NonTerminalHelper
    {
        private readonly NonTerminalCategorySet _categories;
        private readonly RuleSet _rules;

        private NonTerminalHelper(NonTerminalCategorySet categories, RuleSet rules)
        {
            _categories = categories;
            _rules = rules;
        }

        public NonTerminalCategorySet Categories
        {
            get { return _categories; }
        }

        public RuleSet Rules
        {
            get { return _rules; }
        }

        public static NonTerminalHelper Create(NonTerminalCategorySet categories, RuleSet rules)
        {
            return new NonTerminalHelper(categories, rules);
        }
    }
}