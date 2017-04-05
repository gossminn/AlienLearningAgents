using UnityEngine;
using System.Linq;
using AlienDebug;

namespace LearningEngine
{
    class ChildAgent : LanguageAgent
    {
        // Random number generator
        private static readonly System.Random _random = new System.Random();

        // Queue with n last heard sentences
        private const int _memoryMax = 10;
        private readonly SentenceMemory _memory;

        // Current sentence
        private readonly string _current;
        public string CurrentSentence { get { return _current; } }
            
        // Constructor
        private ChildAgent( KnowledgeSet knowledge, string sentence, 
            SentenceMemory memory) : base(knowledge)
        {
            _memory = memory;
            _current = sentence;
            DebugHelpers.WriteToFile(_memory.ToXMLString() + GetXMLString());
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
            var memory1 = _memory.Memorize(input);

            // Learn categories
            var categories1 = _knowledge.Categories.ProcessInput(input);
            var knowledge1 = _knowledge.UpdateCategories(categories1);

            //// Update syntax rules
            //var knowledge1 = NonTerminalLearning.UpdateNonTerms(
            //    knowledge0, memory0, input);

            return new ChildAgent(knowledge1, _current, memory1);
        }

        // Evaluate feedback
        // Simplistic version: if parent is angry, remove sentence from memory
        public ChildAgent EvaluateFeedback(Feedback feedback)
        {
            if (feedback == Feedback.Happy)
            {
                return new ChildAgent(_knowledge, _current, _memory);
            }

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
