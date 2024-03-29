﻿using System;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.UI.NodeGraph
{
    public class Node : BaseEntity
    {
        public string Key { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double SlotPadding { get; set; }
        public double SlotHeaderFooter { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public string Color4 { get; set; }
        public int InputSlotCount { get; set; }
        public string InputSlotLabels { get; set; }
        public int OutputSlotCount { get; set; }
        public string OutputSlotLabels { get; set; }
        public string Grouping { get; set; }
        public int NodeType { get; set; }
        public string Title { get; set; }
        public double Udfd1 { get; set; }
        public double Udfd2 { get; set; }
        public double Udfd3 { get; set; }
        public string Udfs1 { get; set; }
        public string Udfs2 { get; set; }
        public string Udfs3 { get; set; }
        public string Udfs4 { get; set; }
        public bool IsDirty { get; set; }

        public Node() { IsDirty = true; }
        public Node(string key, double positionX, double positionY, double initialWidth, string color1, string color2, string color3, string color4, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, string title, double udfd1, double udfd2, double udfd3, string udfs1, string udfs2, string udfs3, string udfs4) :
            this(key, positionX, positionY, initialWidth, color1, color2, color3, color4, inputSlotCount, inputSlotLabels, outputSlotCount, outputSlotLabels, grouping, nodeType, title, udfd1, udfd2, udfd3)
        {
            Udfs1 = udfs1;
            Udfs2 = udfs2;
            Udfs3 = udfs3;
            Udfs4 = udfs4;
        }
        public Node(string key, double positionX, double positionY, double initialWidth, string color1, string color2, string color3, string color4, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, string title, double udfd1, double udfd2, double udfd3) :
            this(key, positionX, positionY, initialWidth, color1, color2, color3, color4, inputSlotCount, inputSlotLabels, outputSlotCount, outputSlotLabels, grouping, nodeType, title, 50d, 20d)
        {
            Udfd1 = udfd1;
            Udfd2 = udfd2;
            Udfd3 = udfd3;
        }
        public Node(string key, double positionX, double positionY, double initialWidth, string color1, string color2, string color3, string color4, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, string title) :
            this(key, positionX, positionY, initialWidth, color1, color2, color3, color4, inputSlotCount, inputSlotLabels, outputSlotCount, outputSlotLabels, grouping, nodeType, 50d, 20d)
        {
            Title = title;
            IsDirty = true;
        }
        public Node(string key, double positionX, double positionY, double initialWidth, string color1, string color2, string color3, string color4, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, string title, double slotHeaderFooter, double slotPadding) : 
            this(key, positionX, positionY, initialWidth, color1, color2, color3, color4, inputSlotCount, inputSlotLabels, outputSlotCount, outputSlotLabels, grouping, nodeType, slotHeaderFooter, slotPadding)
        {
            Title = title;
        }
        public Node(string key, double positionX, double positionY, double initialWidth, string color1, string color2, string color3, string color4, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, double slotHeaderFooter, double slotPadding)
        {
            Key = key;
            PositionX = positionX;
            PositionY = positionY;
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
            Color4 = color4;
            InputSlotCount = inputSlotCount;
            InputSlotLabels = inputSlotLabels;
            OutputSlotCount = outputSlotCount;
            OutputSlotLabels = outputSlotLabels;
            Width = initialWidth;
            Height = 0;
            Grouping = grouping;
            NodeType = nodeType;
            SlotPadding = slotPadding;
            SlotHeaderFooter = slotHeaderFooter;
            CalculateSize();
            IsDirty = true;
        }

        private void CalculateSize()
        {
            double inputHeight, outputHeight;
            inputHeight = SlotHeaderFooter + ((InputSlotCount - 1) * SlotPadding) + SlotHeaderFooter;
            outputHeight = SlotHeaderFooter + ((OutputSlotCount - 1) * SlotPadding) + SlotHeaderFooter;

            //Width = 150; // = new Size(width, Math.Max(inputHeight, outputHeight));
            Height = Math.Max(inputHeight, outputHeight);
        }
        public InputSlotPosition GetInputSlotPosition(int slotNo)
        {
            return new InputSlotPosition(PositionX, PositionY + ((slotNo + 1) * (Height / (InputSlotCount + 1))));
        }
        public OutputSlotPosition GetOutputSlotPosition(int slotNo)
        {
            return new OutputSlotPosition(PositionX + Width, PositionY + ((slotNo + 1) * (Height / (OutputSlotCount + 1))));
        }
    }
}
