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

        public void DrawNodeGraph() {

            if (_isRendererInitialized) {
                foreach (var node in _nodes) {
                    RenderNode(node.Value);
                }
                foreach (var link in _links)
                {
                    RenderNodeSlotLink(link);
                }
            }
        }

        public void DrawNodeSlotLink(string key) {
            foreach (var link in _links)
            {
                //need to update links ?????  I want to use Windows Composition eventually
                if (link.InputNodeKey.Equals(key) || link.OutputNodeKey.Equals(key))
                    RenderNodeSlotLink(link);
            }
        }

        public Node UpdateNodePosition(string key, double positionX, double positionY) {
            var foundNode = _nodes[key];
            foundNode.Position.X = positionX;
            foundNode.Position.Y = positionY;
            _nodes[key] = foundNode; //force immutable element to be updated
            return foundNode;
        }

        public Node FindNode(string key) {
            return _nodes[key];
        }
    }
}
