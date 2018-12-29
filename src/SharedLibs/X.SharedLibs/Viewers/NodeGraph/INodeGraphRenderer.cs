using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphRenderer
    {
        void InitializeRenderer(UIElement uiNodeGraphRoot);
        void RenderNode(Node node);
        void RenderNodeSlotLink(NodeLink nodeLink);
    }
}
