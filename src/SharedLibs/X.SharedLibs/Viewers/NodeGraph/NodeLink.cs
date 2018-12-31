using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public class NodeLink : BaseEntity
    {
        public string InputNodeKey { get; set; }
        public int InputSlotIndex { get; set; }
        public string OutputNodeKey { get; set; }
        public int OutputSlotIndex { get; set; }
        public string Grouping { get; set; }

        public NodeLink() { }
        public NodeLink(string outputNodeKey, int outputSlotIndex, string inputNodeKey, int inputSlotIndex, string grouping)
        {
            InputNodeKey = inputNodeKey;
            InputSlotIndex = inputSlotIndex;
            OutputNodeKey = outputNodeKey;
            OutputSlotIndex = outputSlotIndex;
            Grouping = grouping;
        }
        public string UniqueId { get { return $"nsl_{InputNodeKey}_{InputSlotIndex}_{OutputNodeKey}_{OutputSlotIndex}"; } }
    }
}
