using System.Numerics;
using Windows.Foundation;

namespace X.UI.NodeGraph
{
    // this knows about rendering technology BUT is limited to the SELECTED NODE
    public interface INodeGraphHover
    {
        void HoverOverNodeGraph(Point currentPosition, double scale);   
    }
}
