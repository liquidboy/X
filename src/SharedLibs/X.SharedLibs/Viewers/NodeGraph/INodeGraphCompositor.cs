using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphCompositor
    {
        void InitializeCompositor(UIElement rootVisualElement);
        void CreateNodeVisual(string nodeKey, UIElement parentRootOfVisual, NodeType nodeType);
        void ClearCompositor();
    }
}
