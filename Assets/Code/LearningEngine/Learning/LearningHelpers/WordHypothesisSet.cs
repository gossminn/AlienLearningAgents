using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    // Defines the type of meaning an atomic meaning can encode

    // Wrapper class for storing a set of word hypotheses
    internal class WordHypothesisSet
    {
        private readonly ImmutableHashSet<WordHypothesis<TypeEtValue>> _species;
        private readonly ImmutableHashSet<WordHypothesis<TypeEtEValue>> _directions;
        private readonly ImmutableHashSet<WordHypothesis<TypeEEtValue>> _relations;

        private WordHypothesisSet(ImmutableHashSet<WordHypothesis<TypeEtValue>> species,
            ImmutableHashSet<WordHypothesis<TypeEtEValue>> directions,
            ImmutableHashSet<WordHypothesis<TypeEEtValue>> relations)
        {
            _species = species;
            _directions = directions;
            _relations = relations;
        }

        public static WordHypothesisSet CreateEmpty()
        {
            return new WordHypothesisSet(
                ImmutableHashSet<WordHypothesis<TypeEtValue>>.Empty,
                ImmutableHashSet<WordHypothesis<TypeEtEValue>>.Empty,
                ImmutableHashSet<WordHypothesis<TypeEEtValue>>.Empty);
        }

        public IWordHypothesis GetSpeciesHypothesis(AtomTypes atomType, string word)
        {
            Func<IWordHypothesis, bool> hasWord = x => x.Word == word;

            switch (atomType)
            {
                case AtomTypes.Species:
                    return _species.Cast<IWordHypothesis>().Single(hasWord);
                case AtomTypes.Direction:
                    return _directions.Cast<IWordHypothesis>().Single(hasWord);
                case AtomTypes.Relation:
                    return _relations.Cast<IWordHypothesis>().Single(hasWord);
                default:
                    throw new ArgumentOutOfRangeException("atomType", atomType, null);
            }
        }

        public WordHypothesisSet AddSpeciesHypothesis(WordHypothesis<TypeEtValue> hypothesis)
        {
            return new WordHypothesisSet(_species.Add(hypothesis), _directions, _relations);
        }

        public WordHypothesisSet RemoveSpeciesHypothesis(WordHypothesis<TypeEtValue> hypothesis)
        {
            return new WordHypothesisSet(_species.Remove(hypothesis), _directions, _relations);
        }
    }
}