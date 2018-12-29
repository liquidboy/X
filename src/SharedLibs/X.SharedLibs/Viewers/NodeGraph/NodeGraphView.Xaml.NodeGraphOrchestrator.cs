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
            if (IsSlotSelected)
            {
                CompleteGhostLink(point);
                ClearGhostLink();
                ClearSelectedSlot(point);
            }

            if (IsNodeSelected)
            {
                ClearSelectedNode();
            }

            _shouldStopPropogatingPointerMoved = false;
        }

        public void PointerUpdated(Vector2 distanceToMove, double scale) {
            _shouldStopPropogatingPointerMoved = false;

            if (IsSlotSelected)
            {
                //join slots
                MoveSelectedSlot(distanceToMove, nodeGraphZoomContainer.Scale);
                _shouldStopPropogatingPointerMoved = true;
            }
            else if (IsNodeSelected)
            {
                //move node
                MoveSelectedNode(distanceToMove, nodeGraphZoomContainer.Scale);
                _shouldStopPropogatingPointerMoved = true;
            }
        }
    }
}
