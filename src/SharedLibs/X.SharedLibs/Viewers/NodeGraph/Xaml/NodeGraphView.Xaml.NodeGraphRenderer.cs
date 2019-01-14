using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using X.Viewer.NodeGraph.NodeTypeComponents;
using System.Collections.Generic;

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

        // ==========
        // NODE
        // ==========
        public void RenderNode(Node node, List<NodeLink> relatedLinks)
        {
            double slotRadius = 5;
            double slotDiameter = 2 * slotRadius;

            //vm if it is needed
            var nodeNodeLinkVM = new NodeNodeLinkModel() {
                Node = node,
                InputNodeLinks = relatedLinks.Where(x => x.InputNodeKey == node.Key).OrderBy(x=>x.InputSlotIndex).ToList(),
                OutputNodeLinks = relatedLinks.Where(x => x.OutputNodeKey == node.Key).OrderBy(x => x.OutputSlotIndex).ToList()
            };
            
            //node-container
            var newNodeGroup = CreateNodeContainerUI(node);
            newNodeGroup.DataContext = nodeNodeLinkVM;

            //node-slots in node-container (appears underneath those rendered after)
            for (int slotIndex = 0; slotIndex < node.InputSlotCount; slotIndex++)
            {
                newNodeGroup.Children.Add(CreateSlotUI("nsi", node, slotIndex, slotDiameter, slotRadius, Colors.Black, true, Colors.LightGray));
            }

            for (int slotIndex = 0; slotIndex < node.OutputSlotCount; slotIndex++)
            {
                newNodeGroup.Children.Add(CreateSlotUI("nso", node, slotIndex, slotDiameter, slotRadius, Colors.Black, false, Colors.LightGray));
            }
            
            //node in node-container
            FrameworkElement newNodeUIElement = CreateNodeUI(node);
            newNodeGroup.Children.Add(newNodeUIElement);
            
            //node-title
            if (node.Title != null) newNodeGroup.Children.Add(CreateNodeTitleUI(node));
            



            //node-visual, created at the end after the node's full dimensions are realized
            //await DispatcherHelper.ExecuteOnUIThreadAsync(()=> CreateNodeVisual(nodeNodeLinkVM, newNodeUIElement, (NodeType)node.NodeType), Windows.UI.Core.CoreDispatcherPriority.Normal);
            CreateNodeVisualUI(nodeNodeLinkVM, newNodeUIElement, (NodeType)node.NodeType);

            _uiNodeGraphPanelXamlRoot.Children.Add(newNodeGroup);
        }

        Panel CreateNodeContainerUI(Node node) {
            var newNodeGroup = new Canvas()
            {
                Name = node.Key,
                Width = node.Width,
                Height = node.Height,
                //Background = new SolidColorBrush(Colors.Red),
                Tag = "nc"
            };
            newNodeGroup.SetValue(Canvas.LeftProperty, node.PositionX);
            newNodeGroup.SetValue(Canvas.TopProperty, node.PositionY);

            //shadow caster
            newNodeGroup.Shadow = sharedShadow;
            newNodeGroup.Projection = new PlaneProjection() { GlobalOffsetZ = 1 };

            return newNodeGroup;
        }

        FrameworkElement CreateNodeUI(Node node) {
            FrameworkElement newNodeUIElement = null;
            if (node.NodeType > 100 && node.NodeType < 1000) //EFFECTS
            {
                newNodeUIElement = new Windows.UI.Xaml.Shapes.Rectangle()
                {
                    Fill = new SolidColorBrush((Color)(typeof(Colors)).GetProperty(node.Color).GetValue(null))
                };
            }
            else if (node.NodeType > 1000 && node.NodeType < 2000) //VALUES
            {
                var nodeType = (NodeType)node.NodeType;

                switch (nodeType)
                {
                    case NodeType.TextboxValue: newNodeUIElement = new TextboxValue(); break;
                    case NodeType.SliderValue: newNodeUIElement = new SliderValue(); break;
                    case NodeType.ToggleValue: newNodeUIElement = new ToggleValue(); break;
                    case NodeType.BlendEffectModeValue: newNodeUIElement = new BlendEffectModeValue(); break;
                    case NodeType.ColorSliderValue: newNodeUIElement = new ColorSliderValue(); break;
                    case NodeType.GammaTransferValue: newNodeUIElement = new GammaTransferSliderValue(); break;
                    case NodeType.CanvasAlphaModeValue: newNodeUIElement = new CanvasAlphaModeValue(); break;
                    case NodeType.BorderModeValue: newNodeUIElement = new BorderModeValue(); break;
                    case NodeType.TransformMatrixValue: newNodeUIElement = new TransformMatrixValue(); break;
                }
                INodeTypeComponent nodeTypeComponent = newNodeUIElement as INodeTypeComponent;
                nodeTypeComponent.NodeTypeValueChanged += NodeTypeComponent_NodeTypeValueChanged;
            }
            else
            {
                // no idea what this node is so just make it a rectangle for now
                newNodeUIElement = new Windows.UI.Xaml.Shapes.Rectangle()
                {
                    Fill = new SolidColorBrush((Color)(typeof(Colors)).GetProperty(node.Color).GetValue(null))
                };
            }

            newNodeUIElement.Name = $"n_{node.Key}";
            newNodeUIElement.Width = node.Width;
            newNodeUIElement.Height = node.Height;
            newNodeUIElement.Tag = "n";

            return newNodeUIElement;
        }

        UIElement CreateNodeTitleUI(Node node) {
            var titleUIElement = new TextBlock()
            {
                Text = node.Title,
                FontSize = 14,
                Foreground = new SolidColorBrush(Colors.LightGray)
            };
            titleUIElement.SetValue(Canvas.LeftProperty, 0);
            titleUIElement.SetValue(Canvas.TopProperty, -25);
            return titleUIElement;
        }

        UIElement CreateSlotUI(string tag, Node node, int slotIndex, double slotDiameter, double slotRadius, Color slotColor, bool isInputSlot, Color labelColor) {
            var slotPosition = isInputSlot ? node.GetInputSlotPosition(slotIndex) : node.GetOutputSlotPosition(slotIndex);

            var slotContainer = new Canvas()
            {
                Name = $"{tag}_{node.Key}_{slotIndex}",
                Width = slotDiameter,
                Height = slotDiameter,
                Tag = tag,
                IsHitTestVisible = true
            };
            slotContainer.SetValue(Canvas.LeftProperty, slotPosition.X - slotRadius - node.PositionX);
            slotContainer.SetValue(Canvas.TopProperty, slotPosition.Y - slotRadius - node.PositionY);
            
            //dot
            var newSlotUIElement = new Windows.UI.Xaml.Shapes.Ellipse()
            {
                //Name = $"{tag}_{node.Key}_{slotIndex}",
                Width = slotDiameter,
                Height = slotDiameter,
                Fill = new SolidColorBrush(slotColor),
                //Tag = tag
            };
            slotContainer.Children.Add(newSlotUIElement);

            //text
            var labels = new string[0];
            if (isInputSlot) labels = string.IsNullOrEmpty(node.InputSlotLabels) ? new string[0] : node.InputSlotLabels.Split(",".ToCharArray());
            else labels = string.IsNullOrEmpty(node.OutputSlotLabels) ? new string[0] : node.OutputSlotLabels.Split(",".ToCharArray());

            if (labels.Length > 0 && labels.Length >= slotIndex)
            {
                var labelText = labels[slotIndex];
                if (!string.IsNullOrEmpty(labelText))
                {
                    var label = new TextBlock()
                    {
                        Text = labelText,
                        Foreground = new SolidColorBrush(labelColor),
                        FontSize = 12
                    };
                    //label.HorizontalAlignment = HorizontalAlignment.Left;
                    slotContainer.Children.Add(label);

                    label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity)); // framework to calculate width/height of label so i can use ActualWidth
                    label.Margin = new Thickness(isInputSlot ? ((label.ActualWidth * -1) - 10) : 20, -4, 0, 0);
                    //label.Arrange(new Rect(textBlock.DesiredSize));
                    
                }
            }
            
            
            return slotContainer;
        }
        
        (bool ElementFound, UIElement Element) HasNodeAlreadyBeenRendered(string id)
        {
            var foundUIElement = (UIElement)_uiNodeGraphPanelXamlRoot.FindName(id);
            if (foundUIElement != null) return (true, foundUIElement);
            return (false, null);
        }


        bool TryUpdateExistingNode(NodeNodeLinkModel nodeNodeLinkModel) {
            var findingUIElement = HasNodeAlreadyBeenRendered(nodeNodeLinkModel.Node.Key);
            if (findingUIElement.ElementFound && nodeNodeLinkModel != null && nodeNodeLinkModel.Node != null)
            {
                UpdateNodeVisual(nodeNodeLinkModel, findingUIElement.Element);
            }
            return false;
        }



        // ==========
        // NODE-SLOT-LINK
        // ==========
        public void RenderNodeSlotLink(NodeLink nodeLink)
        {
            Node inputNode = FindNode(nodeLink.InputNodeKey);
            Node outputNode = FindNode(nodeLink.OutputNodeKey);

            InputSlotPosition inputSlotPosition = inputNode.GetInputSlotPosition(nodeLink.InputSlotIndex);
            OutputSlotPosition outputSlotPosition = outputNode.GetOutputSlotPosition(nodeLink.OutputSlotIndex);

            // find node-slot-link if it exists and then update it
            var foundUIElement = _uiNodeGraphPanelXamlRoot.FindName(nodeLink.UniqueName);
            if (TryUpdateExistingNodeSlotLink(nodeLink.UniqueName, inputSlotPosition, outputSlotPosition)) return;
            
            //create new node-slot-link
            CreateNewNodeSlotLink(nodeLink.UniqueName, inputSlotPosition, outputSlotPosition, Colors.Orange);
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
            foreach (var child in _uiNodeGraphPanelXamlRoot.Children) {
                if (child is INodeTypeComponent) {
                    var nodeTypeComponent = (INodeTypeComponent)child;
                    nodeTypeComponent.NodeTypeValueChanged -= NodeTypeComponent_NodeTypeValueChanged;
                }
                var fe = (FrameworkElement)child;
                if (fe.DataContext != null) {
                    fe.DataContext = null;
                }
            }
            _uiNodeGraphPanelXamlRoot.Children.Clear();
        }




        // ==========
        // PRIVATE
        // ==========

        private void NodeTypeComponent_NodeTypeValueChanged(object sender, System.EventArgs e)
        {
            if (sender != null) {
                var nodeKeyToUpdate = sender as string;
                var foundNode = FindNode(nodeKeyToUpdate);
                if (foundNode != null) {
                    var foundLinks = _links.Where(x => x.InputNodeKey == foundNode.Key || x.OutputNodeKey == foundNode.Key).ToList();
                    var nodeNodeLinkVM = new NodeNodeLinkModel()
                    {
                        Node = foundNode,
                        InputNodeLinks = foundLinks.Where(x => x.InputNodeKey == foundNode.Key).OrderBy(x => x.InputSlotIndex).ToList(),
                        OutputNodeLinks = foundLinks.Where(x => x.OutputNodeKey == foundNode.Key).OrderBy(x => x.OutputSlotIndex).ToList()
                    };
                    TryUpdateExistingNode(nodeNodeLinkVM);
                }
            }
        }
    }
}
