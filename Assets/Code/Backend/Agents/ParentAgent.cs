using System.Collections.Immutable;
using UnityEngine;

namespace LearningEngine
{
    class ParentAgent : LanguageAgent
    {
        // Random number generator
        private readonly System.Random _random = new System.Random();

        // List of possible sentences
        private readonly ImmutableList<ITreeNode> _sentences;

        // Current sentence
        private readonly ITreeNode _currentSent;
        public string CurrentSentence { get { return _currentSent.GetFlatString(); } }

        // Constructor is private, public access through factory methods
        private ParentAgent(
            CategorySet categories, RuleSet rules, 
            TerminalSet terminals, ITreeNode currentSent) 
            : base(categories, rules, terminals)
        {
            var rootNode = categories.Root;
            var rootRule = rules.FindWithLeftSide(rootNode);
            _sentences = rootRule.GenerateAll(rules).ToImmutableList();
            _currentSent = currentSent;
        }

        // Factory method: create based on rules
        public static ParentAgent Create(
            CategorySet categories, RuleSet rules, TerminalSet terminals)
        {
            return new ParentAgent(categories, rules, terminals, new EmptyNode());
        }

        // 'Say' something
        public ParentAgent SaySomething()
        {
            var num = _random.Next(_sentences.Count);
            var sentence = _sentences[num];
            Debug.Log("Parent says: " + sentence.GetFlatString()); // TODO: remove print after testing
            return new ParentAgent(_categories, _rules, _terminals, sentence);
        }

        // Provide feedback on utterance of child
        public Feedback ProvideFeedback(string sentence)
        {
            if (_currentSent.GetFlatString() == sentence)
            {
                Debug.Log("Parent is happy!"); // TODO: remove print
                return Feedback.Happy;
            }

            else
            {
                Debug.Log("Parent is angry!"); // TODO: remove print
                return Feedback.Angry;
            }
        }
    }
}
