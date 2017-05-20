using System.Collections.Immutable;
using System.Linq;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.Categories;
using Code.LearningEngine.Knowledge.Rules;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Syntax;
using Code.LearningEngine.Trees;
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

        // Constructor is private, public access through factory methods
        private ParentAgent(KnowledgeSet knowledgeSet, CategoryLabel rootCat,
            ITreeNode currentSent) : base(knowledgeSet)
        {
            _rootCat = rootCat;
            _currentSent = currentSent;
        }

        public string CurrentSentence
        {
            get { return _currentSent.GetFlatString(); }
        }

        // Factory method: create based on rules
        public static ParentAgent Initialize(TerminalCategorySet categories, RuleSet rules,
            VocabularySet terminals, CategoryLabel rootCat, LogicalModel model)
        {
            // Define knowledge set
            var categorySet = CategorySet.CreateEmpty().UpdateRawTerminals(categories);
            var knowledgeSet = KnowledgeSet.CreateEmpty()
                .UpdateCategories(categorySet)
                .UpdateRules(rules)
                .UpdateTerminals(terminals);

            return new ParentAgent(knowledgeSet, rootCat, EmptyNode.Create());
        }

        // Produce an utterance
        public ParentAgent SaySomething(LogicalModel model)
        {
            // Generate all true sentences
            var rootRule = KnowledgeSet.Rules.FindWithLeftSide(_rootCat);
            var sentences = rootRule.GenerateAll(KnowledgeSet.Rules, model)
                .Where(x => x.GetTruthValue())
                .ToImmutableList();

            // Select random sentence
            var num = _random.Next(sentences.Count);
            var sentence = sentences.Count > 0
                ? sentences[num]
                : EmptyNode.Create(); // Use dummy tree if no true sentence exist

            // New version of agent with new current sentence
            return new ParentAgent(KnowledgeSet, _rootCat, sentence);
        }

        // Determine if a sentence is true under the current model
        public bool EvaluateSentence(string s, LogicalModel model)
        {
            var words = s.Split().ToImmutableList();
            var rootRule = KnowledgeSet.Rules.FindWithLeftSide(_rootCat);
            return rootRule.Parse(words, KnowledgeSet.Rules, model).Tree.GetTruthValue();
        }

        // Provide feedback on utterance of child
        public Feedback ProvideFeedback(string sentence, LogicalModel model)
        {
            // Evaluate child's sentence; be 'happy' if it's true, else be 'angry'
            return EvaluateSentence(sentence, model) ? Feedback.Happy : Feedback.Angry;
        }
    }
}