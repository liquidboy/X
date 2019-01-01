using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    public class NodeNodeLinkModel
    {
        public Node Node { get; set; }
        public List<NodeLink> InputNodeLinks { get; set; }
        public List<NodeLink> OutputNodeLinks { get; set; }

        public NodeNodeLinkModel()
        {
            InputNodeLinks = new List<NodeLink>();
            OutputNodeLinks = new List<NodeLink>();
        }
    }
}
