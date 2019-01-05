using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InputSlotPosition = Windows.Foundation.Point;
using OutputSlotPosition = Windows.Foundation.Point;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Numerics;
using System.Diagnostics;
using Windows.UI;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelectedNodeLink
    {
        string _selectedNodeLinkKey;

        public bool IsNodeLinkSelected => !string.IsNullOrEmpty(_selectedNodeLinkKey);

        public void ClearSelectedNodeLink(OutputSlotPosition point)
        {
            throw new System.NotImplementedException();
        }

        public void SetSelectedNodeLink(OutputSlotPosition point)
        {
            if (IsSlotSelected) return; // short circuit if SLOT is selected
            if (IsNodeSelected) return; // short circuit if SLOT is selected
            if (IsNodeLinkSelected) return; //only one slot can be selected at any given time for now

            var nodeLinkUnderPoint = TryToFindNodeLinkUnderPoint(point);
            if (nodeLinkUnderPoint.FoundNodeLink)
            {
                //var nameParts = nodeLinkUnderPoint.SlotElement.Name.Split("_");
                //var slotNodeKeyStart = nameParts[1];
                //var slotIndexStart = int.Parse(nameParts[2]);
                //var slotNodeKeyEnd = nameParts[3];
                //var slotIndexEnd = int.Parse(nameParts[4]);

                var foundNodeLink = FindNodeLink(nodeLinkUnderPoint.SlotElement.Name);
                if (foundNodeLink != null) foundNodeLink.DeleteIt = true;
            }
        }

        private (bool FoundNodeLink, FrameworkElement SlotElement) TryToFindNodeLinkUnderPoint(Point point)
        {
            var foundElementsUnderPoint = VisualTreeHelper.FindElementsInHostCoordinates(point, _uiNodeGraphXamlRoot);
            if (foundElementsUnderPoint != null && foundElementsUnderPoint.Count() > 0)
            {
                var foundNC = foundElementsUnderPoint.Where(x => x is FrameworkElement &&
                    ((FrameworkElement)x).Tag != null &&
                    (((FrameworkElement)x).Tag.ToString().Equals("nsl")));
                if (foundNC != null && foundNC.Count() > 0)
                {
                    return (true, (FrameworkElement)foundNC.FirstOrDefault());
                }
            }
            return (false, null);
        }
    }
}
