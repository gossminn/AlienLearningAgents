using System.Collections.Immutable;
using UnityEngine;

namespace LearningEngine
{
    class ParentAgent : LanguageAgent
    {
        // Random number generator
        private static readonly System.Random _random = new System.Random();

        // List of possible sentences
        private readonly ImmutableList<ITreeNode> _sentences;

        // Current sentence
        private readonly ITreeNode _currentSent;
        public string CurrentSentence { get { return _currentSent.GetFlatString(); } }

        // Constructor is private, public access through factory methods
        private ParentAgent(
            KnowledgeSet knowledge, ImmutableList<ITreeNode> sentences, 
            ITreeNode currentSent) : base(knowledge)
        {
            var rootNode = knowledge.RawCategories.Root;
            var rootRule = knowledge.Rules.FindWithLeftSide(rootNode);

            _sentences = sentences.IsEmpty
                ? rootRule.GenerateAll(knowledge.Rules).ToImmutableList()
                : sentences;
            _currentSent = currentSent;
        }

        // Factory method: create based on rules
        public static ParentAgent Create(
            CategorySet categories, RuleSet rules, TerminalSet terminals)
        {
            return new ParentAgent(
                KnowledgeSet.Initialize()
                    .UpdateRawCategories(categories)
                    .UpdateRules(rules)
                    .UpdateTerminals(terminals),
                ImmutableList<ITreeNode>.Empty, new EmptyNode());
        }

        // 'Say' something
        public ParentAgent SaySomething()
        {
            var num = _random.Next(_sentences.Count);
            var sentence = _sentences[num];
            //Debug.Log("Parent says: " + sentence.GetFlatString()); // TODO: remove print after testing
            return new ParentAgent(_knowledge, _sentences, sentence);
        }

        // Provide feedback on utterance of child
        public Feedback ProvideFeedback(string sentence)
        {
            if (_currentSent.GetFlatString() == sentence)
            {
                //Debug.Log("Parent is happy!"); // TODO: remove print
                return Feedback.Happy;
            }

            else
            {
                //Debug.Log("Parent is angry!"); // TODO: remove print
                return Feedback.Angry;
            }
        }
    }
}
