using System;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public class Node : BaseEntity
    {
        public string Key { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Color { get; set; }
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

        public Node() { }
        public Node(string key, double positionX, double positionY, double initialWidth, string color, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, string title, double udfd1, double udfd2, double udfd3) :
            this(key, positionX, positionY, initialWidth, color, inputSlotCount, inputSlotLabels, outputSlotCount, outputSlotLabels, grouping, nodeType, title)
        {
            Udfd1 = udfd1;
            Udfd2 = udfd2;
            Udfd3 = udfd3;
        }
        public Node(string key, double positionX, double positionY, double initialWidth, string color, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType, string title) : 
            this(key, positionX, positionY, initialWidth, color, inputSlotCount, inputSlotLabels, outputSlotCount, outputSlotLabels, grouping, nodeType)
        {
            Title = title;
        }
        public Node(string key, double positionX, double positionY, double initialWidth, string color, int inputSlotCount, string inputSlotLabels, int outputSlotCount, string outputSlotLabels, string grouping, int nodeType)
        {
            Key = key;
            PositionX = positionX;
            PositionY = positionY;
            Color = color;
            InputSlotCount = inputSlotCount;
            InputSlotLabels = inputSlotLabels;
            OutputSlotCount = outputSlotCount;
            OutputSlotLabels = outputSlotLabels;
            Width = initialWidth;
            Height = 0;
            Grouping = grouping;
            NodeType = nodeType;
            CalculateSize();
        }

        private void CalculateSize()
        {
            double inputHeight, outputHeight;
            inputHeight = 50d + ((InputSlotCount - 1) * 20d) + 50d;
            outputHeight = 50d + ((OutputSlotCount - 1) * 20d) + 50d;

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
