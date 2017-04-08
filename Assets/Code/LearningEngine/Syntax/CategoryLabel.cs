namespace LearningEngine
{
    // Class for labelling syntactic categories
    internal class CategoryLabel
    {
        // Counter for identifiers
        private static int _counter = 1;

        // Empty category
        private static readonly CategoryLabel _emptyCat = new CategoryLabel(0, "", NodeType.NonTerminal);

        // Number for identifying categories
        private readonly int _identifier;

        // Optional: give the category a name
        private readonly string _name;

        private readonly NodeType _type;

        // Private constructor
        private CategoryLabel(int identifier, string name, NodeType type)
        {
            _identifier = identifier;
            _name = name;
            _type = type;
        }

        public static CategoryLabel EmptyCat
        {
            get { return _emptyCat; }
        }

        public int Identifier
        {
            get { return _identifier; }
        }

        public string Name
        {
            get { return _name; }
        }

        public NodeType Type
        {
            get { return _type; }
        }

        // Create label with only identifier
        public static CategoryLabel Create(NodeType type)
        {
            return new CategoryLabel(_counter++, "", type);
        }

        // Create label with identifier and name
        public static CategoryLabel Create(string name, NodeType type)
        {
            return new CategoryLabel(_counter++, name, type);
        }

        // Return label if present, else identifier number
        public override string ToString()
        {
            if (_name == "")
                return _identifier.ToString();
            return _name;
        }
    }
}