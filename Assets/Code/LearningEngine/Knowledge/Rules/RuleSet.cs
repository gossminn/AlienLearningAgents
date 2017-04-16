using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Datatype for representing a set of syntax rules
    internal class RuleSet
    {
        private readonly ImmutableHashSet<TermRule> _terminalRules;
        private readonly ImmutableHashSet<NonTermRule> _nonTerminalRules;

        // Private constructor
        private RuleSet(ImmutableHashSet<TermRule> terminalRules, ImmutableHashSet<NonTermRule> nonTerminalRules)
        {
            _terminalRules = terminalRules;
            _nonTerminalRules = nonTerminalRules;
        }

        public IEnumerable<TermRule> TerminalRules
        {
            get { return _terminalRules.AsEnumerable(); }
        }

        // Factory method: create an empty RuleSet
        public static RuleSet CreateEmpty()
        {
            return new RuleSet(ImmutableHashSet<TermRule>.Empty, ImmutableHashSet<NonTermRule>.Empty);
        }

        // Factory method: add a rule to an existing RuleSet
        public RuleSet AddRule(TermRule rule)
        {
            // If rule with same left side present: replace with new rule
            Func<TermRule, bool> sameLeft = r => r.Left == rule.Left;

            if (_terminalRules.Any(sameLeft))
            {
                var newRules = _terminalRules.Remove(_terminalRules.Single(sameLeft)).Add(rule);
                return new RuleSet(newRules, _nonTerminalRules);
            }

            // Otherwise: just add the rule
            return new RuleSet(_terminalRules.Add(rule), _nonTerminalRules);
        }

        public RuleSet AddRule(NonTermRule rule)
        {
            // If rule with same left side present: replace with new rule
            Func<NonTermRule, bool> sameLeft = r => r.Left == rule.Left;

            if (_nonTerminalRules.Any(sameLeft))
            {
                var newRules = _nonTerminalRules.Remove(_nonTerminalRules.Single(sameLeft)).Add(rule);
                return new RuleSet(_terminalRules, newRules);
            }

            // Otherwise: just add the rule
            return new RuleSet(_terminalRules, _nonTerminalRules.Add(rule));
        }

        // Factory method: add multiple rules to an existing RuleSet
        public RuleSet AddRules(IEnumerable<TermRule> rules)
        {
            return rules.Aggregate(this, (x, y) => x.AddRule(y));
        }

        public RuleSet AddRules(IEnumerable<NonTermRule> rules)
        {
            return rules.Aggregate(this, (x, y) => x.AddRule(y));
        }

        public RuleSet ClearNonterminals()
        {
            return new RuleSet(_terminalRules, ImmutableHashSet<NonTermRule>.Empty);
        }

        // Method for getting rules from the set based on the 'left-handed' SyntaxCat
        public ISyntaxRule FindWithLeftSide(CategoryLabel cat)
        {
            return _terminalRules.Cast<ISyntaxRule>()
                .Union(_nonTerminalRules.Cast<ISyntaxRule>())
                .Single(x => x.Left == cat);
        }

        // Get XML representation of the RuleSet
        public string GetXmlString()
        {
            var terminalEntries = string.Join("", _terminalRules.Select(x => x.GetXmlString()).ToArray());
            var nonTerminalEntries = string.Join("", _nonTerminalRules.Select(x => x.GetXmlString()).ToArray());
            return "<syntaxRules>" + terminalEntries + nonTerminalEntries + "</syntaxRules>";
        }
    }
}