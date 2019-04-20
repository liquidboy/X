using Microsoft.WindowsAzure.Storage.Table;
using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public class CloudNodeTypeEntity : TableEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }

        public String InputNodeSlots { get; set; }
        public String OutputNodeSlots { get; set; }
        public int InputNodeSlotCount { get; set; }
        public int OutputNodeSlotCount { get; set; }

        public string Color { get; set; }
        public string View { get; set; }

        public CloudNodeTypeEntity() { }
        public CloudNodeTypeEntity(string name, string category, string inputNodeSlots, int inputNodeSlotCount, string outputNodeSlots, int outputNodeSlotCount, string view, string color)
        {
            PartitionKey = category;
            RowKey = name;
            InputNodeSlots = inputNodeSlots;
            InputNodeSlotCount = inputNodeSlotCount;
            OutputNodeSlots = outputNodeSlots;
            OutputNodeSlotCount = outputNodeSlotCount;
            View = view;
            Color = color;
        }
    }
}
