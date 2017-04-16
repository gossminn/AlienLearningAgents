namespace LearningEngine
{
    internal interface ITreeNode
    {
        LambdaExpression Semantics { get; }
        CategoryLabel Category { get; }
        string GetXmlString();
        string GetFlatString();
    }
}