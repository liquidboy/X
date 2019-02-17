using System.Numerics;
using Windows.Foundation;

namespace X.Viewer.NodeGraph
{
    // this knows about rendering technology BUT is limited to the SELECTED NODE
    public interface INodeGraphHover
    {
        void HoverOverNodeGraph(Point currentPosition, double scale);   
    }
}
