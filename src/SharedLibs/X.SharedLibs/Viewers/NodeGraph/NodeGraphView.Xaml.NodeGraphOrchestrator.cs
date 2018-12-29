using Windows.Foundation;
using System.Numerics;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphOrchestrator
    {
        bool _shouldStopPropogatingPointerMoved;

        public void PointingStarted(Point point) {
            _shouldStopPropogatingPointerMoved = false;
            SetSelectedSlot(point);
            SetSelectedNode(point);
        }

        public void PointingCompleted(Point point) {
            if (!string.IsNullOrEmpty(_selectedSlotNodeKey))
            {
                CompleteGhostLink(point);
                ClearSelectedSlot(point);
            }

            if (!string.IsNullOrEmpty(_selectedNodeKey))
            {
                ClearSelectedNode();
            }

            _shouldStopPropogatingPointerMoved = false;
        }

        public void PointerUpdated(Vector2 distanceToMove, double scale) {
            _shouldStopPropogatingPointerMoved = false;

            if (!string.IsNullOrEmpty(_selectedSlotNodeKey))
            {
                //join slots
                MoveSelectedSlot(distanceToMove, nodeGraphZoomContainer.Scale);
                _shouldStopPropogatingPointerMoved = true;
            }
            else if (!string.IsNullOrEmpty(_selectedNodeKey))
            {
                //move node
                MoveSelectedNode(distanceToMove, nodeGraphZoomContainer.Scale);
                _shouldStopPropogatingPointerMoved = true;
            }
        }
    }
}
