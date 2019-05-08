using System;

namespace X.UI.NodeGraph
{
    // this knows nothing about the rendering technology
    public interface INodeGraph
    {
        void InitializeNodeGraph();
        void DrawNodeGraph();
        void DrawNodeSlotLink(string key);
        Node UpdateNodePosition(string key, double positionX, double positionY);
        Node FindNode(string key);
        NodeLink FindNodeLink(string uniqueid);
        void AddNodeToGraph(Node node);
        void AddLinkToGraph(NodeLink nodeLink);
        void RemoveNodeLinkFromGraph(NodeLink nodeLink);
        void MarkNodeLinkForDeletion(Guid uniqueId);
        void ClearGraph();
    }
}
