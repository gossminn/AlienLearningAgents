using System;

namespace LearningEngine
{
    internal class EntityModel
    {
        // Random number generator
        private static readonly Random Random = new Random();

        // Constant: number of existing species
        public const int SpeciesCount = 4;

        // Species
        private readonly EntitySpecies _species;

        // Location and orientation
        private readonly int _latitude;
        private readonly int _longitude;
        private readonly int _riverDistance;
        private readonly CardinalDirection _longHemisphere;
        private readonly CardinalDirection _latHemisphere;
        private readonly CardinalDirection _orientation;

        private EntityModel(EntitySpecies species, int latitude, int longitude, CardinalDirection orientation)
        {
            // Store location/orientation information
            _latitude = latitude;
            _longitude = longitude;
            _orientation = orientation;

            // Get species
            _species = species;

            // Calculate absolute distance from river
            _riverDistance = Math.Abs(latitude - WorldModel.RiverLatitude);

            // Determine east/west hemisphere
            _latHemisphere = longitude > WorldModel.EastWestBoundary
                ? CardinalDirection.East
                : CardinalDirection.West;

            _longHemisphere = latitude > WorldModel.RiverLatitude
                ? CardinalDirection.South
                : CardinalDirection.North;
        }

        public int Longitude
        {
            get { return _longitude; }
        }

        public int Latitude
        {
            get { return _latitude; }
        }

        public int RiverDistance
        {
            get { return _riverDistance; }
        }

        public EntitySpecies Species
        {
            get { return _species; }
        }

        public CardinalDirection LongHemisphere
        {
            get { return _longHemisphere; }
        }

        public CardinalDirection LatHemisphere
        {
            get { return _latHemisphere; }
        }

        public CardinalDirection Orientation
        {
            get { return _orientation; }
        }

        public static EntityModel Generate()
        {
            var entity = (EntitySpecies) Random.Next(SpeciesCount);
            var latitude = Random.Next(WorldModel.RiverLatitude * 2 + 1);
            var longtitude = Random.Next(WorldModel.EastWestBoundary * 2 + 1);
            var orientation = (CardinalDirection) Random.Next(4);

            return new EntityModel(entity, latitude, longtitude, orientation);
        }
    }
}