namespace LearningEngine
{
    class SyntaxCat
    {
        // Counter for identifiers
        static private int _counter = 1;

        // Empty category
        static private readonly SyntaxCat _emptyCat = new SyntaxCat(0, "", CatType.NonTerminal);
        static public SyntaxCat EmptyCat { get { return _emptyCat; } }

        // Object attributes
        private readonly int _identifier;
        private readonly string _label;
        private readonly CatType _type;
        public int Identifier { get { return _identifier; } }
        public string Label { get { return _label; } }
        public CatType Type { get { return _type; } }

        // Private constructor
        private SyntaxCat(int identifier, string label, CatType type)
        {
            _identifier = identifier;
            _label = label;
            _type = type;
        }

        // Factory methods
        public static SyntaxCat Create(CatType type)
        {
            return new SyntaxCat(_counter++, "", type);
        }

        public static SyntaxCat Create(string label, CatType type)
        {
            return new SyntaxCat(_counter++, label, type);
        }

        // Return label if present, else identifier number
        public override string ToString()
        {
            if (_label == "")
            {
                return _identifier.ToString();
            }

            return _label;
        }
    }
}