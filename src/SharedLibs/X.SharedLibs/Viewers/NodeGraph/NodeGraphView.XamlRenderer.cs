using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NodePosition = Windows.Foundation.Point;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using System.Numerics;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphRenderer
    {
        bool _isRendererInitialized;
        UIElement _uiNodeGraphXamlRoot;
        Panel _uiNodeGraphPanelXamlRoot;
        
        Point _selectedNodeStartDragPosition;
        string _selectedNodeKey;

        public void InitializeRenderer(UIElement uiNodeGraphRoot)
        {   
            if (!_isRendererInitialized && uiNodeGraphRoot is Panel)
            {
                _uiNodeGraphXamlRoot = uiNodeGraphRoot;
                _uiNodeGraphPanelXamlRoot = (Panel)_uiNodeGraphXamlRoot;
                _isRendererInitialized = true;
            }
            
        }

        public void RenderNode(Node node)
        {
            double slotRadius = 5;
            double slotDiameter = 2 * slotRadius;

            //node-container
            var newNodeGroup = new Canvas()
            {
                Name = node.Key,
                Width = node.Size.Width,
                Height = node.Size.Height,
                Tag = "nc"
            };
            newNodeGroup.SetValue(Canvas.LeftProperty, node.Position.X);
            newNodeGroup.SetValue(Canvas.TopProperty, node.Position.Y);
            
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

        public void RenderNodeSlotLink(NodeLink nodeLink)
        {
            Node inputNode = FindNode(nodeLink.InputNodeKey);
            Node outputNode = FindNode(nodeLink.OutputNodeKey);

            InputSlotPosition inputSlotPosition = inputNode.GetInputSlotPosition(nodeLink.InputSlotIndex);
            OutputSlotPosition outputSlotPosition = outputNode.GetOutputSlotPosition(nodeLink.OutputSlotIndex);

            // find node-slot-link if it exists and then update it
            var foundUIElement = _uiNodeGraphPanelXamlRoot.FindName(nodeLink.UniqueId);
            if (foundUIElement != null)
            {
                PathGeometry pthGeometryFound = (PathGeometry)((Path)foundUIElement).Data;
                PathFigure pthFigureFound = pthGeometryFound.Figures.First();
                BezierSegment bezierSegmentFound = (BezierSegment)pthFigureFound.Segments.First();

                pthFigureFound.StartPoint = inputSlotPosition;
                bezierSegmentFound.Point1 = new Point(inputSlotPosition.X - 50, inputSlotPosition.Y);
                bezierSegmentFound.Point2 = new Point(outputSlotPosition.X + 50, outputSlotPosition.Y);
                bezierSegmentFound.Point3 = outputSlotPosition;
                return;
            }

            //create new node-slot-link
            PathFigure pthFigure = new PathFigure() { StartPoint = inputSlotPosition };

            BezierSegment bezierSegment = new BezierSegment()
            {
                Point1 = new Point(inputSlotPosition.X - 50, inputSlotPosition.Y),
                Point2 = new Point(outputSlotPosition.X + 50, outputSlotPosition.Y),
                Point3 = outputSlotPosition
            };

            pthFigure.Segments = new PathSegmentCollection();
            pthFigure.Segments.Add(bezierSegment);

            PathFigureCollection pthFigureCollection = new PathFigureCollection();
            pthFigureCollection.Add(pthFigure);

            Path arcPath = new Path()
            {
                Name = nodeLink.UniqueId,
                Stroke = new SolidColorBrush(Colors.Orange),
                StrokeThickness = 1,
                Data = new PathGeometry() { Figures = pthFigureCollection }
            };
            _uiNodeGraphPanelXamlRoot.Children.Add(arcPath);
        }

        public void SetSelectedNode(Point point) {
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement && 
                    ((FrameworkElement)x).Tag != null && 
                    ((FrameworkElement)x).Tag.ToString().Equals("nc"));
                if (foundNC != null) {
                    var uiCurrentFocusedNode = (FrameworkElement)foundNC.FirstOrDefault();
                    _selectedNodeStartDragPosition = new NodePosition((double)uiCurrentFocusedNode.GetValue(Canvas.LeftProperty), (double)uiCurrentFocusedNode.GetValue(Canvas.TopProperty));
                    _selectedNodeKey = uiCurrentFocusedNode.Name;
                }
            }
        }

        public void ClearSelectedNode() {
            _selectedNodeKey = string.Empty;
        }


        public void MoveSelectedNode(Vector2 distanceToMove, double scale)
        {
            //update new node position
            var updatedNode = UpdateNodePosition(_selectedNodeKey, _selectedNodeStartDragPosition.X + (distanceToMove.X / scale), _selectedNodeStartDragPosition.Y + (distanceToMove.Y / scale));

            //update node position
            var foundNodeUIElement = (Canvas)_uiNodeGraphPanelXamlRoot.FindName(_selectedNodeKey);
            foundNodeUIElement.SetValue(Canvas.LeftProperty, updatedNode.Position.X);
            foundNodeUIElement.SetValue(Canvas.TopProperty, updatedNode.Position.Y);

            //update node-slot-links positions between the node-slots
            DrawNodeSlotLink(_selectedNodeKey);

        }
    }
}
