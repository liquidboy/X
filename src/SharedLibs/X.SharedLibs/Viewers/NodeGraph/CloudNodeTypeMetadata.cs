namespace X.Viewer.NodeGraph
{
    public class CloudNodeTypeMetadata : NodeTypeMetadata
    {
        public string InputNodeSlots { get; set; }
        public string OutputNodeSlots { get; set; }
        public int InputNodeSlotCount { get; set; }
        public int OutputNodeSlotCount { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public string Color4 { get; set; }
        public string View { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Template { get; set; }

        public CloudNodeTypeMetadata(string name, string type, string inputNodeSlots, int inputNodeSlotClount, string outputNodeSlots, int outputNodeSlotCount, string color1, string color2, string color3, string color4, string view, string description, string icon, string header, string footer, string template) : base(NodeType.CloudNodeType, name, type)
        {
            NodeType = NodeType.CloudNodeType;
            InputNodeSlots = inputNodeSlots;
            InputNodeSlotCount = inputNodeSlotClount;
            OutputNodeSlots = outputNodeSlots;
            OutputNodeSlotCount = outputNodeSlotCount;
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
            Color4 = color4;
            View = view;
            Description = description;
            Icon = icon;
            Header = header;
            Footer = footer;
            Template = template;
        }

    }
}
