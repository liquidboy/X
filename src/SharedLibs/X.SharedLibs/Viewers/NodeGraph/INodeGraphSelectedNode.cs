using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    // this knows about rendering technology BUT is limited to the SELECTED NODE
    public interface INodeGraphSelectedNode
    {
        void SetSelectedNode(Point point);
        void ClearSelectedNode();
        void MoveSelectedNode(Vector2 distanceToMove, double scale);   
    }
}
