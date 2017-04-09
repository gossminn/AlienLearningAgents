using System;
using System.Linq;
using AlienDebug;

namespace LearningEngine
{
    internal class ChildAgent : LanguageAgent
    {
        // Random number generator
        private static readonly Random _random = new Random();

        // Current sentence
        private readonly string _current;

        // Queue with n last heard sentences
        private readonly SentenceMemory _memory;

        // Constructor
        private ChildAgent(KnowledgeSet knowledge, string sentence,
            SentenceMemory memory) : base(knowledge)
        {
            _memory = memory;
            _current = sentence;
        }

        public string Current
        {
            get { return _current; }
        }

        // Factory method: create empty instance
        public static ChildAgent Initialize()
        {
            return new ChildAgent(
                KnowledgeSet.Initialize(), "", SentenceMemory.Initialize());
        }

        // Learn from input
        public ChildAgent Learn(string input)
        {
            // Add input to memory
            var memory = _memory.Memorize(input);

            // Learn terminal terminalCategories
            var categories0 = _knowledge.RawCategories.ProcessInput(input);
            var categories1 = categories0.GeneralizeContexts();

            // Generate terminal nodes and rules
            var termNodes = categories1.GenerateTerminals();
            var termRules = termNodes.ExtractRules().Cast<ISyntaxRule>();

            // Learn constituents

            // Add TermRules
            var ruleSet0 = RuleSet.CreateEmpty().AddRules(termRules);

            // Add NonTermRules
            var categories2 = ruleSet0.ProcessInput(categories1, input);

            // Update knowledge set
            var knowledge = _knowledge
                .UpdateRawCategories(categories0)
                .UpdateGeneralizedCategories(categories1)
                .UpdateTerminals(termNodes)
                .UpdateRules(ruleSet0);

            // Write output files
            DebugHelpers.WriteCatNumbers(categories0.Count, categories1.Count);
            DebugHelpers.WriteXmlFile(_memory.ToXmlString() + GetXmlString());
            return new ChildAgent(knowledge, _current, memory);
        }

        // Evaluate feedback
        // Simplistic version: if parent is angry, remove sentence from memory
        public ChildAgent EvaluateFeedback(Feedback feedback)
        {
            if (feedback == Feedback.Happy)
                return new ChildAgent(_knowledge, _current, _memory);

            var memory = _memory.Forget(_current);
            return new ChildAgent(_knowledge, _current, memory);
        }

        // Produce sentence
        // Simplistic version: just produce random sentence from memory
        public ChildAgent SaySomething()
        {
            var n = _random.Next(_memory.Size);
            var sentence = n == 0
                ? _memory.Sentences.First()
                : _memory.Sentences.Take(n).Last();
            return new ChildAgent(_knowledge, sentence, _memory);
        }
    }
}