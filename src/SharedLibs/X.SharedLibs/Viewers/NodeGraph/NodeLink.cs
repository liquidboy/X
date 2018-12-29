namespace X.Viewer.NodeGraph
{
    public struct NodeLink
    {
        public string InputNodeKey;
        public int InputSlotIndex;
        public string OutputNodeKey;
        public int OutputSlotIndex;

        public NodeLink(string outputNodeKey, int outputSlotIndex, string inputNodeKey, int inputSlotIndex)
        {
            InputNodeKey = inputNodeKey;
            InputSlotIndex = inputSlotIndex;
            OutputNodeKey = outputNodeKey;
            OutputSlotIndex = outputSlotIndex;
        }
        public string UniqueId { get { return $"nsl_{InputNodeKey}_{InputSlotIndex}_{OutputNodeKey}_{OutputSlotIndex}"; } }
    }
}
