using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphCompositor
    {
        void InitializeCompositor(UIElement rootVisualElement);
        void CreateNodeVisualUI(NodeNodeLinkModel nodeNodeLinkViewModel, UIElement parentRootOfVisual, NodeType nodeType);
        void ClearCompositor();
    }
}
