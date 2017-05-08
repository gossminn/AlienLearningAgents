using System.Collections.Immutable;
using System.Linq;
using Code.Debugging;
using Code.LearningEngine.Knowledge;
using Code.LearningEngine.Knowledge.MeaningHypotheses;
using Code.LearningEngine.Learning;
using Code.LearningEngine.Semantics.Model;
using Random = System.Random;

namespace Code.LearningEngine.Agents
{
    internal class ChildAgent : LanguageAgent
    {
        // Random numbers generator
        private static readonly Random _random = new Random();

        // Current sentence
        private readonly string _current;

        // Last sentence by father
        private readonly ImmutableArray<string> _parentSent;

        // Queue with n last heard sentences
        private readonly SentenceMemory _memory;

        // Current guess
        private readonly MeaningHypothesisSet _guess;

        // Constructor
        private ChildAgent(KnowledgeSet knowledgeSet, string sentence, SentenceMemory memory,
        MeaningHypothesisSet guess, ImmutableArray<string> parentSent) : base(knowledgeSet)
        {
            _memory = memory;
            _current = sentence;
            _parentSent = parentSent;
            _guess = guess;

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
                KnowledgeSet.CreateEmpty(), "", SentenceMemory.Initialize(), MeaningHypothesisSet.Empty(),
                ImmutableArray<string>.Empty);
        }

        // Process input
        public ChildAgent ProcessInput(string input, LogicalModel model)
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

            // Infer knowledge from input
            var knowledge = InferKnowledge(model, words);

            return knowledge.Hypotheses.IsFixed()
                ? new ChildAgent(knowledge, "", memory, _guess, words).ProduceGuess(model)
                : new ChildAgent(knowledge, _current, memory, _guess, words);
        }

        private ChildAgent ProduceGuess(LogicalModel model, int iteration = 0)
        {
            // Avoid infinite looping
            if (iteration > 100)
            {
                return this;
            }

            // Produce guess
            var guess = KnowledgeSet.Hypotheses.Guess();

            // Make terminal nodes and rules based on guess
            var knowledge1 = KnowledgeSet.LearnTerminals(guess);

            // Infer rules based on guess
            var knowledge2 = knowledge1.GuessStructure(guess, _parentSent, model);

            // Try to parse current sentence with guessed rules
            var parseSent = knowledge2.Rules.Root.Parse(_parentSent.ToImmutableList(), knowledge2.Rules, model);

            // If this fails, guess again
            if (!parseSent.Tree.GetTruthValue())
            {
                // Treat this in same way as negative feedback from parent
                var hypotheses = KnowledgeSet.Hypotheses.ProcessFeedback(Feedback.Angry, _parentSent, guess);

                return new ChildAgent(KnowledgeSet.UpdateHypotheses(hypotheses), _current, _memory, guess, _parentSent)
                    .ProduceGuess(model, iteration + 1);
            }

            // Generate all possible sentences and select a random one
            var sentences = knowledge2.Rules.Root
                .GenerateAll(knowledge2.Rules, model)
                .Where(x => x.GetTruthValue())
                .ToArray();
            var randomNum = _random.Next(sentences.Length);
            var randomSent = sentences[randomNum].GetFlatString();

            return new ChildAgent(knowledge2, randomSent, _memory, guess, _parentSent);
        }

        private KnowledgeSet InferKnowledge(LogicalModel model, ImmutableArray<string> words)
        {
            // Learn and generalize categories
            var knowledge1 = KnowledgeSet.LearnCategories(words);

            // Determine if category knowledge has changed
            var catsHaveChanged = knowledge1.Categories.GeneralizedTerminals
                .HasChanged(KnowledgeSet.Categories.GeneralizedTerminals);
            DebugHelpers.WriteWordsetChanged(catsHaveChanged); // TODO: remove after testing

            // If so: re-initialize hypothesis set
            var hypotheses = catsHaveChanged
                ? MeaningHypothesisSet.Initialize(knowledge1.Categories.GeneralizedTerminals)
                : knowledge1.Hypotheses;

            // Evaluate hypotheses based on model
            var knowledge2 = knowledge1.UpdateHypotheses(hypotheses).LearnWordSemantics(model, words);
            return knowledge2;
        }

        // Evaluate feedback
        public ChildAgent EvaluateFeedback(Feedback feedback)
        {
            var hypotheses = KnowledgeSet.Hypotheses.ProcessFeedback(feedback, _current.Split().ToImmutableArray(),
                _guess);

            return new ChildAgent(KnowledgeSet.UpdateHypotheses(hypotheses), _current, _memory, _guess, _parentSent);
        }
    }
}