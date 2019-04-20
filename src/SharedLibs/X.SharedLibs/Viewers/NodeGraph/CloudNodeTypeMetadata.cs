namespace X.Viewer.NodeGraph
{
    public class CloudNodeTypeMetadata : NodeTypeMetadata
    {
        //public NodeType NodeType { get; set; }
        private string _name;
        private string _type;
        public string InputNodeSlots { get; set; }
        public string OutputNodeSlots { get; set; }
        public int InputNodeSlotCount { get; set; }
        public int OutputNodeSlotCount { get; set; }


        public CloudNodeTypeMetadata(string name, string type, string inputNodeSlots, int inputNodeSlotClount, string outputNodeSlots, int outputNodeSlotCount) : base(NodeType.CloudNodeType)
        {
            NodeType = NodeType.CloudNodeType;
            _name = name;
            _type = type;
            InputNodeSlots = inputNodeSlots;
            InputNodeSlotCount = inputNodeSlotClount;
            OutputNodeSlots = outputNodeSlots;
            OutputNodeSlotCount = outputNodeSlotCount;

        }

        public new string FriendlyName => parseFriendlyName();
        public new string FriendlyType => parseFriendlyType();

        private string parseFriendlyName() => _name;

        private string parseFriendlyType() => _type;
    }
}
