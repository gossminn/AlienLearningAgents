using System;
using UnityEngine;

namespace LearningEngine
{
    class ChildAgent : LanguageAgent
    {
        // Random number generator
        System.Random random = new System.Random();

        // Queue with n last heard sentences
        private const int _memoryMax = 10;
        private readonly SentenceMemory _memory;
            
        // Constructor
        private ChildAgent(
            CategorySet categories, RuleSet rules, TerminalSet terminals, 
            SentenceMemory memory) : base(categories, rules, terminals)
        {
            _memory = memory;
        }

        // Factory method: create empty instance
        public static ChildAgent Initialize()
        {
            return new ChildAgent(
                CategorySet.CreateEmpty(),
                RuleSet.CreateEmpty(),
                TerminalSet.CreateEmpty(),
                SentenceMemory.Initialize()    
            );
        }

        // Learn from input
        // Simplistic version: just store sentence in memory
        public ChildAgent Learn(string input)
        {
            var memory = _memory.Add(input);
            return new ChildAgent(_categories, _rules, _terminals, memory);
        }

        // Evaluate feedback
        // Simplistic version: if parent is angry, remove sentence from memory
        public ChildAgent EvaluateFeedback(Feedback feedback, string sentence)
        {
            if (feedback == Feedback.Happy)
            {
                return new ChildAgent(_categories, _rules, _terminals, _memory);
            }

            var memory = _memory.Remove(sentence);
            return new ChildAgent(_categories, _rules, _terminals, memory);
        }

        // Produce sentence
        // Simplistic version: just produce random sentence from memory
        public string SaySomething()
        {
            var num = random.Next(_memory.Size - 1);
            var sentence = _memory.GetNthElement(num);
            Debug.Log("Child says:" + sentence); // TODO: remove print after testing
            return sentence;
        }        
    }
}
