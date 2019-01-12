namespace X.Viewer.NodeGraph
{
    public interface INodeSelector
    {
        void InitializeNodeSelector();
        void OnNodeTypeSelected(NodeType nodeType);
    }
}