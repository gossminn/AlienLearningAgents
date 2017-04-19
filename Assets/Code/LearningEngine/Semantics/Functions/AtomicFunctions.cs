using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningEngine
{
    // Functions for atomic semantic types (i.e., the types of terminal nodes)
    internal static class AtomicFunctions
    {
        // Sets of atomic functions applying to current
        private static readonly IEnumerable<Func<LogicalModel, ISemanticValue>> _species;
        private static readonly IEnumerable<Func<LogicalModel, ISemanticValue>> _directions;
        private static readonly IEnumerable<Func<LogicalModel, ISemanticValue>> _spatialRelations;

        static AtomicFunctions()
        {
            _species = MakeSpecies();
            _directions = MakeDirections();
            _spatialRelations = MakeSpatialRelations();
        }

        public static IEnumerable<Func<LogicalModel, ISemanticValue>> Species
        {
            get { return _species.AsEnumerable(); }
        }

        public static IEnumerable<Func<LogicalModel, ISemanticValue>> Directions
        {
            get { return _directions; }
        }

        public static IEnumerable<Func<LogicalModel, ISemanticValue>> SpatialRelations
        {
            get { return _spatialRelations; }
        }

        private static IEnumerable<Func<LogicalModel, ISemanticValue>> MakeSpecies()
        {
            // Define set of species
            var species = Enum.GetValues(typeof(AnimalSpecies)).Cast<AnimalSpecies>();

            // Given a species, make a function from models to <e,t> functions
            Func<AnimalSpecies, Func<LogicalModel, ISemanticValue>> speciesFunction =
                // Start with a species
                s =>
                    // Given a model, return an <e,t>
                    m => (ISemanticValue) TypeEtValue.Create(
                        // Given an <e>, return true iff it is of species f
                        e => TypeTValue.Create(m.Species.Contains(s, e.Value)));

            // Make a function for each species
            return species.Select(speciesFunction);
        }

        private static IEnumerable<Func<LogicalModel, ISemanticValue>> MakeDirections()
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
            Func<RiverDirection, Func<LogicalModel, ISemanticValue>> directionFunction =
                // Start with a direction
                d =>
                    // Make a function that takes in a model and returns an <<e,t>,e>
                    m => (ISemanticValue) TypeEtEValue.Create(
                        // Take in an <e,t> and filter out entities satisfying that <e,t>
                        f => TypeEValue.Create(filterEntities(m)(f)
                            .DefaultIfEmpty(Entity.Nothing).SingleOrDefault(
                            // Finally, take the single entity with direction d
                            e => m.Orientations.Contains(d, e))));

            // Return collection of directionFunctions
            return directions.Select(directionFunction);
        }

        private static IEnumerable<Func<LogicalModel, ISemanticValue>> MakeSpatialRelations()
        {
            // Define combinations of orientations and booleans (same side/not same side)
            var orientations = Enum.GetValues(typeof(RiverDirection)).Cast<RiverDirection>();
            var relations = orientations.SelectMany(
                x => new[] {true, false}
                    .Select(y => new DirectionAndBool(x, y)));

            // Given a relation, make a function from models to <e,<e,t>> functions
            Func<DirectionAndBool, Func<LogicalModel, ISemanticValue>> relationFunction =
                // Start with a relation
                r =>
                    // Take in a model and return an <e,<e,t>> function
                    m => (ISemanticValue) TypeEEtValue.Create(
                        // Take in an e and return an <e,t> function
                        e1 => TypeEtValue.Create(
                            // Take in another e and return a t
                            e2 => TypeTValue.Create(m.SpatialRelations.Contains(
                                r.Orientation, r.SameSide, e1.Value, e2.Value))
                            ));

            return relations.Select(relationFunction);
        }

        // Type for temporarily storing a RiverOrientation and a boolean
        private struct DirectionAndBool
        {
            public readonly RiverDirection Orientation;
            public readonly bool SameSide;
            public DirectionAndBool(RiverDirection orientation, bool sameSide)
            {
                Orientation = orientation;
                SameSide = sameSide;
            }
        }

    }
}