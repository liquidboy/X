﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class NodeGraphView
    {
        bool _isRendererInitialized;
        UIElement _uiNodeGraphXamlRoot;
        Panel _uiNodeGraphPanelXamlRoot;
        
        Point _selectedNodeStartDragPosition;
        string _selectedNodeKey;

        private void InitializeRenderer(UIElement uiNodeGraphRoot)
        {   
            if (uiNodeGraphRoot is Panel)
            {
                _uiNodeGraphXamlRoot = uiNodeGraphRoot;
                _uiNodeGraphPanelXamlRoot = (Panel)_uiNodeGraphXamlRoot;
                _isRendererInitialized = true;
            }
            
        }

        private void DrawNode(Node node)
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

        private void DrawNodeSlotLink(NodeLink nodeLink)
        {
            Node inputNode = _nodes[nodeLink.InputNodeKey];
            Node outputNode = _nodes[nodeLink.OutputNodeKey];

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

        private void CheckIfNodeIsPressed(Point point) {
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement && 
                    ((FrameworkElement)x).Tag != null && 
                    ((FrameworkElement)x).Tag.ToString().Equals("nc"));
                if (foundNC != null) {
                    //_isNodeFocusedOn = true;
                    var uiCurrentFocusedNode = (FrameworkElement)foundNC.FirstOrDefault();
                    _selectedNodeStartDragPosition = new NodePosition((double)uiCurrentFocusedNode.GetValue(Canvas.LeftProperty), (double)uiCurrentFocusedNode.GetValue(Canvas.TopProperty));
                    _selectedNodeKey = uiCurrentFocusedNode.Name;
                }
            }
        }

        private void ClearSelectedNode() {
            //_isNodeFocusedOn = false;
            _selectedNodeKey = string.Empty;
        }


        private void MoveNode(Vector2 distanceToMove, double scale)
        {
            //update new node position
            var nodeKey = _selectedNodeKey;
            var foundNode = _nodes[nodeKey];
            foundNode.Position.X = _selectedNodeStartDragPosition.X + (distanceToMove.X / scale);
            foundNode.Position.Y = _selectedNodeStartDragPosition.Y + (distanceToMove.Y / scale);
            _nodes[nodeKey] = foundNode; //force immutable element to be updated

            //update node position
            var foundNodeUIElement = (Canvas)_uiNodeGraphPanelXamlRoot.FindName(nodeKey);
            foundNodeUIElement.SetValue(Canvas.LeftProperty, foundNode.Position.X);
            foundNodeUIElement.SetValue(Canvas.TopProperty, foundNode.Position.Y);

            //update node-slot-links positions between the node-slots
            foreach (var link in _links)
            {
                //need to update links ?????  I want to use Windows Composition eventually
                if (link.InputNodeKey.Equals(nodeKey) || link.OutputNodeKey.Equals(nodeKey))
                    DrawNodeSlotLink(link);
            }
        }
    }
}