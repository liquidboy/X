namespace X.Viewer.NodeGraph
{
    public class CloudNodeTypeMetadata : NodeTypeMetadata
    {
        public string InputNodeSlots { get; set; }
        public string OutputNodeSlots { get; set; }
        public int InputNodeSlotCount { get; set; }
        public int OutputNodeSlotCount { get; set; }
        public string Color { get; set; }
        public string View { get; set; }


        public CloudNodeTypeMetadata(string name, string type, string inputNodeSlots, int inputNodeSlotClount, string outputNodeSlots, int outputNodeSlotCount, string color, string view) : base(NodeType.CloudNodeType, name, type)
        {
            NodeType = NodeType.CloudNodeType;
            InputNodeSlots = inputNodeSlots;
            InputNodeSlotCount = inputNodeSlotClount;
            OutputNodeSlots = outputNodeSlots;
            OutputNodeSlotCount = outputNodeSlotCount;
            Color = color;
            View = view;
        }

    }
}
