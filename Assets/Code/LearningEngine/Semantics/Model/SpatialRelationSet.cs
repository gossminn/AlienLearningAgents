using System;
using System.Collections.Immutable;
using System.Linq;

namespace LearningEngine
{
    internal class SpatialRelationSet
    {
        // Helper type for holding an ordered pair of entities
        private struct EntityPair
        {
            public readonly Entity A;
            public readonly Entity B;

            public EntityPair(Entity a, Entity b)
            {
                A = a;
                B = b;
            }
        }

        // "X is located further in direction ... than B"
        private readonly ImmutableHashSet<EntityPair> _towards;
        private readonly ImmutableHashSet<EntityPair> _away;
        private readonly ImmutableHashSet<EntityPair> _upstream;
        private readonly ImmutableHashSet<EntityPair> _downstream;

        // "X is located further in direction ... than B and on the same river bank"
        private readonly ImmutableHashSet<EntityPair> _sameTowards;
        private readonly ImmutableHashSet<EntityPair> _sameAway;
        private readonly ImmutableHashSet<EntityPair> _sameUpstream;
        private readonly ImmutableHashSet<EntityPair> _sameDownstream;

        // Private constructor
        private SpatialRelationSet(ImmutableHashSet<EntityPair> towards,
            ImmutableHashSet<EntityPair> away, ImmutableHashSet<EntityPair> upstream,
            ImmutableHashSet<EntityPair> downstream, ImmutableHashSet<EntityPair> sameTowards,
            ImmutableHashSet<EntityPair> sameAway, ImmutableHashSet<EntityPair> sameUpstream,
            ImmutableHashSet<EntityPair> sameDownstream)
        {
            _towards = towards;
            _away = away;
            _upstream = upstream;
            _downstream = downstream;
            _sameTowards = sameTowards;
            _sameAway = sameAway;
            _sameUpstream = sameUpstream;
            _sameDownstream = sameDownstream;
        }

        public static SpatialRelationSet CreateEmpty()
        {
            var emptySet = ImmutableHashSet<EntityPair>.Empty;
            return new SpatialRelationSet(emptySet, emptySet, emptySet, emptySet,
                emptySet, emptySet, emptySet, emptySet);
        }

        public bool Contains(RiverDirection direction, bool sameSide, Entity a, Entity b)
        {
            Func<EntityPair, bool> containsPair = pair => pair.A == a && pair.B == b;

            switch (direction)
            {
                case RiverDirection.Towards:
                    if (sameSide)
                    {
                        return _sameTowards.Any(containsPair);
                    }
                    return _towards.Any(containsPair);
                case RiverDirection.Away:
                    if (sameSide)
                    {
                        return _sameAway.Any(containsPair);
                    }
                    return _away.Any(containsPair);
                case RiverDirection.Upstream:
                    if (sameSide)
                    {
                        return _sameUpstream.Any(containsPair);
                    }
                    return _upstream.Any(containsPair);
                case RiverDirection.Downstream:
                    if (sameSide)
                    {
                        return _sameDownstream.Any(containsPair);
                    }
                    return _downstream.Any(containsPair);

                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public SpatialRelationSet Add(RiverDirection direction, bool sameSide, Entity a, Entity b)
        {
            var pair = new EntityPair(a, b);

            switch (direction)
            {
                case RiverDirection.Towards:
                    if (sameSide)
                    {
                        return new SpatialRelationSet(
                            _towards.Add(pair), _away, _upstream, _downstream,
                            _sameTowards.Add(pair), _sameAway, _sameUpstream, _sameDownstream);
                    }
                    return new SpatialRelationSet(
                        _towards.Add(pair), _away, _upstream, _downstream,
                        _sameTowards, _sameAway, _sameUpstream, _sameDownstream);
                case RiverDirection.Away:
                    if (sameSide)
                    {
                        return new SpatialRelationSet(
                            _towards, _away.Add(pair), _upstream, _downstream,
                            _sameTowards, _sameAway.Add(pair), _sameUpstream, _sameDownstream);
                    }
                    return new SpatialRelationSet(
                        _towards, _away.Add(pair), _upstream, _downstream,
                        _sameTowards, _sameAway, _sameUpstream, _sameDownstream);

                case RiverDirection.Upstream:
                    if (sameSide)
                    {
                        return new SpatialRelationSet(
                            _towards, _away, _upstream.Add(pair), _downstream,
                            _sameTowards, _sameAway, _sameUpstream.Add(pair), _sameDownstream);
                    }
                    return new SpatialRelationSet(
                        _towards, _away, _upstream.Add(pair), _downstream,
                        _sameTowards, _sameAway, _sameUpstream, _sameDownstream);
                case RiverDirection.Downstream:
                    if (sameSide)
                    {
                        return new SpatialRelationSet(
                            _towards, _away, _upstream, _downstream.Add(pair),
                            _sameTowards, _sameAway, _sameUpstream, _sameDownstream.Add(pair));
                    }
                    return new SpatialRelationSet(
                        _towards, _away, _upstream, _downstream.Add(pair),
                        _sameTowards, _sameAway, _sameUpstream, _sameDownstream);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }
    }
}