using System;
using System.Collections.Immutable;

namespace LearningEngine
{
    internal class SpeciesSet
    {
        private readonly ImmutableHashSet<Entity> _rabbits;
        private readonly ImmutableHashSet<Entity> _ducks;
        private readonly ImmutableHashSet<Entity> _chairs;
        private readonly ImmutableHashSet<Entity> _frogs;

        private SpeciesSet(ImmutableHashSet<Entity> rabbits, ImmutableHashSet<Entity> ducks, ImmutableHashSet<Entity> chairs, ImmutableHashSet<Entity> frogs)
        {
            _rabbits = rabbits;
            _ducks = ducks;
            _chairs = chairs;
            _frogs = frogs;
        }

        public static SpeciesSet CreateEmpty()
        {
            var emtpySet = ImmutableHashSet<Entity>.Empty;
            return new SpeciesSet(emtpySet, emtpySet, emtpySet, emtpySet);
        }

        public bool Contains(AnimalSpecies species, Entity entity)
        {
            switch (species)
            {
                case AnimalSpecies.Rabbit:
                    return _rabbits.Contains(entity);
                case AnimalSpecies.Duck:
                    return _ducks.Contains(entity);
                case AnimalSpecies.Chair:
                    return _chairs.Contains(entity);
                case AnimalSpecies.Frog:
                    return _frogs.Contains(entity);
                default:
                    throw new ArgumentOutOfRangeException("species", species, null);
            }
        }

        public SpeciesSet Add(AnimalSpecies species, Entity entity)
        {
            switch (species)
            {
                case AnimalSpecies.Rabbit:
                    return new SpeciesSet(_rabbits.Add(entity), _ducks, _chairs, _frogs);
                case AnimalSpecies.Duck:
                    return new SpeciesSet(_rabbits, _ducks.Add(entity), _chairs, _frogs);
                case AnimalSpecies.Chair:
                    return new SpeciesSet(_rabbits, _ducks, _chairs.Add(entity), _frogs);
                case AnimalSpecies.Frog:
                    return new SpeciesSet(_rabbits, _ducks, _chairs, _frogs.Add(entity));
                default:
                    throw new ArgumentOutOfRangeException("species", species, null);
            }
        }
    }
}