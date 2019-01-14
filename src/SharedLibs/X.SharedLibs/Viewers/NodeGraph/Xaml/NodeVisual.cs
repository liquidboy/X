using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public class NodeVisual
    {
        public string NodeKey { get; set; }
        public FrameworkElement AssociatedObject;
        public CompositionBrush Brush;
    }
}
