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
using Windows.UI.Xaml.Shapes;
using NodePosition = Windows.Foundation.Point;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using System.Diagnostics;
using System.Numerics;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView
    {
        private struct Node {
            public string Key;
            public NodePosition Position;
            public Size Size;
            public Color Color;
            public int InputSlotCount;
            public int OutputSlotCount;

            public Node(string key, NodePosition position, Color color, int inputSlotCount, int outputSlotCount) {
                Key = key;
                Position = position;
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
            public InputSlotPosition GetInputSlotPosition(int slotNo) {
                return new InputSlotPosition(Position.X, Position.Y + ((slotNo + 1) * (Size.Height / (InputSlotCount + 1))));
            }
            public OutputSlotPosition GetOutputSlotPosition(int slotNo) {
                return new OutputSlotPosition(Position.X + Size.Width, Position.Y + ((slotNo + 1) * (Size.Height / (OutputSlotCount + 1))));
            }
        }

        private struct NodeLink {
            public string InputNodeKey;
            public int InputSlotIndex;
            public string OutputNodeKey;
            public int OutputSlotIndex;

            public NodeLink(string outputNodeKey, int outputSlotIndex, string inputNodeKey, int inputSlotIndex) {
                InputNodeKey = inputNodeKey;
                InputSlotIndex = inputSlotIndex;
                OutputNodeKey = outputNodeKey;
                OutputSlotIndex = outputSlotIndex;
            }
            public string UniqueId{ get{ return $"nsl_{InputNodeKey}_{InputSlotIndex}_{OutputNodeKey}_{OutputSlotIndex}"; } }
        }

        IDictionary<string, Node> _nodes;
        List<NodeLink> _links;


        private void InitializeExampleNodes() {
            _nodes = new Dictionary<string, Node>();
            _links = new List<NodeLink>();

            _nodes.Add("Node1", new Node("Node1", new NodePosition(100, 100), Colors.Red, 1, 1));
            _nodes.Add("Node2", new Node("Node2", new NodePosition(100, 300), Colors.Green, 1, 1));
            _nodes.Add("Node3", new Node("Node3", new NodePosition(400, 190), Colors.Yellow, 2, 2));
            _nodes.Add("Node4", new Node("Node4", new NodePosition(400, 0), Colors.Purple, 1, 1));
            _nodes.Add("Node5", new Node("Node5", new NodePosition(700, 100), Colors.Blue, 2, 1));
            _nodes.Add("Node6", new Node("Node6", new NodePosition(400, 400), Colors.Pink, 1, 1));

            _links.Add(new NodeLink("Node1", 0, "Node3", 0));
            _links.Add(new NodeLink("Node2", 0, "Node3", 1));
            _links.Add(new NodeLink("Node4", 0, "Node5", 0));
            _links.Add(new NodeLink("Node3", 0, "Node5", 0));
            _links.Add(new NodeLink("Node3", 1, "Node5", 0));
            _links.Add(new NodeLink("Node6", 0, "Node5", 1));

            DrawNodes();
        }

        private void DrawNodes() {
            
            if (_isRendererInitialized) {
                //draw nodes
                foreach (var node in _nodes) {
                    DrawNode(node.Value);
                }
                
                //node-slot-links between the node-slots
                foreach (var link in _links)
                {
                    DrawNodeSlotLink(link);
                }                
            }
        }

    }
}
