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

        public void SetSelectedSlot(Point point) {
            if (!string.IsNullOrEmpty(_selectedSlotNodeKey)) return; //only one slot can be selected at any given time for now

            var slotUnderPoint = TryToFindSlotUnderPoint(point);
            if (slotUnderPoint.FoundSlot) {
                var nameParts = slotUnderPoint.SlotElement.Name.Split("_");
                _selectedSlotIsInputSlot = slotUnderPoint.SlotElement.Tag.Equals("nsi");
                _selectedSlotNodeKey = nameParts[1];
                _selectedSlotIndex = int.Parse(nameParts[2]);
                //_selectedSlotStartDragPosition = new Point((double)uiCurrentFocusedNode.GetValue(Canvas.LeftProperty), (double)uiCurrentFocusedNode.GetValue(Canvas.TopProperty));
                var foundNode = FindNode(_selectedSlotNodeKey);
                _selectedSlotStartDragPosition = _selectedSlotIsInputSlot ? foundNode.GetInputSlotPosition(_selectedSlotIndex) : foundNode.GetOutputSlotPosition(_selectedSlotIndex);

                StartGhostLink(_selectedSlotStartDragPosition);
            }
        }

        public void ClearSelectedSlot(Point point) {
            _selectedSlotNodeKey = string.Empty;
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
        public void CompleteGhostLink(Point point) {
            var slotUnderPoint = TryToFindSlotUnderPoint(point);
            if (slotUnderPoint.FoundSlot) {
                var nameParts = slotUnderPoint.SlotElement.Name.Split("_");
                var slotNodeKey = nameParts[1];
                var slotIndex = int.Parse(nameParts[2]);
                var nodeLink = _selectedSlotIsInputSlot ? 
                    new NodeLink(slotNodeKey, slotIndex, _selectedSlotNodeKey, _selectedSlotIndex): 
                    new NodeLink(_selectedSlotNodeKey, _selectedSlotIndex, slotNodeKey, slotIndex);
                AddLinkToGraph(nodeLink);
                RenderNodeSlotLink(nodeLink);
            }
        }
        public void UpdateGhostLink(Point startPosition, double movedX, double movedY) {
            Debug.WriteLine($"slot update ghost startPosition : {startPosition} movedx: {movedX} movedy: {movedY}");
            
            if(_selectedSlotIsInputSlot) TryUpdateExistingNodeSlotLink("gl", startPosition, new OutputSlotPosition(movedX, movedY));
            else TryUpdateExistingNodeSlotLink("gl", new InputSlotPosition(movedX, movedY), startPosition);
        }

        private (bool FoundSlot, FrameworkElement SlotElement) TryToFindSlotUnderPoint(Point point) {
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement &&
                    ((FrameworkElement)x).Tag != null &&
                    (((FrameworkElement)x).Tag.ToString().Equals("nsi") || ((FrameworkElement)x).Tag.ToString().Equals("nso")));
                if (foundNC != null && foundNC.Count() > 0)
                {
                    return (true, (FrameworkElement)foundNC.FirstOrDefault());
                }
            }
            return (false, null);
        }
    }
}
