using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Syntax;
using Code.LearningEngine.Trees;

namespace Code.LearningEngine.Knowledge.Rules
{
    // Data type for representing a set of TermNodes
    internal class VocabularySet
    {
        // TermNodes are stored in an immutable collection type
        private readonly ImmutableHashSet<TermNode> _terminals;

        // Constructor is private, public interface through factory methods
        private VocabularySet(ImmutableHashSet<TermNode> terminals)
        {
            _terminals = terminals;
        }

        public IEnumerable<TermNode> Collection
        {
            get { return _terminals.AsEnumerable(); }
        }

        // Factory method: create an empty TerminalSet
        public static VocabularySet CreateEmpty()
        {
            return new VocabularySet(ImmutableHashSet<TermNode>.Empty);
        }

        // Factory method: add a TermNode to an existing TerminalSet
        public VocabularySet AddTerminal(TermNode node)
        {
            return new VocabularySet(_terminals.Add(node));
        }

        // Factory method: add multiple TermNodes
        public VocabularySet AddTerminals(IEnumerable<TermNode> nodes)
        {
            return nodes.Aggregate(this, (x, y) => x.AddTerminal(y));
        }

        // Factory method: remove a TermNode from an existing TerminalSet
        public VocabularySet RemoveTerminal(TermNode node)
        {
            return new VocabularySet(_terminals.Remove(node));
        }

        // Convert current TerminalSet to collection of TermRules
        public IEnumerable<TermRule> ExtractRules()
        {
            // Get a collection of SyntaxCats
            var categories = _terminals
                .Select(x => x.Category)
                .Distinct();

            // For each category, yield a TermRule containing the TermNodes of that category
            return categories
                .Select(x => _terminals
                    .Where(y => y.Category == x)
                    .Aggregate(
                        TermRule.CreateEmpty(x),
                        (acc, next) => acc.AddToRight(next))
                );
        }

        // Get XML representation of the TerminalSet
        public string GetXmlString()
        {
            var entries = string.Join("", _terminals.Select(x => x.GetXmlString()).ToArray());
            return "<terminalNodes>" + entries + "</terminalNodes>";
        }
    }
}