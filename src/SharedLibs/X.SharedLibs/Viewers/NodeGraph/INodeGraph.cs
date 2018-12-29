namespace X.Viewer.NodeGraph
{
    // this knows nothing about the rendering technology
    public interface INodeGraph
    {
        void InitializeNodeGraph();
        void DrawNodeGraph();
        void DrawNodeSlotLink(string key);
        Node UpdateNodePosition(string key, double positionX, double positionY);
        Node FindNode(string key);
    }
}
