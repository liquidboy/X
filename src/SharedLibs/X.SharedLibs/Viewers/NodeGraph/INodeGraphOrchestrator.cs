using System.Numerics;
using Windows.Foundation;

namespace X.Viewer.NodeGraph
{
    // this orchestrates events between dependent components
    // - the "selected" events between nodes/slots
    public interface INodeGraphOrchestrator
    {
        bool PointingStarted(Point point);
        void PointingCompleted(Point point);
        void PointerMovingAndNotPressed(Point currentPoint, double scale);
        void PointerUpdated(Vector2 distanceToMove, double scale);

        bool LoadGraph(string guid);
        SavedGraph SetupExampleGraph(string size);
        void ClearBoard();
    }
}
