using System;
using System.Linq;
using AlienDebug;

namespace LearningEngine
{
    internal class ChildAgent : LanguageAgent
    {
        // Random number generator
        private static readonly Random Random = new Random();

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
            DebugHelpers.WriteXMLFile(_memory.ToXmlString() + GetXmlString());
            DebugHelpers.WriteCatNumbers(knowledge.RawCategories.Count,
                knowledge.GeneralizedCategories.Count);
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

            // Learn categories
            var rawCategories = _knowledge.RawCategories.ProcessInput(input);
            var generalizedCategories = rawCategories.Generalize();
            var knowledge = _knowledge
                .UpdateRawCategories(rawCategories)
                .UpdateGeneralizedCategories(generalizedCategories);

            //// Update syntax rules
            //var knowledge1 = NonTerminalLearning.UpdateNonTerms(
            //    knowledge0, memory0, input);

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
            var n = Random.Next(_memory.Size);
            var sentence = n == 0
                ? _memory.Sentences.First()
                : _memory.Sentences.Take(n).Last();
            return new ChildAgent(_knowledge, sentence, _memory);
        }
    }
}