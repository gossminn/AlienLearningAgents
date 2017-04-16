using System;
using System.Collections.Immutable;

namespace LearningEngine
{
    internal class ParentAgent : LanguageAgent
    {
        // Random number generator
        private static readonly Random _random = new Random();

        // Root category
        private readonly CategoryLabel _rootCat;

        // Current sentence
        private readonly ITreeNode _currentSent;

        // List of possible sentences
        private readonly ImmutableList<ITreeNode> _sentences;

        // Constructor is private, public access through factory methods
        private ParentAgent(
            KnowledgeSet knowledge, CategoryLabel rootCat, ImmutableList<ITreeNode> sentences,
            ITreeNode currentSent) : base(knowledge)
        {
            _rootCat = rootCat;
            var rootRule = knowledge.Rules.FindWithLeftSide(_rootCat);

            _sentences = sentences.IsEmpty
                ? rootRule.GenerateAll(knowledge.Rules).ToImmutableList()
                : sentences;
            _currentSent = currentSent;
        }

        public string CurrentSentence
        {
            get { return _currentSent.GetFlatString(); }
        }

        // Factory method: create based on rules
        public static ParentAgent Create(
            TerminalCategorySet categories, RuleSet rules, VocabularySet terminals, CategoryLabel rootCat)
        {
            var categorySet = CategorySet.CreateEmpty().UpdateRawTerminals(categories);

            return new ParentAgent(
                KnowledgeSet.CreateEmpty()
                    .UpdateCategories(categorySet)
                    .UpdateRules(rules)
                    .UpdateTerminals(terminals),
                rootCat,
                ImmutableList<ITreeNode>.Empty, EmptyNode.Create());
        }

        // 'Say' something
        public ParentAgent SaySomething()
        {
            var num = _random.Next(_sentences.Count);
            var sentence = _sentences[num];
            //Debug.Log("Parent says: " + sentence.GetFlatString()); // TODO: remove print after testing
            return new ParentAgent(_knowledge, _rootCat, _sentences, sentence);
        }

        // Provide feedback on utterance of child
        public Feedback ProvideFeedback(string sentence)
        {
            if (_currentSent.GetFlatString() == sentence)
                return Feedback.Happy;

            return Feedback.Angry;
        }
    }
}