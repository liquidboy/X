using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphRenderer
    {
        void InitializeRenderer(UIElement uiNodeGraphRoot);
        void RenderNode(Node node);
        void RenderNodeSlotLink(NodeLink nodeLink);
        void SetSelectedNode(Point point);
        void ClearSelectedNode();
        void MoveSelectedNode(Vector2 distanceToMove, double scale);   
    }
}
