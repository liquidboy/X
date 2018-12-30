using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NodePosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Numerics;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelectedNode
    {
        Point _selectedNodeStartDragPosition;
        string _selectedNodeKey;

        public bool IsNodeSelected => !string.IsNullOrEmpty(_selectedNodeKey);

        public void SetSelectedNode(Point point) {
            if (IsSlotSelected) return; // short circuit if SLOT is selected

            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement && 
                    ((FrameworkElement)x).Tag != null && ((FrameworkElement)x).Tag.ToString().Equals("nc"));
                if (foundNC != null && foundNC.Count() > 0) {
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
            foundNodeUIElement.SetValue(Canvas.LeftProperty, updatedNode.PositionX);
            foundNodeUIElement.SetValue(Canvas.TopProperty, updatedNode.PositionY);

            //update node-slot-links positions between the node-slots
            DrawNodeSlotLink(_selectedNodeKey);

        }
    }
}
