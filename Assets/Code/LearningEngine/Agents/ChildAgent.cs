using System;
using System.Collections.Immutable;
using System.Linq;
using Code.Debugging;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.MeaningHypotheses;
using Code.LearningEngine.Learning;
using Code.LearningEngine.Semantics.Model;

namespace Code.LearningEngine.Agents
{
    internal class ChildAgent : LanguageAgent
    {
        // Random number generator
        private static readonly Random _random = new Random();

        // Current sentence
        private readonly string _current;

        // Queue with n last heard sentences
        private readonly SentenceMemory _memory;

        // Constructor
        private ChildAgent(KnowledgeSet knowledgeSet, string sentence,
            SentenceMemory memory) : base(knowledgeSet)
        {
            _memory = memory;
            _current = sentence;

            // Write output files
            DebugHelpers.WriteCatNumbers(
                KnowledgeSet.Categories.RawTerminals.Count,
                KnowledgeSet.Categories.GeneralizedTerminals.Count);
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
        public ChildAgent Learn(string input, LogicalModel model)
        {
            // String is empty: skip this step TODO: find out why!
            if (input == "")
            {
                return this;
            }

            // Tokenize input
            var words = input.Split().ToImmutableArray();

            // Add input to memory
            var memory = _memory.Memorize(input);

            // Learn and generalize categories
            var knowledge1 = KnowledgeSet.LearnCategories(input);

            // Learn terminal nodes and rules
            var knowledge2 = knowledge1.LearnTerminals();

            // Generate (random) non-terminal rules
            var knowledge3 = knowledge2.LearnConstituents(words);

            // Determine if category knowledge has changed
            var catsHaveChanged = knowledge3.Categories.GeneralizedTerminals
                .HasChanged(KnowledgeSet.Categories.GeneralizedTerminals);
            DebugHelpers.WriteWordsetChanged(catsHaveChanged); // TODO: remove after testing

            // If so: re-initialize hypothesis set
            var hypotheses = catsHaveChanged
                ? MeaningHypothesisSet.Initialize(knowledge3.Categories.GeneralizedTerminals)
                : knowledge3.Hypotheses;

            // Evaluate hypotheses based on model
            var knowledge4 = knowledge3.UpdateHypotheses(hypotheses).LearnWordSemantics(model, words);

            // New child agent object
            return new ChildAgent(knowledge4, _current, memory);
        }

        // Evaluate feedback
        // Simplistic version: if parent is angry, remove sentence from memory
        public ChildAgent EvaluateFeedback(Feedback feedback)
        {
            if (feedback == Feedback.Happy)
                return new ChildAgent(KnowledgeSet, _current, _memory);

            var memory = _memory.Forget(_current);
            return new ChildAgent(KnowledgeSet, _current, memory);
        }

        // Produce sentence
        // Simplistic version: just produce random sentence from memory
        public ChildAgent SaySomething()
        {
            // Memory is empty: don't do anything, return self
            if (!_memory.Sentences.Any())
            {
                return this;
            }

            var n = _random.Next(_memory.Size);
            var sentence = n == 0
                ? _memory.Sentences.First()
                : _memory.Sentences.Take(n).Last();
            return new ChildAgent(KnowledgeSet, sentence, _memory);
        }
    }
}