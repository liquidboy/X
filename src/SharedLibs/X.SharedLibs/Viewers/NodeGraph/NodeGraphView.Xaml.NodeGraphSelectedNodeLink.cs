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
using System;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelectedNodeLink
    {
        string _selectedNodeLinkKey;

        public bool IsNodeLinkSelected => !string.IsNullOrEmpty(_selectedNodeLinkKey);

        public bool SetSelectedNodeLink(OutputSlotPosition point, bool currentPointingStarted)
        {
            var returnPointingStarted = currentPointingStarted;
            if (IsSlotSelected) return returnPointingStarted; // short circuit if SLOT is selected
            if (IsNodeSelected) return returnPointingStarted; // short circuit if SLOT is selected
            if (IsNodeLinkSelected) return returnPointingStarted; //only one slot can be selected at any given time for now

            var nodeLinkUnderPoint = TryToFindNodeLinkUnderPoint(point);
            if (nodeLinkUnderPoint.FoundNodeLink)
            {
                var foundNodeLink = FindNodeLink(nodeLinkUnderPoint.SlotElement.Name);
                if (foundNodeLink != null)
                {
                    returnPointingStarted = false; //we dont want the board to think the pointer was pressed
                    _selectedNodeLinkKey = nodeLinkUnderPoint.SlotElement.Name;

                    var menuFlyout = new MenuFlyout();
                    var m = new MenuFlyoutItem() { Text = "Delete", Tag = foundNodeLink.UniqueId }; m.Click += MenuFlyoutItem_Click; menuFlyout.Items.Add(m);
                    menuFlyout.Closed += MenuFlyout_Closed;
                    nodeLinkUnderPoint.SlotElement.ContextFlyout = menuFlyout;
                    var boardTraslation = GetBoardTranslation();
                    var boardScale = GetBoardScale();
                    var gt = nodeLinkUnderPoint.SlotElement.TransformToVisual(GetBoard());
                    var tp = gt.TransformPoint(point);
                    Debug.WriteLine($"board scale : {boardScale}");
                    Debug.WriteLine($"board translation : {boardTraslation}");
                    Debug.WriteLine($"cursor point: {point}");
                    Debug.WriteLine($"link translation point: {tp}");
                    menuFlyout.ShowAt(nodeLinkUnderPoint.SlotElement, new InputSlotPosition(Math.Abs(tp.X) - ( boardTraslation.X/ boardScale), Math.Abs(tp.Y) - (boardTraslation.Y / boardScale)));
                }
            }

            return returnPointingStarted;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuFlyoutItem)sender;
            Debug.WriteLine($"menu clicked: {menuItem.Text} - { menuItem.Tag }");
            switch (menuItem.Text) {
                case "Delete":
                    MarkNodeLinkForDeletion((Guid)menuItem.Tag);
                    break;
            }

        }

        private void MenuFlyout_Closed(object sender, object e)
        {
            Debug.WriteLine($"menu closed");
            var menuFlyout = (MenuFlyout)sender;
            menuFlyout.Closed -= MenuFlyout_Closed;

            foreach (MenuFlyoutItem mi in menuFlyout.Items)  mi.Click -= MenuFlyoutItem_Click;
            
            ClearSelectedNodeLink();
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
        
        public void ClearSelectedNodeLink()
        {
            _selectedNodeLinkKey = string.Empty;
        }

    }
}
