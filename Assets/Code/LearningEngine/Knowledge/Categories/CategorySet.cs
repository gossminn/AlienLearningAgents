namespace Code.LearningEngine.Knowledge.Categories
{
    // Wrapper class for category sets 
    internal class CategorySet
    {
        // Syntactic categories of terminals
        private readonly TerminalCategorySet _rawTerminals;
        private readonly TerminalCategorySet _generalizedTerminals;

        // Syntactic caterories of nonterminals
        private readonly NonTerminalCategorySet _nonTerminals;

        // Constructor
        private CategorySet(TerminalCategorySet rawTerminals, TerminalCategorySet generalizedTerminals,
            NonTerminalCategorySet nonTerminals)
        {
            _rawTerminals = rawTerminals;
            _generalizedTerminals = generalizedTerminals;
            _nonTerminals = nonTerminals;
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

        public NonTerminalCategorySet NonTerminals
        {
            get { return _nonTerminals; }
        }

        // Factory method: create empty
        public static CategorySet CreateEmpty()
        {
            return new CategorySet(TerminalCategorySet.CreateEmpty(), TerminalCategorySet.CreateEmpty(),
                NonTerminalCategorySet.CreateEmpty());
        }

        // Update sets
        public CategorySet UpdateRawTerminals(TerminalCategorySet rawTerminals)
        {
            return new CategorySet(rawTerminals, _generalizedTerminals, _nonTerminals);

        }

        public CategorySet UpdateGeneralizedTerminals(TerminalCategorySet generalizedTerminals)
        {
            return new CategorySet(_rawTerminals, generalizedTerminals, _nonTerminals);
        }

        public CategorySet UpdateNonTerminals(NonTerminalCategorySet generalizedNonTerminals)
        {
            return new CategorySet(_rawTerminals, _generalizedTerminals, generalizedNonTerminals);
        }
    }
}