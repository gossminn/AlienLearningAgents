namespace Code.LearningEngine.Knowledge.Categories
{
    // Wrapper class for category sets 
    internal class CategorySet
    {
        // Syntactic categories of terminals
        private readonly TerminalCategorySet _rawTerminals;
        private readonly TerminalCategorySet _generalizedTerminals;

        // Syntactic caterories of nonterminals
        private readonly NonTerminalCategorySet _rawNonTerminals;
        private readonly NonTerminalCategorySet _generalizedNonTerminals;

        // Constructor
        private CategorySet(TerminalCategorySet rawTerminals, TerminalCategorySet generalizedTerminals,
            NonTerminalCategorySet rawNonTerminals, NonTerminalCategorySet generalizedNonTerminals)
        {
            _rawTerminals = rawTerminals;
            _generalizedTerminals = generalizedTerminals;
            _rawNonTerminals = rawNonTerminals;
            _generalizedNonTerminals = generalizedNonTerminals;
        }

        // Getters
        public TerminalCategorySet RawTerminals
        {
            get { return _rawTerminals; }
        }

        public TerminalCategorySet GeneralizedTerminals
        {
            get { return _generalizedTerminals; }
        }

        public NonTerminalCategorySet RawNonTerminals
        {
            get { return _rawNonTerminals; }
        }

        public NonTerminalCategorySet GeneralizedNonTerminals
        {
            get { return _generalizedNonTerminals; }
        }

        // Factory method: create empty
        public static CategorySet CreateEmpty()
        {
            return new CategorySet(
                TerminalCategorySet.CreateEmpty(), 
                TerminalCategorySet.CreateEmpty(), 
                NonTerminalCategorySet.CreateEmpty(),
                NonTerminalCategorySet.CreateEmpty());
        }

        // Update sets
        public CategorySet UpdateRawTerminals(TerminalCategorySet rawTerminals)
        {
            return new CategorySet(
                rawTerminals,
                _generalizedTerminals,
                _rawNonTerminals,
                _generalizedNonTerminals);
        }

        public CategorySet UpdateGeneralizedTerminals(TerminalCategorySet generalizedTerminals)
        {
            return new CategorySet(
                _rawTerminals,
                generalizedTerminals,
                _rawNonTerminals,
                _generalizedNonTerminals);
        }

        public CategorySet UpdateRawNonTerminals(NonTerminalCategorySet rawNonTerminals)
        {
            return new CategorySet(
                _rawTerminals,
                _generalizedTerminals,
                rawNonTerminals,
                _generalizedNonTerminals);
        }

        public CategorySet UpdateGeneralizedNonTerminals(NonTerminalCategorySet generalizedNonTerminals)
        {
            return new CategorySet(
                _rawTerminals,
                _generalizedTerminals,
                _rawNonTerminals,
                generalizedNonTerminals);
        }
    }
}