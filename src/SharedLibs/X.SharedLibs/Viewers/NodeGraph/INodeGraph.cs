namespace X.Viewer.NodeGraph
{
    public interface INodeGraph
    {
        void InitializeNodeGraph();
        void DrawNodes();
        Node UpdateNodePosition(string key, double positionX, double positionY);
    }
}
