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
        string _selectedSlotNodeKey;
        int _selectedSlotIndex;
        bool _selectedSlotIsInputSlot;

        NodeLink? _ghostLink;

        public void SetSelectedSlot(Point point) {
            if (!string.IsNullOrEmpty(_selectedSlotNodeKey)) return; //only one slot can be selected at any given time for now
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement && 
                    ((FrameworkElement)x).Tag != null && 
                    (((FrameworkElement)x).Tag.ToString().Equals("nsi") || ((FrameworkElement)x).Tag.ToString().Equals("nso")));
                if (foundNC != null && foundNC.Count() > 0) {
                    var uiCurrentFocusedNode = (FrameworkElement)foundNC.FirstOrDefault();
                    var nameParts = uiCurrentFocusedNode.Name.Split("_");
                    _selectedSlotIsInputSlot = uiCurrentFocusedNode.Tag.Equals("nsi");
                    _selectedSlotNodeKey = nameParts[1];
                    _selectedSlotIndex = int.Parse(nameParts[2]);
                    //_selectedSlotStartDragPosition = new Point((double)uiCurrentFocusedNode.GetValue(Canvas.LeftProperty), (double)uiCurrentFocusedNode.GetValue(Canvas.TopProperty));
                    var foundNode = FindNode(_selectedSlotNodeKey);
                    _selectedSlotStartDragPosition = _selectedSlotIsInputSlot ? foundNode.GetInputSlotPosition(_selectedSlotIndex) : foundNode.GetOutputSlotPosition(_selectedSlotIndex);
                    
                    StartGhostLink(_selectedSlotStartDragPosition);
                }
            }
        }

        public void ClearSelectedSlot() {
            _selectedSlotNodeKey = string.Empty;
            DestroyGhostLink();
        }

        public void MoveSelectedSlot(Vector2 distanceToMove, double scale)
        {
            Debug.WriteLine($"slot moving ... vector distance : {distanceToMove}  scale : {scale} ");
            UpdateGhostLink(_selectedSlotStartDragPosition, _selectedSlotStartDragPosition.X + (distanceToMove.X / scale), _selectedSlotStartDragPosition.Y + (distanceToMove.Y / scale));
        }




        public void StartGhostLink(Point startPosition) {
            Debug.WriteLine($"slot start ghost startPosition : {startPosition}");
            CreateNewNodeSlotLink("gl", startPosition, startPosition);
        }
        public void CompleteGhostLink() {
            // todo : create nodelink in nodelink list and redraw graph
            DestroyGhostLink();
        }
        public void DestroyGhostLink() { _ghostLink = null; }
        public void UpdateGhostLink(Point startPosition, double movedX, double movedY) {
            Debug.WriteLine($"slot update ghost startPosition : {startPosition} movedx: {movedX} movedy: {movedY}");
            
            if(_selectedSlotIsInputSlot) TryUpdateExistingNodeSlotLink("gl", startPosition, new OutputSlotPosition(movedX, movedY));
            else TryUpdateExistingNodeSlotLink("gl", new InputSlotPosition(movedX, movedY), startPosition);
        }
    }
}
