using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace LearningEngine
{
    static class ImmutableCollectionExtensions
    {
        public static IImmutableSet<T> AddRange(this IImmutableSet<T> set,IEnumerable<T> range)
        {
            return range.Aggregate(set, (acc, next) => acc.Add(next));
        }
    }
}
