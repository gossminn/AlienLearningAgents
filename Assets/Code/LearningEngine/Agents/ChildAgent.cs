using UnityEngine;

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
        private readonly string _currentSent;
        public string CurrentSentence { get { return _currentSent; } }
            
        // Constructor
        private ChildAgent(
            KnowledgeSet knowledge, string sentence, SentenceMemory memory) 
            : base(knowledge)
        {
            _memory = memory;
            _currentSent = sentence;

            Debug.Log(_memory.ToXMLString() + GetXMLString());
        }

        // Factory method: create empty instance
        public static ChildAgent Initialize()
        {
            return new ChildAgent(
                KnowledgeSet.Initialize(), "", SentenceMemory.Initialize());
        }

        // Learn from input
        // Simplistic version: just store sentence in memory, update categories
        public ChildAgent Learn(string input)
        {
            // Add input to memory
            var memory = _memory.Add(input);

            // Update terminals and categories
            var knowledge0 = TerminalLearning.UpdateTerminals(
                _knowledge, memory, input);

            // Update syntax rules
            var knowledge1 = NonTerminalLearning.UpdateNonTerms(
                knowledge0, memory, input);

            return new ChildAgent(knowledge1, _currentSent, memory);
        }

        // Evaluate feedback
        // Simplistic version: if parent is angry, remove sentence from memory
        public ChildAgent EvaluateFeedback(Feedback feedback)
        {
            if (feedback == Feedback.Happy)
            {
                return new ChildAgent(_knowledge, _currentSent, _memory);
            }

            var memory = _memory.Remove(_currentSent);
            return new ChildAgent(_knowledge, _currentSent, memory);
        }

        // Produce sentence
        // Simplistic version: just produce random sentence from memory
        public ChildAgent SaySomething()
        {
            var num = _random.Next(_memory.Size);
            var sentence = _memory.GetNthElement(num);
            //Debug.Log("Child says:" + sentence); // TODO: remove print after testing
            return new ChildAgent(_knowledge, sentence, _memory);
        }        
    }
}
