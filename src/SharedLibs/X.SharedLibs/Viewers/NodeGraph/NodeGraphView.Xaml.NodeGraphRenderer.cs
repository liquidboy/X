using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using System;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphRenderer
    {
        bool _isRendererInitialized;
        UIElement _uiNodeGraphXamlRoot;
        Panel _uiNodeGraphPanelXamlRoot;
        
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
                Width = node.Width,
                Height = node.Height,
                Tag = "nc"
            };
            newNodeGroup.SetValue(Canvas.LeftProperty, node.PositionX);
            newNodeGroup.SetValue(Canvas.TopProperty, node.PositionY);

            //node in node-container
            var newNodeUIElement = new Windows.UI.Xaml.Shapes.Rectangle()
            {
                Name = $"n_{node.Key}",
                Fill = new SolidColorBrush((Color)(typeof(Colors)).GetProperty(node.Color).GetValue(null)),
                Width = node.Width,
                Height = node.Height,
                Tag = "n"
            };
            newNodeGroup.Children.Add(newNodeUIElement);

            //create nodevisual
            if (node.Key == "Node1") {
                CreateNodeVisual(node.Key, newNodeUIElement, EffectType.NoEffect);
            }
            
            //node-slots in node-container
            for (int slotIndex = 0; slotIndex < node.InputSlotCount; slotIndex++)
            {
                var slotPosition = node.GetInputSlotPosition(slotIndex);
                var newSlotUIElement = new Windows.UI.Xaml.Shapes.Ellipse()
                {
                    Name = $"nsi_{node.Key}_{slotIndex}",
                    Width = slotDiameter,
                    Height = slotDiameter,
                    Fill = new SolidColorBrush(Colors.Black),
                    Tag = "nsi"
                };
                newSlotUIElement.SetValue(Canvas.LeftProperty, slotPosition.X - slotRadius - node.PositionX);
                newSlotUIElement.SetValue(Canvas.TopProperty, slotPosition.Y - slotRadius - node.PositionY);
                newNodeGroup.Children.Add(newSlotUIElement);
            }

            for (int slotIndex = 0; slotIndex < node.OutputSlotCount; slotIndex++)
            {
                var slotPosition = node.GetOutputSlotPosition(slotIndex);
                var newSlotUIElement = new Windows.UI.Xaml.Shapes.Ellipse()
                {
                    Name = $"nso_{node.Key}_{slotIndex}",
                    Width = slotDiameter,
                    Height = slotDiameter,
                    Fill = new SolidColorBrush(Colors.Black),
                    Tag = "nso"
                };
                newSlotUIElement.SetValue(Canvas.LeftProperty, slotPosition.X - slotRadius - node.PositionX);
                newSlotUIElement.SetValue(Canvas.TopProperty, slotPosition.Y - slotRadius - node.PositionY);
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
            if (TryUpdateExistingNodeSlotLink(nodeLink.UniqueId, inputSlotPosition, outputSlotPosition)) return;
            
            //create new node-slot-link
            CreateNewNodeSlotLink(nodeLink.UniqueId, inputSlotPosition, outputSlotPosition, Colors.Orange);
        }

        (bool ElementFound, UIElement Element) HasNodeSlotLinkAlreadyBeenRendered(string id) {
            var foundUIElement = (UIElement)_uiNodeGraphPanelXamlRoot.FindName(id);
            if (foundUIElement != null) return (true, foundUIElement);
            return (false, null);
        }

        bool TryUpdateExistingNodeSlotLink(string id, InputSlotPosition inputSlotPosition, OutputSlotPosition outputSlotPosition) {
            var findingUIElement = HasNodeSlotLinkAlreadyBeenRendered(id);
            if (findingUIElement.ElementFound)
            {
                PathGeometry pthGeometryFound = (PathGeometry)((Path)findingUIElement.Element).Data;
                PathFigure pthFigureFound = pthGeometryFound.Figures.First();
                BezierSegment bezierSegmentFound = (BezierSegment)pthFigureFound.Segments.First();

                pthFigureFound.StartPoint = inputSlotPosition;
                bezierSegmentFound.Point1 = new Point(inputSlotPosition.X - 50, inputSlotPosition.Y);
                bezierSegmentFound.Point2 = new Point(outputSlotPosition.X + 50, outputSlotPosition.Y);
                bezierSegmentFound.Point3 = outputSlotPosition;
                return true;
            }

            return false;
        }
        void TryRemoveNodeSlotLink(string id) {
            var findingSlot = HasNodeSlotLinkAlreadyBeenRendered(id);
            if (findingSlot.ElementFound) {
                _uiNodeGraphPanelXamlRoot.Children.Remove(findingSlot.Element);
            }
        }
        void CreateNewNodeSlotLink(string id, InputSlotPosition inputSlotPosition, OutputSlotPosition outputSlotPosition, Color color)
        {
            if (HasNodeSlotLinkAlreadyBeenRendered(id).ElementFound) return;

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
                Name = id,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 1,
                Data = new PathGeometry() { Figures = pthFigureCollection },
                Tag = "nsl"
            };
            _uiNodeGraphPanelXamlRoot.Children.Add(arcPath);
        }

        public void ClearRenderer()
        {
            _uiNodeGraphPanelXamlRoot.Children.Clear();
        }
    }
}
