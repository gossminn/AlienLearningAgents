using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Knowledge.Rules;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Syntax;
using Code.LearningEngine.Trees;
using UnityEngine;
using Random = System.Random;

namespace Code.LearningEngine.Agents
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

        // Current logicalmodel
        private readonly LogicalModel _model;

        // Constructor is private, public access through factory methods
        private ParentAgent(KnowledgeSet knowledge, CategoryLabel rootCat, ITreeNode currentSent,
            LogicalModel model) : base(knowledge)
        {
            _rootCat = rootCat;
            var rootRule = knowledge.Rules.FindWithLeftSide(_rootCat);

            _sentences = rootRule
                    .GenerateAll(knowledge.Rules, model)
                    .Where(x => x.GetTruthValue())
                    .ToImmutableList();

            _model = model;

            Debug.Log("Sentences: " + _sentences.Count);

            if (_sentences.Count == 0)
            {
                Debug.Log("No sentences!");
            }

            _currentSent = currentSent;
        }

        public string CurrentSentence
        {
            get { return _currentSent.GetFlatString(); }
        }

        // Factory method: create based on rules
        public static ParentAgent Create(TerminalCategorySet categories, RuleSet rules,
            VocabularySet terminals, CategoryLabel rootCat, LogicalModel model)
        {
            var categorySet = CategorySet.CreateEmpty().UpdateRawTerminals(categories);

            return new ParentAgent(
                KnowledgeSet.CreateEmpty()
                    .UpdateCategories(categorySet)
                    .UpdateRules(rules)
                    .UpdateTerminals(terminals),
                rootCat, EmptyNode.Create(), model);
        }

        // 'Say' something
        public ParentAgent SaySomething()
        {
            var num = _random.Next(_sentences.Count);
            var sentence = _sentences.Count > 0 ? _sentences[num] : EmptyNode.Create();
            return new ParentAgent(_knowledge, _rootCat, sentence, _model);
        }

        // Evaluate a sentence
        public bool EvaluateSentence(string s)
        {
            var words = s.Split().ToImmutableList();
            var rootRule = _knowledge.Rules.FindWithLeftSide(_rootCat);
            return rootRule.Parse(words, _knowledge.Rules, _model).Tree.GetTruthValue();
        }

        // Update model
        public ParentAgent UpdateModel(LogicalModel model)
        {
            return new ParentAgent(_knowledge, _rootCat, _currentSent, model);
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