using System.Collections.Generic;

namespace LearningEngine
{
    internal interface ICategoryNGram
    {
        IEnumerable<CategoryLabel> Sequence { get; }
    }
}