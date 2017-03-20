﻿using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Datatype for representing a set of syntax rules
    class RuleSet
    {
        // Rules are stored in an immutable collection type
        private readonly ImmutableHashSet<ISyntaxRule> _rules;

        // Constructor is private; public interface through factory methods
        private RuleSet(ImmutableHashSet<ISyntaxRule> rules)
        {
            _rules = rules;
        }

        // Factory method: create an empty RuleSet
        public static RuleSet CreateEmpty()
        {
            return new RuleSet(ImmutableHashSet<ISyntaxRule>.Empty);
        }

        // Factory method: add a rule to an existing RuleSet
        public RuleSet AddRule(ISyntaxRule rule)
        {
            // If rule with same left side present: replace with new rule
            Func<ISyntaxRule, bool> sameLeft = r => r.Left == rule.Left;

            if (_rules.Any(sameLeft))
            {
                var newRules = _rules.Remove(_rules.Single(sameLeft)).Add(rule);
                return new RuleSet(newRules);
            }

            // Otherwise: just add the rule
            return new RuleSet(_rules.Add(rule));
        }

        // Factory method: remove a rule from an existing RuleSet
        public RuleSet RemoveRule(ISyntaxRule rule)
        {
            return new RuleSet(_rules.Remove(rule));
        }

        // Method for getting rules from the set based on the 'left-handed' SyntaxCat
        public ISyntaxRule FindWithLeftSide(SyntaxCat cat)
        {
            return _rules.Single(x => x.Left == cat);
        }

        // Get XML representation of the RuleSet
        public string GetXMLString()
        {
            var entries = String.Join("", _rules.Select(x => x.GetXMLString()).ToArray());
            return "<syntaxRules>" + entries + "</syntaxRules>";
        }
    }
}