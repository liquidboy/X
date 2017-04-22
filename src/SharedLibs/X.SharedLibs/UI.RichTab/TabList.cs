using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace X.UI.RichTab
{
    public sealed class TabList : Control
    {

        StackPanel _grdTabs;
        StackPanel _grdLockedTabs;
        ScrollViewer _svTabs;

        public event PointerEventHandler TabPointerExited;
        public event PointerEventHandler TabPointerEntered;


        public TabList()
        {
            this.DefaultStyleKey = typeof(TabList);

            this.Loaded += TabList_Loaded;
            this.Unloaded += TabList_Unloaded;
        }

        private void TabList_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void TabList_Loaded(object sender, RoutedEventArgs e)
        {

        }


        protected override void OnApplyTemplate()
        {
            if (_grdTabs == null) {
                _grdTabs = GetTemplateChild("grdTabs") as StackPanel;
                _grdTabs.PointerWheelChanged += _grdTabs_PointerWheelChanged;
            }

            if (_grdLockedTabs == null)
            {
                _grdLockedTabs = GetTemplateChild("grdLockedTabs") as StackPanel;
            }

            if (_svTabs == null) {
                _svTabs = GetTemplateChild("svTabs") as ScrollViewer;
            }
            


            base.OnApplyTemplate();
        }

        private void _grdTabs_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            //var delta = 25;
            var change = e.GetCurrentPoint(null).Properties.MouseWheelDelta;
            if (change > 0)
            {
                //svTabs.ChangeView(svTabs.HorizontalOffset + delta, svTabs.VerticalOffset, svTabs.ZoomFactor);
                _svTabs.ScrollToHorizontalOffset(_svTabs.HorizontalOffset + 50);
            }
            if (change < 0)
            {
                //svTabs.ChangeView(svTabs.HorizontalOffset - delta, svTabs.VerticalOffset, svTabs.ZoomFactor);
                _svTabs.ScrollToHorizontalOffset(_svTabs.HorizontalOffset - 50);
            }
        }


        public void RaiseTabPointerExited(object sender, PointerRoutedEventArgs e)
        {

            if (TabPointerExited != null) TabPointerExited(sender, e);
        }

        public void RaiseTabPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (TabPointerEntered != null) TabPointerEntered(sender, e);
        }




    }
}
