﻿using Microsoft.WindowsAzure.Storage.Table;
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

        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public string Color4 { get; set; }
        public string View { get; set; }

        public string Description { get; set; }
        public string Icon { get; set; }

        public CloudNodeTypeEntity() { }
        public CloudNodeTypeEntity(string name, string category, string inputNodeSlots, int inputNodeSlotCount, string outputNodeSlots, int outputNodeSlotCount, string view, string color1, string color2, string color3, string color4, string description, string icon)
        {
            PartitionKey = category;
            RowKey = name;
            InputNodeSlots = inputNodeSlots;
            InputNodeSlotCount = inputNodeSlotCount;
            OutputNodeSlots = outputNodeSlots;
            OutputNodeSlotCount = outputNodeSlotCount;
            View = view;
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
            Color4 = color4;
            Description = description;
            Icon = icon;
        }
    }
}
