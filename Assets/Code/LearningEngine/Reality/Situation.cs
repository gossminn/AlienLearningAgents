namespace Code.LearningEngine.Reality
{

    internal class Situation
    {
        // Constant: location of the river (= north/south boundary)
        public const int RiverLatitude = 4;

        // Constant: east/west boundary
        public const int EastWestBoundary = 4;

        // Entities
        private readonly Animal _animal1;
        private readonly Animal _animal2;

        // Private constructor
        private Situation(Animal animal1, Animal animal2)
        {
            _animal1 = animal1;
            _animal2 = animal2;
        }

        public Animal Animal1
        {
            get { return _animal1; }
        }

        public Animal Animal2
        {
            get { return _animal2; }
        }

        private static bool IsValid(Situation s)
        {
            // Animals with same species must have unique orientations
            if (s.Animal1.Species == s.Animal2.Species &&
                s.Animal1.RiverOrientation == s.Animal2.RiverOrientation)
            {
                return false;
            }

            // Positions must be unique
            if (s.Animal1.Latitude == s.Animal2.Latitude &&
                s.Animal1.Longitude == s.Animal2.Longitude)
            {
                return false;
            }

            // No problems found: situation is valid
            return true;
        }

        // Generate random situation
        public static Situation Generate()
        {
            // Generate random situation
            var situation = new Situation(Animal.GenerateRandom(), Animal.GenerateRandom());

            // Check for validity
            if (IsValid(situation))
            {
                return situation;
            }

            // Invalid situation: try again
            return Generate();
        }
    }
}