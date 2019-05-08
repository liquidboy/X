using Windows.Foundation;

namespace X.UI.NodeGraph
{
    public interface INodeGraphSelectedNodeLink
    {
        bool IsNodeLinkSelected { get; }
        bool SetSelectedNodeLink(Point point, bool currentPointingStarted);
        void ClearSelectedNodeLink();
    }
}
