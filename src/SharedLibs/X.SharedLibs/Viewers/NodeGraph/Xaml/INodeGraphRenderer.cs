using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    // this knows about the rendering technology eg. XAML/WEB/DirectX etc
    public interface INodeGraphRenderer
    {
        void InitializeRenderer(UIElement uiNodeGraphRoot);
        void RenderNode(Node node, List<NodeLink> relatedLinks);
        void RenderNodeSlotLink(NodeLink nodeLink);
        void ClearRenderer();
        (bool FoundSlot, string SlotName, string SlotTag) TryToFindSlotUnderPoint(Point point);
    }
}
