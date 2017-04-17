using System;
using System.Collections.Immutable;

namespace LearningEngine
{
    internal class OrientationSet
    {
        private readonly ImmutableHashSet<Entity> _towards;
        private readonly ImmutableHashSet<Entity> _away;
        private readonly ImmutableHashSet<Entity> _upstream;
        private readonly ImmutableHashSet<Entity> _downstream;

        private OrientationSet(ImmutableHashSet<Entity> towards, ImmutableHashSet<Entity> away,
            ImmutableHashSet<Entity> upstream, ImmutableHashSet<Entity> downstream)
        {
            _towards = towards;
            _away = away;
            _upstream = upstream;
            _downstream = downstream;
        }

        public static OrientationSet CreateEmtpy()
        {
            var emptySet = ImmutableHashSet<Entity>.Empty;
            return new OrientationSet(emptySet, emptySet, emptySet, emptySet);
        }

        public bool Contains(RiverDirection direction, Entity entity)
        {
            switch (direction)
            {
                case RiverDirection.Towards:
                    return _towards.Contains(entity);
                case RiverDirection.Away:
                    return _away.Contains(entity);
                case RiverDirection.Upstream:
                    return _upstream.Contains(entity);
                case RiverDirection.Downstream:
                    return _downstream.Contains(entity);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public OrientationSet Add(RiverDirection direction, Entity entity)
        {
            switch (direction)
            {
                case RiverDirection.Towards:
                    return new OrientationSet(_towards.Add(entity), _away, _upstream, _downstream);
                case RiverDirection.Away:
                    return new OrientationSet(_towards, _away.Add(entity), _upstream, _downstream);
                case RiverDirection.Upstream:
                    return new OrientationSet(_towards, _away, _upstream.Add(entity), _downstream);
                case RiverDirection.Downstream:
                    return new OrientationSet(_towards, _away, _upstream, _downstream.Add(entity));
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }
    }
}