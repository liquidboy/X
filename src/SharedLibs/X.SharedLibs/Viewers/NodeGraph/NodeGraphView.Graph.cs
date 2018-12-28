using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView
    {
        private struct Node {
            public int ID;
            public string Name;
            public Point Position;
            public Size Size;
            public float Value;
            public Color Color;
            public int InputSlotCount;
            public int OutputSlotCount;

            public Node(int id, string name, Point position, float value, Color color, int inputSlotCount, int outputSlotCount) {
                ID = id;
                Name = name;
                Position = position;
                Value = value;
                Color = color;
                InputSlotCount = inputSlotCount;
                OutputSlotCount = outputSlotCount;
                CalculateSize();
            }

            private void CalculateSize() {
                double inputHeight, outputHeight, width;
                inputHeight = 50d + ((InputSlotCount - 1) * 20d) + 50d;
                outputHeight = 50d + ((OutputSlotCount - 1) * 20d) + 50d;
                width = 150;

                Size = new Size(width, Math.Max(inputHeight, outputHeight));
            }
        }

        private struct NodeLink {
            public int InputNodeID;
            public int InputSlotIndex;
            public int OutputNodeID;
            public int OutputSlotIndex;
        }

        List<Node> _nodes;
        List<NodeLink> _links;
        UIElement _uiNodeGraphXamlRoot;

        private void InitializeExampleNodes(UIElement uiNodeGraphRoot) {
            _uiNodeGraphXamlRoot = uiNodeGraphRoot;
            _nodes = new List<Node>();
            _links = new List<NodeLink>();

            _nodes.Add(new Node(1, "MainTex", new Point(100, 100), 0.5f, Colors.Red, 1, 1));
            _nodes.Add(new Node(2, "BumpMap", new Point(100, 300), 0.42f, Colors.Green, 1, 1));
            _nodes.Add(new Node(3, "Combine", new Point(400, 190), 1.0f, Colors.Yellow, 2, 2));

            DrawNodes();
        }

        private void DrawNodes() {
            if (_uiNodeGraphXamlRoot is Panel) {
                Panel xamlRoot = (Panel)_uiNodeGraphXamlRoot;

                foreach (var node in _nodes) {
                    var newNodeUIElement = new Windows.UI.Xaml.Shapes.Rectangle() {
                        Name = $"node{node.ID}",
                        Fill = new SolidColorBrush(node.Color),
                        Width = node.Size.Width,
                        Height = node.Size.Height
                    };
                    newNodeUIElement.SetValue(Canvas.LeftProperty, node.Position.X);
                    newNodeUIElement.SetValue(Canvas.TopProperty, node.Position.Y);
                    xamlRoot.Children.Add(newNodeUIElement);
                }
            }
        }
    }
}
