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
            // Tokenize input
            var words = input.Split();

            // Add input to memory
            var memory = _memory.Memorize(input);

            // Learn and genereralize syntactic categories
            var categories0 = _knowledge.Categories;
            var rawCategories = categories0.RawTerminals.ExtractRawCategories(input);
            var generalizedCategories = rawCategories.GeneralizeContexts();
            var categories1 = categories0
                .UpdateRawTerminals(rawCategories)
                .UpdateGeneralizedTerminals(generalizedCategories);

            // Generate terminal nodes and rules
            var termNodes = generalizedCategories.GenerateTerminals();
            var termRules = termNodes.ExtractRules();

            // Add to rule set
            var ruleSet1 = RuleSet.CreateEmpty().AddRules(termRules);

            // Infer nonterminal rules and categories TODO: should not happen again every time Learn() is called
            var helper = ConstituentLearning.InferFromInput(ruleSet1, words);
            var categories2 = categories1.UpdateRawNonTerminals(helper.Categories);
            var ruleSet2 = ruleSet1.ClearNonterminals().AddRules(helper.Rules);
            
            // Update knowledge set
            var knowledge = _knowledge
                .UpdateCategories(categories2)
                .UpdateTerminals(termNodes)
                .UpdateRules(ruleSet2);

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