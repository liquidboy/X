using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphSelectedNodeLink
    {
        bool IsNodeLinkSelected { get; }
        void SetSelectedNodeLink(Point point);
        void ClearSelectedNodeLink(Point point);
    }
}
