using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningEngine
{
    // Functions for atomic semantic types (i.e., the types of terminal nodes)
    internal static class AtomicFunctions
    {
        // Sets of atomic functions applying to current
        private static readonly IEnumerable<Func<LogicalModel, TypeEtValue>> _species;
        private static readonly IEnumerable<Func<LogicalModel, TypeEtEValue>> _directions;
        private static readonly IEnumerable<Func<LogicalModel, TypeEEtValue>> _spatialRelations;

        static AtomicFunctions()
        {
            _species = MakeSpecies();
            _directions = MakeDirections();
            _spatialRelations = MakeSpatialRelations();
        }

        public static IEnumerable<Func<LogicalModel, TypeEtValue>> Species
        {
            get { return _species.AsEnumerable(); }
        }

        public static IEnumerable<Func<LogicalModel, TypeEtEValue>> Directions
        {
            get { return _directions; }
        }

        public static IEnumerable<Func<LogicalModel, TypeEEtValue>> SpatialRelations
        {
            get { return _spatialRelations; }
        }

        private static IEnumerable<Func<LogicalModel, TypeEtValue>> MakeSpecies()
        {
            // Define set of species
            var species = Enum.GetValues(typeof(AnimalSpecies)).Cast<AnimalSpecies>();

            // Given a species, make a function from models to <e,t> functions
            Func<AnimalSpecies, Func<LogicalModel, TypeEtValue>> speciesFunction =
                // Start with a species
                s =>
                    // Given a model, return an <e,t>
                    m => TypeEtValue.Create(
                        // Given an <e>, return true iff it is of species f
                        e => TypeTValue.Create(m.Species.Contains(s, e.Value)));

            // Make a function for each species
            return species.Select(speciesFunction);
        }

        private static IEnumerable<Func<LogicalModel, TypeEtEValue>> MakeDirections()
        {
            // Helper function: convert a TypeEtValue to an <Entity, bool> predicate
            Func<TypeEtValue, Func<Entity, bool>> asPredicate =
                f => e => f.Value(TypeEValue.Create(e)).Value;

            // Define set of directions
            var directions = Enum.GetValues(typeof(RiverDirection)).Cast<RiverDirection>();

            // Define set of entities
            var entities = new[] {Entity.Entity1, Entity.Entity2};

            // Given a model, filter out the entities for which a predicate is true
            Func<LogicalModel, Func<TypeEtValue, IEnumerable<Entity>>> filterEntities =
                m => f => entities.Where(asPredicate(f));

            // Given a direction, make a function from models to <<e,t>,e> functions
            Func<RiverDirection, Func<LogicalModel, TypeEtEValue>> directionFunction =
                // Start with a direction
                d =>
                    // Make a function that takes in a model and returns an <<e,t>,e>
                    m => TypeEtEValue.Create(
                        // Take in an <e,t> and filter out entities satisfying that <e,t>
                        f => TypeEValue.Create(filterEntities(m)(f).Single(
                            // Finally, take the single entity with direction d
                            e => m.Orientations.Contains(d, e))));

            // Return collection of directionFunctions
            return directions.Select(directionFunction);
        }

        private static IEnumerable<Func<LogicalModel, TypeEEtValue>> MakeSpatialRelations()
        {
            throw new NotImplementedException();
        }


    }
}