using Windows.UI.Xaml;
using X.UI.NodeGraph;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphCompositor
    {
        void InitializeCompositor(UIElement rootVisualElement);
        void CreateNodeVisualUI(NodeNodeLinkModel nodeNodeLinkViewModel, UIElement parentRootOfVisual, NodeType nodeType);
        void ClearCompositor();
        void UpdateNodeVisual(NodeNodeLinkModel nodeNodeLinkModel, UIElement parentRootOfVisual);
    }
}
