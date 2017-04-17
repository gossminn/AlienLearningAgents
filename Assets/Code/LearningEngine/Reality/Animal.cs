using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningEngine
{
    internal class Animal
    {
        // Random number generator
        private static readonly Random _random = new Random();

        // Constant: number of existing species
        public const int SpeciesCount = 4;

        // Constants: maximum latitude and longitude
        public const int MaxLongitude = Situation.EastWestBoundary * 2;
        public const int MaxLatitude = Situation.RiverLatitude * 2;

        // Species
        private readonly AnimalSpecies _species;

        // Location and orientation
        private readonly int _latitude;
        private readonly int _longitude;
        private readonly int _riverDistance;
        private readonly CardinalDirection _longHemisphere;
        private readonly CardinalDirection _latHemisphere;
        private readonly CardinalDirection _absoluteOrientation;
        private readonly RiverDirection _riverOrientation;

        private Animal(AnimalSpecies species, int latitude, int longitude,
            CardinalDirection absoluteOrientation)
        {
            // Store location/orientation information
            _latitude = latitude;
            _longitude = longitude;
            _absoluteOrientation = absoluteOrientation;

            // Get species
            _species = species;

            // Calculate absolute distance from river
            _riverDistance = Math.Abs(latitude - Situation.RiverLatitude);

            // Determine east/west hemisphere
            _longHemisphere = longitude > Situation.EastWestBoundary
                ? CardinalDirection.East
                : CardinalDirection.West;

            _latHemisphere = latitude > Situation.RiverLatitude
                ? CardinalDirection.South
                : CardinalDirection.North;

            // Determine orientation relative to river
            _riverOrientation = GetRiverOrientation(_absoluteOrientation, _latHemisphere);
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

        public AnimalSpecies Species
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

        public CardinalDirection AbsoluteOrientation
        {
            get { return _absoluteOrientation; }
        }

        public RiverDirection RiverOrientation
        {
            get { return _riverOrientation; }
        }

        private static RiverDirection GetRiverOrientation(CardinalDirection absoluteOrientation,
            CardinalDirection latHemisphere)
        {
            switch (absoluteOrientation)
            {
                case CardinalDirection.North:
                    return latHemisphere == CardinalDirection.North
                        ? RiverDirection.Away
                        : RiverDirection.Towards;
                case CardinalDirection.South:
                    return latHemisphere == CardinalDirection.South
                        ? RiverDirection.Away
                        : RiverDirection.Towards;
                case CardinalDirection.East:
                    return RiverDirection.Upstream;
                case CardinalDirection.West:
                    return RiverDirection.Downstream;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Animal GenerateRandom()
        {
            var entity = (AnimalSpecies) _random.Next(SpeciesCount);
            var latitude = _random.Next(MaxLatitude) + 1;
            var longtitude = _random.Next(MaxLongitude) + 1;
            var orientation = (CardinalDirection) _random.Next(4);

            return new Animal(entity, latitude, longtitude, orientation);
        }

        public static IEnumerable<Animal> GenerateAll()
        {
            // Define sets of values
            var species = Enum.GetValues(typeof(AnimalSpecies)).Cast<AnimalSpecies>();
            var longitudes = Enumerable.Range(1, MaxLongitude);
            var latitudes = Enumerable.Range(1, MaxLatitude);
            var orientations = Enum.GetValues(typeof(CardinalDirection)).Cast<CardinalDirection>();

            // All possible combinations
            return species.SelectMany(
                s => latitudes.SelectMany(
                    la => longitudes.SelectMany(
                        lo => orientations.Select(
                            o => new Animal(s, la, lo, o)))));
        }
    }
}