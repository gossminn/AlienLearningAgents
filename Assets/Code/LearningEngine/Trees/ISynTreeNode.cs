namespace LearningEngine
{
    interface ITreeNode
    {
        string GetXMLString();
        string GetFlatString();
        SemValue Semantics { get; }
    }
}
