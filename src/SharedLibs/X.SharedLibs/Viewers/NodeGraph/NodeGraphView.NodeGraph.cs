using System;
using System.Collections.Generic;
using System.Linq;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraph
    {
        IDictionary<string, Node> _nodes;
        List<NodeLink> _links;
        List<Guid> _linksToDelete;

        public void InitializeNodeGraph() {
            _nodes = new Dictionary<string, Node>();
            _links = new List<NodeLink>();
            _linksToDelete = new List<Guid>();
        }

        public void DrawNodeGraph() {

            if (_isRendererInitialized) {

                foreach (var link in _links)
                {
                    RenderNodeSlotLink(link);
                }
                foreach (var node in _nodes)
                {
                    var foundLinks = _links.Where(x => x.InputNodeKey == node.Key || x.OutputNodeKey == node.Key).ToList();
                    RenderNode(node.Value, foundLinks);
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
            foundNode.PositionX = positionX;
            foundNode.PositionY = positionY;
            _nodes[key] = foundNode; //force immutable element to be updated
            return foundNode;
        }

        public Node FindNode(string key) {
            return _nodes[key];
        }

        public void AddNodeToGraph(Node node) {
            _nodes.Add(node.Key, node);
        }

        public void AddLinkToGraph(NodeLink nodeLink) {
            _links.Add(nodeLink);
        }

        public void ClearGraph() {
            _nodes.Clear();
            _links.Clear();
            _linksToDelete.Clear();
        }

        public void RemoveNodeLinkFromGraph(NodeLink nodeLink)
        {
            _links.Remove(nodeLink);
        }

        public NodeLink FindNodeLink(string uniqueid)
        {
            return _links.Where(link => link.UniqueName == uniqueid).FirstOrDefault();
        }

        public void MarkNodeLinkForDeletion(Guid uniqueId)
        {
            _linksToDelete.Add(uniqueId);
        }
    }
}
