using System.Numerics;
using Windows.Foundation;

namespace X.Viewer.NodeGraph
{
    // this orchestrates events between dependent components
    // - the "selected" events between nodes/slots
    public interface INodeGraphOrchestrator
    {
        void PointingStarted(Point point);
        void PointingCompleted(Point point);
        void PointerUpdated(Vector2 distanceToMove, double scale);
    }
}
