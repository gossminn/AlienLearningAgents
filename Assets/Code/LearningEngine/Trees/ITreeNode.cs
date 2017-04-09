namespace LearningEngine
{
    internal interface ITreeNode
    {
        SemValue Semantics { get; }
        CategoryLabel Category { get; }
        string GetXmlString();
        string GetFlatString();
    }
}