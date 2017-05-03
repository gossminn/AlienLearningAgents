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

            // Write output files
            DebugHelpers.WriteCatNumbers(
                _knowledge.Categories.RawTerminals.Count, 
                _knowledge.Categories.GeneralizedTerminals.Count);
            DebugHelpers.WriteXmlFile(_memory.ToXmlString() + GetXmlString());

        }

        public string Current
        {
            get { return _current; }
        }

        // Factory method: create empty instance
        public static ChildAgent Initialize()
        {
            return new ChildAgent(
                KnowledgeSet.CreateEmpty(), "", SentenceMemory.Initialize());
        }

        // Learn from input
        public ChildAgent Learn(string input)
        {
            // String is empty: skip this step TODO: find out why!
            if (input == "")
            {
                return this;
            }

            // Tokenize input
            var words = input.Split();

            // Add input to memory
            var memory = _memory.Memorize(input);

            // Learn and generalize categories
            var knowledge1 = _knowledge.LearnCategories(input);

            // Learn terminal nodes and rules
            var knowledge2 = knowledge1.LearnTerminals();

            // Generate (random) non-terminal rules
            var knowledge3 = knowledge2.LearnConstituents(words);

            // TODO: remove after testing
            DebugHelpers.WriteWordsetChanged(knowledge3.Categories.GeneralizedTerminals
                .HasChanged(_knowledge.Categories.GeneralizedTerminals));

            // New child agent object
            return new ChildAgent(knowledge3, _current, memory);
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
            // Memory is empty: don't do anything, return self
            if (!_memory.Sentences.Any())
            {
                return this;
            }

            var n = _random.Next(_memory.Size);
            var sentence = n == 0
                ? _memory.Sentences.First()
                : _memory.Sentences.Take(n).Last();
            return new ChildAgent(_knowledge, sentence, _memory);
        }
    }
}