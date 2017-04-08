namespace LearningEngine
{
    internal interface ITreeNode
    {
        SemValue Semantics { get; }
        string GetXmlString();
        string GetFlatString();
    }
}