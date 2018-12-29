using System;
using NodePosition = Windows.Foundation.Point;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.Foundation;
using Windows.UI;

namespace X.Viewer.NodeGraph
{
    public struct Node
    {
        public string Key;
        public NodePosition Position;
        public Size Size;
        public Color Color;
        public int InputSlotCount;
        public int OutputSlotCount;

        public Node(string key, NodePosition position, Color color, int inputSlotCount, int outputSlotCount)
        {
            Key = key;
            Position = position;
            Color = color;
            InputSlotCount = inputSlotCount;
            OutputSlotCount = outputSlotCount;
            CalculateSize();
        }
        private void CalculateSize()
        {
            double inputHeight, outputHeight, width;
            inputHeight = 50d + ((InputSlotCount - 1) * 20d) + 50d;
            outputHeight = 50d + ((OutputSlotCount - 1) * 20d) + 50d;
            width = 150;

            Size = new Size(width, Math.Max(inputHeight, outputHeight));
        }
        public InputSlotPosition GetInputSlotPosition(int slotNo)
        {
            return new InputSlotPosition(Position.X, Position.Y + ((slotNo + 1) * (Size.Height / (InputSlotCount + 1))));
        }
        public OutputSlotPosition GetOutputSlotPosition(int slotNo)
        {
            return new OutputSlotPosition(Position.X + Size.Width, Position.Y + ((slotNo + 1) * (Size.Height / (OutputSlotCount + 1))));
        }
    }
}
