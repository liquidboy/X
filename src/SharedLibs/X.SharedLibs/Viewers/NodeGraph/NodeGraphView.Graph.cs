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
        UIElement _uiNodeGraphXamlRoot;
        Panel _uiNodeGraphPanelXamlRoot;
        bool _isNodeSelected = false;
        string _selectedNodeKey;
        string _selectedUIElementNodeName;
        Point _selectedNodePosition;

        private void InitializeExampleNodes(UIElement uiNodeGraphRoot) {
            _uiNodeGraphXamlRoot = uiNodeGraphRoot;
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
            
            if (_uiNodeGraphXamlRoot is Panel) {
                _uiNodeGraphPanelXamlRoot = (Panel)_uiNodeGraphXamlRoot;

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

        private void DrawNode(Node node) {
            double slotRadius = 5;
            double slotDiameter = 2 * slotRadius;

            //node-container
            var newNodeGroup = new Canvas()
            {
                Name = $"nc_{node.Key}",
                Width = node.Size.Width,
                Height = node.Size.Height
            };
            newNodeGroup.SetValue(Canvas.LeftProperty, node.Position.X);
            newNodeGroup.SetValue(Canvas.TopProperty, node.Position.Y);
            newNodeGroup.PointerEntered += NewNodeGroup_PointerEntered;
            newNodeGroup.PointerExited += NewNodeGroup_PointerExited;

            //node in node-container
            var newNodeUIElement = new Windows.UI.Xaml.Shapes.Rectangle()
            {
                Name = $"n_{node.Key}",
                Fill = new SolidColorBrush(node.Color),
                Width = node.Size.Width,
                Height = node.Size.Height
            };
            newNodeGroup.Children.Add(newNodeUIElement);

            //node-slots in node-container
            for (int slotIndex = 0; slotIndex < node.InputSlotCount; slotIndex++)
            {
                var slotPosition = node.GetInputSlotPosition(slotIndex);
                var newSlotUIElement = new Windows.UI.Xaml.Shapes.Ellipse()
                {
                    Width = slotDiameter,
                    Height = slotDiameter,
                    Fill = new SolidColorBrush(Colors.Black)
                };
                newSlotUIElement.SetValue(Canvas.LeftProperty, slotPosition.X - slotRadius - node.Position.X);
                newSlotUIElement.SetValue(Canvas.TopProperty, slotPosition.Y - slotRadius - node.Position.Y);
                newNodeGroup.Children.Add(newSlotUIElement);
            }

            for (int slotIndex = 0; slotIndex < node.OutputSlotCount; slotIndex++)
            {
                var slotPosition = node.GetOutputSlotPosition(slotIndex);
                var newSlotUIElement = new Windows.UI.Xaml.Shapes.Ellipse()
                {
                    Width = slotDiameter,
                    Height = slotDiameter,
                    Fill = new SolidColorBrush(Colors.Black)
                };
                newSlotUIElement.SetValue(Canvas.LeftProperty, slotPosition.X - slotRadius - node.Position.X);
                newSlotUIElement.SetValue(Canvas.TopProperty, slotPosition.Y - slotRadius - node.Position.Y);
                newNodeGroup.Children.Add(newSlotUIElement);
            }

            _uiNodeGraphPanelXamlRoot.Children.Add(newNodeGroup);
        }

        //private void NewNodeGroup_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        //{
        //    Debug.WriteLine($"node cursor position : {e.GetCurrentPoint(null).RawPosition}");
        //}

        private void DrawNodeSlotLink(NodeLink nodeLink) {
            Node inputNode = _nodes[nodeLink.InputNodeKey];
            Node outputNode = _nodes[nodeLink.OutputNodeKey];

            InputSlotPosition inputSlotPosition = inputNode.GetInputSlotPosition(nodeLink.InputSlotIndex);
            OutputSlotPosition outputSlotPosition = outputNode.GetOutputSlotPosition(nodeLink.OutputSlotIndex);

            // find if it exists to change it
            var foundUIElement = _uiNodeGraphPanelXamlRoot.FindName(nodeLink.UniqueId);
            if (foundUIElement != null) {
                PathGeometry pthGeometryFound = (PathGeometry)((Path)foundUIElement).Data;
                PathFigure pthFigureFound = pthGeometryFound.Figures.First();
                BezierSegment bezierSegmentFound = (BezierSegment)pthFigureFound.Segments.First();

                pthFigureFound.StartPoint = inputSlotPosition;
                bezierSegmentFound.Point1 = new Point(inputSlotPosition.X - 50, inputSlotPosition.Y);
                bezierSegmentFound.Point2 = new Point(outputSlotPosition.X + 50, outputSlotPosition.Y);
                bezierSegmentFound.Point3 = outputSlotPosition;
                return;
            }

            //create new node slot link
            PathFigure pthFigure = new PathFigure();
            pthFigure.StartPoint = inputSlotPosition; //output point of link

            BezierSegment bezierSegment = new BezierSegment()
            {
                Point1 = new Point(inputSlotPosition.X - 50, inputSlotPosition.Y),
                Point2 = new Point(outputSlotPosition.X + 50, outputSlotPosition.Y),
                Point3 = outputSlotPosition
            };

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(bezierSegment);

            pthFigure.Segments = myPathSegmentCollection;

            PathFigureCollection pthFigureCollection = new PathFigureCollection();
            pthFigureCollection.Add(pthFigure);

            PathGeometry pthGeometry = new PathGeometry();
            pthGeometry.Figures = pthFigureCollection;

            Path arcPath = new Path();
            arcPath.Name = nodeLink.UniqueId;
            arcPath.Stroke = new SolidColorBrush(Colors.Orange);
            arcPath.StrokeThickness = 1;
            arcPath.Data = pthGeometry;
            _uiNodeGraphPanelXamlRoot.Children.Add(arcPath);
        }

        private void NewNodeGroup_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _selectedNodeKey = "";
            _selectedUIElementNodeName = "";
            _isNodeSelected = false;

        }
        
        private void NewNodeGroup_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //Console.WriteLine("pointer entered");
            Canvas canvas = (Canvas)sender;
            _selectedNodeKey = canvas.Name.Replace("nc_", string.Empty);
            _selectedUIElementNodeName = canvas.Name;
            _selectedNodePosition = new NodePosition((double)canvas.GetValue(Canvas.LeftProperty), (double)canvas.GetValue(Canvas.TopProperty));
            _isNodeSelected = true;
        }

        private void MoveNodeContainer(Vector2 distanceToMove) {
            Canvas foundNodeUIElement = (Canvas)_uiNodeGraphPanelXamlRoot.FindName(_selectedUIElementNodeName);
            //double left = (double)foundNodeUIElement.GetValue(Canvas.LeftProperty);
            //double top = (double)foundNodeUIElement.GetValue(Canvas.TopProperty);

            var foundNode = _nodes[_selectedNodeKey];
            foundNode.Position.X = _selectedNodePosition.X + distanceToMove.X;
            foundNode.Position.Y = _selectedNodePosition.Y + distanceToMove.Y;
            Debug.WriteLine($"node position 1: {foundNode.Position}");

            _nodes[_selectedNodeKey] = foundNode;

            var foundNode2 = _nodes[_selectedNodeKey];
            Debug.WriteLine($"node position 2: {foundNode2.Position}");
            
            foundNodeUIElement.SetValue(Canvas.LeftProperty, foundNode.Position.X);
            foundNodeUIElement.SetValue(Canvas.TopProperty, foundNode.Position.Y);

            //node-slot-links between the node-slots
            foreach (var link in _links)
            {
                //need to update links ?????  I want to use Windows Composition eventually
                if(link.InputNodeKey.Equals(_selectedNodeKey) || link.OutputNodeKey.Equals(_selectedNodeKey))
                    DrawNodeSlotLink(link);
            }
        }
    }
}
