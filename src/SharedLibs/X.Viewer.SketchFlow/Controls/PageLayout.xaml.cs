using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace X.Viewer.SketchFlow.Controls
{
    public sealed partial class PageLayout : UserControl
    {
        public event EventHandler PerformAction;

        public PageLayout()
        {
            this.InitializeComponent();
        }

        private void pgLayer_LayerChanged(object sender, EventArgs e)
        {
            var page = this.DataContext as SketchPage;
            page.ExternalPC("Layers");

        }



        private void grdPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var nm = (string)((Grid)sender).Tag;
            PerformAction?.Invoke(this, new PageLayoutEventArgs() { ActionType = nm + "PageLayoutStarted", StartPoint = e.GetCurrentPoint(null) });
        }

        private void grdPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var nm = (string)((Grid)sender).Tag;
            PerformAction?.Invoke(this, new PageLayoutEventArgs() { ActionType = nm + "PageLayoutFinished", StartPoint = e.GetCurrentPoint(null) });
        }

        private void grdPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ((Grid)sender).Opacity = 1;
        }

        private void grdPointerExited(object sender, PointerRoutedEventArgs e)
        {
            ((Grid)sender).Opacity = 0.5;
        }
    }

    public class PageLayoutEventArgs: EventArgs
    {
        public string ActionType;
        public PointerPoint StartPoint;
    }
}
