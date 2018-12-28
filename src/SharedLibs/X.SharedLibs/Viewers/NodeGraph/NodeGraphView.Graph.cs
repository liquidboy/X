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
            public Point GetInputSlotPosition(int slotNo) {
                return new Point(Position.X, Position.Y + ((slotNo + 1) * (Size.Height / (InputSlotCount + 1))));
            }
            public Point GetOutputSlotPosition(int slotNo) {
                return new Point(Position.X + Size.Width, Position.Y + ((slotNo + 1) * (Size.Height / (OutputSlotCount + 1))));
            }
        }

        private struct NodeLink {
            public int InputNodeIndex;
            public int InputSlotIndex;
            public int OutputNodeIndex;
            public int OutputSlotIndex;

            public NodeLink(int inputNodeIndex, int inputSlotIndex, int outputNodeIndex, int outputSlotIndex) {
                InputNodeIndex = inputNodeIndex;
                InputSlotIndex = inputSlotIndex;
                OutputNodeIndex = outputNodeIndex;
                OutputSlotIndex = outputSlotIndex;
            }
        }

        List<Node> _nodes;
        List<NodeLink> _links;
        UIElement _uiNodeGraphXamlRoot;

        private void InitializeExampleNodes(UIElement uiNodeGraphRoot) {
            _uiNodeGraphXamlRoot = uiNodeGraphRoot;
            _nodes = new List<Node>();
            _links = new List<NodeLink>();

            _nodes.Add(new Node(0, "Node1", new Point(100, 100), 0.5f, Colors.Red, 1, 1));
            _nodes.Add(new Node(1, "Node2", new Point(100, 300), 0.42f, Colors.Green, 1, 1));
            _nodes.Add(new Node(2, "Node3", new Point(400, 190), 1.0f, Colors.Yellow, 2, 2));
            _nodes.Add(new Node(3, "Node4", new Point(400, 0), 1.6f, Colors.Purple, 1, 1));
            _nodes.Add(new Node(4, "Node5", new Point(700, 100), 0.8f, Colors.Blue, 2, 1));
            _nodes.Add(new Node(5, "Node6", new Point(400, 400), 1.2f, Colors.Black, 1, 1));

            _links.Add(new NodeLink(2, 0, 0, 0));
            _links.Add(new NodeLink(2, 1, 1, 0));
            _links.Add(new NodeLink(4, 0, 3, 0));
            _links.Add(new NodeLink(4, 0, 2, 0));
            _links.Add(new NodeLink(4, 0, 2, 1));
            _links.Add(new NodeLink(4, 1, 5, 0));

            DrawNodes();
        }

        private void DrawNodes() {
            if (_uiNodeGraphXamlRoot is Panel) {
                Panel xamlRoot = (Panel)_uiNodeGraphXamlRoot;

                //draw nodes
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

                //draw links between the nodes
                foreach (var link in _links)
                {
                    Node inputNode = _nodes[link.InputNodeIndex];
                    Node outputNode = _nodes[link.OutputNodeIndex];

                    Point p1 = inputNode.GetInputSlotPosition(link.InputSlotIndex);
                    Point p2 = outputNode.GetOutputSlotPosition(link.OutputSlotIndex);

                    PathFigure pthFigure = new PathFigure();
                    pthFigure.StartPoint = p1; //output point of link

                    BezierSegment bezierSegment = new BezierSegment();
                    bezierSegment.Point1 = new Point(p1.X - 50, p1.Y);
                    bezierSegment.Point2 = new Point(p2.X + 50, p2.Y);
                    bezierSegment.Point3 = p2;

                    PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                    myPathSegmentCollection.Add(bezierSegment);

                    pthFigure.Segments = myPathSegmentCollection;

                    PathFigureCollection pthFigureCollection = new PathFigureCollection();
                    pthFigureCollection.Add(pthFigure);

                    PathGeometry pthGeometry = new PathGeometry();
                    pthGeometry.Figures = pthFigureCollection;

                    Path arcPath = new Path();
                    arcPath.Stroke = new SolidColorBrush(Colors.Orange);
                    arcPath.StrokeThickness = 1;
                    arcPath.Data = pthGeometry;
                    xamlRoot.Children.Add(arcPath);
                }
                
            }
        }
    }
}
