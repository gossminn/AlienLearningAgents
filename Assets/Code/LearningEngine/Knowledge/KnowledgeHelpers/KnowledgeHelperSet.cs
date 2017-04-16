namespace LearningEngine
{
    // Wrapper for data structures that store intermediate states needed for learning
    internal class KnowledgeHelperSet
    {
        // Attested sequences of syntactic categories
        private readonly CategoryBigramSet _bigrams;

        // Constructor
        private KnowledgeHelperSet(CategoryBigramSet bigrams)
        {
            _bigrams = bigrams;
        }

        // Properties
        public CategoryBigramSet Bigrams
        {
            get { return _bigrams; }
        }
      
        // Factory methods
        public static KnowledgeHelperSet CreateEmpty()
        {
            return new KnowledgeHelperSet(CategoryBigramSet.CreateEmpty());
        }

        public KnowledgeHelperSet UpdateBigrams(CategoryBigramSet bigrams)
        {
            return new KnowledgeHelperSet(bigrams);
        }
    }
}