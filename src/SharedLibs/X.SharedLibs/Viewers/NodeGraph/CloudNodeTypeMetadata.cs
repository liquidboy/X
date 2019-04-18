namespace X.Viewer.NodeGraph
{
    public class CloudNodeTypeMetadata : NodeTypeMetadata
    {
        //public NodeType NodeType { get; set; }
        private string _name;
        private string _type;

        public CloudNodeTypeMetadata(string name, string type) : base(NodeType.CloudNodeType)
        {
            NodeType = NodeType.CloudNodeType;
            _name = name;
            _type = type;
        }

        public new string FriendlyName => parseFriendlyName();
        public new string FriendlyType => parseFriendlyType();

        private string parseFriendlyName() => _name;

        private string parseFriendlyType() => _type;
    }
}
