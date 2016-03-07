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
    public sealed class TabButton : Control
    {
        Button _butTab;





        public TabButton()
        {
            this.DefaultStyleKey = typeof(TabButton);

            this.Loaded += TabButton_Loaded;
            this.Unloaded += TabButton_Unloaded;
        }

        private void TabButton_Unloaded(object sender, RoutedEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void TabButton_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnApplyTemplate()
        {


            if (_butTab == null) {
                _butTab = GetTemplateChild("butTab") as Button;
                _butTab.PointerEntered += _butTab_PointerEntered;
                _butTab.PointerExited += _butTab_PointerExited;
            }


            base.OnApplyTemplate();
        }
        

        private void _butTab_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //get TabList parent and call RaiseTabPointerEntered 
            var p1 = GetTabListParent((DependencyObject)sender);
            p1.RaiseTabPointerExited(sender, e);
        }

        private void _butTab_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            //get TabList parent and call RaiseTabPointerEntered 
            var p1 = GetTabListParent((DependencyObject)sender);
            p1.RaiseTabPointerEntered(sender, e);
            
        }

        private TabList GetTabListParent(DependencyObject ctrl)
        {

            TabList found = null;
            DependencyObject current = ctrl;
            while (found == null)
            {
                current = VisualTreeHelper.GetParent(current);
                if (current is TabList) found = (TabList)current;
            }
            return found;
        }
    }
}
