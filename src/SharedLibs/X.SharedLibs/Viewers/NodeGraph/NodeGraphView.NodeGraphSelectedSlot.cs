using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.Foundation;
using System.Numerics;
using System.Diagnostics;
using Windows.UI;
using X.UI.NodeGraph;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelectedSlot
    {
        Point _selectedSlotStartDragPosition;
        string _selectedSlotNodeKey;
        int _selectedSlotIndex;
        bool _selectedSlotIsInputSlot;
        
        public bool IsSlotSelected => !string.IsNullOrEmpty(_selectedSlotNodeKey);

        public void SetSelectedSlot(Point point) {
            if (IsSlotSelected) return; //only one slot can be selected at any given time for now

            var slotUnderPoint = TryToFindSlotUnderPoint(point);
            if (slotUnderPoint.FoundSlot) {
                var nameParts = slotUnderPoint.SlotName.Split("_");
                _selectedSlotIsInputSlot = slotUnderPoint.SlotTag.Equals("nsi");
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
            CreateNewNodeSlotLink("gl", startPosition, startPosition, Colors.LightGray);
        }
        public void CompleteGhostLink(Point point) {
            var slotUnderPoint = TryToFindSlotUnderPoint(point);
            if (slotUnderPoint.FoundSlot) {
                var nameParts = slotUnderPoint.SlotName.Split("_");
                var slotNodeKey = nameParts[1];
                if (_selectedSlotNodeKey == slotNodeKey) { ClearGhostLink(); return; }
                var slotIndex = int.Parse(nameParts[2]);
                var nodeLink = _selectedSlotIsInputSlot ? 
                    new NodeLink(slotNodeKey, slotIndex, _selectedSlotNodeKey, _selectedSlotIndex, SelectedGraphGuid) : 
                    new NodeLink(_selectedSlotNodeKey, _selectedSlotIndex, slotNodeKey, slotIndex, SelectedGraphGuid);
                AddLinkToGraph(nodeLink);
                RenderNodeSlotLink(nodeLink);
            }
        }
        public void ClearGhostLink() {
            TryRemoveNodeSlotLink("gl");
        }
        public void UpdateGhostLink(Point startPosition, double movedX, double movedY) {
            Debug.WriteLine($"slot update ghost startPosition : {startPosition} movedx: {movedX} movedy: {movedY}");
            
            if(_selectedSlotIsInputSlot) TryUpdateExistingNodeSlotLink("gl", startPosition, new OutputSlotPosition(movedX, movedY));
            else TryUpdateExistingNodeSlotLink("gl", new InputSlotPosition(movedX, movedY), startPosition);
        }
    }
}
