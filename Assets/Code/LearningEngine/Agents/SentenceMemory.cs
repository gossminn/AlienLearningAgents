﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Helper class for storing the sentences a ChildAgent has memorized
    class SentenceMemory
    {
        private const int _maxSize = 10;
        private readonly ImmutableQueue<string> _sentences;
        public IEnumerable<string> Sentences { get { return _sentences.AsEnumerable(); } }

        private readonly int _size;
        public int Size { get { return _size; } }

        private SentenceMemory(ImmutableQueue<string> memory, int size)
        {
            _sentences = memory;
            _size = size;
        }

        public static SentenceMemory Initialize()
        {
            return new SentenceMemory(ImmutableQueue<string>.Empty, 0);
        }

        public SentenceMemory Add(string sentence)
        {
            // Queue not filled yet: add sentence and increment size
            if (_size < _maxSize)
            {
                return new SentenceMemory(_sentences.Enqueue(sentence), _size + 1);
            }

            // Queue has maximum size: remove first element + add sentence
            return new SentenceMemory(_sentences.Dequeue().Enqueue(sentence), _size);
        }

        public SentenceMemory Remove(string sentence)
        {
            var removed = _sentences.Where(x => x != sentence);
            var removedQueue = removed.Aggregate(
                ImmutableQueue<string>.Empty,
                (acc, next) => acc.Enqueue(next));
            return new SentenceMemory(removedQueue, _size - 1);
        }

        // Get the nth element from the front of the queue
        public string GetNthElement(int n)
        {
            // 0th element: return element at the queue
            if (n == 0)
            {
                return _sentences.Peek();
            }

            // Otherwise: dequeue 10 times, then return element at the front
            return Enumerable.Range(0, n)
                .Aggregate(_sentences, (acc, next) => acc.Dequeue())
                .Peek();
        }

        public string ToXMLString()
        {
            var sentences = _sentences.Aggregate("",
                (acc, next) => acc + "<sentence>" + next + "</sentence>");
            return "<memory>" + sentences + "</memory>";
        }
    }
}