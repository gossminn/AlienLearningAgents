using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Code.LearningEngine.Knowledge.KnowledgeHelpers
{
    // Data type for storing a set of CategoryBigrams
    internal class CategoryBigramSet
    {
        // Single field: hashset of bigrams
        private readonly ImmutableHashSet<CategoryBigram> _bigrams;

        // Private constructor
        private CategoryBigramSet(ImmutableHashSet<CategoryBigram> bigrams)
        {
            _bigrams = bigrams;
        }

        // Getter: get hashset as IEnumerable
        public IEnumerable<CategoryBigram> Bigrams
        {
            get { return _bigrams.AsEnumerable(); }
        }

        // Factory method: create empty
        public static CategoryBigramSet CreateEmpty()
        {
            return new CategoryBigramSet(ImmutableHashSet<CategoryBigram>.Empty);
        }

        // Add bigram
        public CategoryBigramSet Add(CategoryBigram bigram)
        {
            var bigrams = _bigrams.Add(bigram);
            return new CategoryBigramSet(bigrams);
        }
    }
}