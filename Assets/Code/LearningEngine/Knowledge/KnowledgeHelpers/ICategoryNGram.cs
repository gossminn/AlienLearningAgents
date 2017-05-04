using System.Collections.Generic;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Knowledge.KnowledgeHelpers
{
    internal interface ICategoryNGram
    {
        IEnumerable<CategoryLabel> Sequence { get; }
    }
}