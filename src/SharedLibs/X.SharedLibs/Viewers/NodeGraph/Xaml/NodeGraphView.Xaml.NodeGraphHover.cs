using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NodePosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Numerics;
using X.UI.NodeGraph;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphHover
    {

        FrameworkElement _currentNodeHovering;

        public void HoverOverNodeGraph(Point currentPosition, double scale)
        {
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(currentPosition, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement &&
                    ((FrameworkElement)x).Tag != null && ((FrameworkElement)x).Tag.ToString().Equals("n"));
                if (foundNC != null && foundNC.Count() > 0)
                {
                    if (_currentNodeHovering != null) _currentNodeHovering.Shadow = null;
                    _currentNodeHovering = (FrameworkElement)foundNC.FirstOrDefault();
                    _currentNodeHovering.Shadow = sharedShadow;
                }
            }
            else {
                if (_currentNodeHovering != null) _currentNodeHovering.Shadow = null;
            }
        }
    }
}
