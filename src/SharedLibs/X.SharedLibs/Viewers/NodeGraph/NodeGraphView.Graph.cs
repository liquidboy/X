using System.Collections.Generic;
using Windows.UI;
using NodePosition = Windows.Foundation.Point;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraph
    {
        IDictionary<string, Node> _nodes;
        List<NodeLink> _links;
        
        public void InitializeNodeGraph() {
            _nodes = new Dictionary<string, Node>();
            _links = new List<NodeLink>();
            SetupExampleNodes();
        }

        private void SetupExampleNodes() {
            _nodes.Add("Node1", new Node("Node1", new NodePosition(100, 100), Colors.Red, 1, 1));
            _nodes.Add("Node2", new Node("Node2", new NodePosition(100, 300), Colors.Green, 1, 1));
            _nodes.Add("Node3", new Node("Node3", new NodePosition(400, 190), Colors.Yellow, 2, 2));
            _nodes.Add("Node4", new Node("Node4", new NodePosition(400, 0), Colors.Purple, 1, 1));
            _nodes.Add("Node5", new Node("Node5", new NodePosition(700, 100), Colors.Blue, 2, 1));
            _nodes.Add("Node6", new Node("Node6", new NodePosition(400, 400), Colors.Pink, 1, 1));

            _links.Add(new NodeLink("Node1", 0, "Node3", 0));
            _links.Add(new NodeLink("Node2", 0, "Node3", 1));
            _links.Add(new NodeLink("Node4", 0, "Node5", 0));
            _links.Add(new NodeLink("Node3", 0, "Node5", 0));
            _links.Add(new NodeLink("Node3", 1, "Node5", 0));
            _links.Add(new NodeLink("Node6", 0, "Node5", 1));

            DrawNodes();
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
