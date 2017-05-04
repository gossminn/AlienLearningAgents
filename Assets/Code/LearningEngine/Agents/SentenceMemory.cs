using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Code.LearningEngine.Agents
{
    // Helper class for storing the sentences a ChildAgent has memorized
    internal class SentenceMemory
    {
        // Maximum memory size
        private const int MaxSize = 10;

        // Sentences are stored in queue
        private readonly ImmutableQueue<string> _sentences;

        private SentenceMemory(ImmutableQueue<string> memory)
        {
            _sentences = memory;
        }

        public IEnumerable<string> Sentences
        {
            get { return _sentences.AsEnumerable(); }
        }

        // Property for getting the size of the queue
        public int Size
        {
            get { return _sentences.Count(); }
        }

        // Initialize the memory
        public static SentenceMemory Initialize()
        {
            return new SentenceMemory(ImmutableQueue<string>.Empty);
        }

        // Add a sentence to memory
        public SentenceMemory Memorize(string sentence)
        {
            // Queue not filled yet: add sentence and increment size
            if (Size < MaxSize)
                return new SentenceMemory(_sentences.Enqueue(sentence));

            // Queue has maximum size: remove first element + add sentence
            return new SentenceMemory(_sentences.Dequeue().Enqueue(sentence));
        }

        // Remove a sentence from memory
        public SentenceMemory Forget(string sentence)
        {
            var removed = _sentences.Where(x => x != sentence);
            var removedQueue = removed.Aggregate(
                ImmutableQueue<string>.Empty,
                (acc, next) => acc.Enqueue(next));
            return new SentenceMemory(removedQueue);
        }

        // Get the nth element from the front of the queue
        public string GetNthElement(int n)
        {
            // 0th element: return element at the queue
            if (n == 0)
                return _sentences.Peek();

            // Otherwise: dequeue 10 times, then return element at the front
            return Enumerable.Range(0, n)
                .Aggregate(_sentences, (acc, next) => acc.Dequeue())
                .Peek();
        }

        public string ToXmlString()
        {
            var sentences = _sentences.Aggregate("",
                (acc, next) => acc + "<sentence>" + next + "</sentence>");
            return "<memory>" + sentences + "</memory>";
        }
    }
}