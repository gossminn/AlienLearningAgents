using System.Collections.Immutable;

namespace LearningEngine
{
    // Helper class for temporarily storing a NonTerminalCategorySet and a RuleSet
    internal class NonTerminalHelper
    {
        private readonly NonTerminalCategorySet _categories;
        private readonly ImmutableHashSet<NonTermRule> _rules;

        private NonTerminalHelper(NonTerminalCategorySet categories, ImmutableHashSet<NonTermRule> rules)
        {
            _categories = categories;
            _rules = rules;
        }

        public NonTerminalCategorySet Categories
        {
            get { return _categories; }
        }

        public ImmutableHashSet<NonTermRule> Rules
        {
            get { return _rules; }
        }

        public static NonTerminalHelper Create(NonTerminalCategorySet categories,
            ImmutableHashSet<NonTermRule> rules)
        {
            return new NonTerminalHelper(categories, rules);
        }
    }
}