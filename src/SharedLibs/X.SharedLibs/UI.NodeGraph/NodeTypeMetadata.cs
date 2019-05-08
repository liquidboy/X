namespace X.UI.NodeGraph
{
    public class NodeTypeMetadata
    {
        public NodeType NodeType { get; set; }

        public NodeTypeMetadata(NodeType nodeType) => NodeType = nodeType;

        public NodeTypeMetadata(NodeType nodeType, string name, string type){ NodeType = nodeType; _name = name; _type = type; }

        private string _name;
        private string _type;

        public string FriendlyName => parseFriendlyName();
        public string FriendlyType => parseFriendlyType();

        private string parseFriendlyName() {
            if (!string.IsNullOrEmpty(_name)) return _name;
            if (NodeType.ToString().EndsWith("Effect")) { return NodeType.ToString().Replace("Effect", ""); }
            else if (NodeType.ToString().EndsWith("Value")) { return NodeType.ToString().Replace("Value", ""); }
            else return NodeType.ToString();
        }

        private string parseFriendlyType()
        {
            if (!string.IsNullOrEmpty(_type)) return _type;
            if (NodeType.ToString().EndsWith("Effect")) { return "(effect)"; }
            else if (NodeType.ToString().EndsWith("Value")) { return "(value)"; }
            else return "";
        }
    }
}
