using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Numerics;
using System.Diagnostics;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelectedSlot
    {
        Point _selectedSlotStartDragPosition;
        string _selectedSlotKey;
        bool _selectedSlotIsInputSlot;

        public void SetSelectedSlot(Point point) {
            if (!string.IsNullOrEmpty(_selectedSlotKey)) return; //only one slot can be selected at any given time for now
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement && 
                    ((FrameworkElement)x).Tag != null && 
                    (((FrameworkElement)x).Tag.ToString().Equals("nsi") || ((FrameworkElement)x).Tag.ToString().Equals("nso")));
                if (foundNC != null && foundNC.Count() > 0) {
                    var uiCurrentFocusedNode = (FrameworkElement)foundNC.FirstOrDefault();
                    _selectedSlotIsInputSlot = uiCurrentFocusedNode.Tag.Equals("nsi");
                    if(_selectedSlotIsInputSlot)
                        _selectedSlotStartDragPosition = new InputSlotPosition((double)uiCurrentFocusedNode.GetValue(Canvas.LeftProperty), (double)uiCurrentFocusedNode.GetValue(Canvas.TopProperty));
                    else
                        _selectedSlotStartDragPosition = new OutputSlotPosition((double)uiCurrentFocusedNode.GetValue(Canvas.LeftProperty), (double)uiCurrentFocusedNode.GetValue(Canvas.TopProperty));
                    _selectedSlotKey = uiCurrentFocusedNode.Name;
                    ShowGhostNodeSlotLine();
                }
            }
        }

        public void ClearSelectedSlot() {
            _selectedSlotKey = string.Empty;
        }


        public void MoveSelectedSlot(Vector2 distanceToMove, double scale)
        {
            Debug.WriteLine($"vector distance : {distanceToMove}  scale : {scale} ");
        }

        private void ShowGhostNodeSlotLine() {

        }
    }
}
