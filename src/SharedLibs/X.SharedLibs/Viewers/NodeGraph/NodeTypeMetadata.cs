using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public class NodeTypeMetadata
    {
        public NodeType NodeType { get; set; }

        public NodeTypeMetadata(NodeType nodeType) => NodeType = nodeType;

        public string FriendlyName => parseFriendlyName();
        public string FriendlyType => parseFriendlyType();

        private string parseFriendlyName() {
            if (NodeType.ToString().EndsWith("Effect")) { return NodeType.ToString().Replace("Effect", ""); }
            else if (NodeType.ToString().EndsWith("Value")) { return NodeType.ToString().Replace("Value", ""); }
            else return NodeType.ToString();
        }

        private string parseFriendlyType()
        {
            if (NodeType.ToString().EndsWith("Effect")) { return "(effect)"; }
            else if (NodeType.ToString().EndsWith("Value")) { return "(value)"; }
            else return "";
        }
    }
}
