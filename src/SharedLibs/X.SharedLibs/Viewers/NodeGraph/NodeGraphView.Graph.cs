using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraph
    {
        IDictionary<string, Node> _nodes;
        List<NodeLink> _links;
        
        public void InitializeNodeGraph() {
            _nodes = new Dictionary<string, Node>();
            _links = new List<NodeLink>();
        }
        
        public void DrawNodes() {
            
            if (_isRendererInitialized) {
                //draw nodes
                foreach (var node in _nodes) {
                    RenderNode(node.Value);
                }
                
                //node-slot-links between the node-slots
                foreach (var link in _links)
                {
                    RenderNodeSlotLink(link);
                }                
            }
        }

        public Node UpdateNodePosition(string key, double positionX, double positionY) {
            var foundNode = _nodes[key];
            foundNode.Position.X = positionX;
            foundNode.Position.Y = positionY;
            _nodes[key] = foundNode; //force immutable element to be updated
            return foundNode;
        }

    }
}
