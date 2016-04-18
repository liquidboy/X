using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace X.Viewer.SketchFlow.Controls
{
    public sealed partial class PageLayers : UserControl
    {
        public event EventHandler LayerChanged;

        public PageLayers()
        {
            this.InitializeComponent();
        }

        private void butEnableDisable_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pl = ((FrameworkElement)sender).DataContext as PageLayer;
            pl.IsEnabled = !pl.IsEnabled;
            //LayerChanged?.Invoke(null, EventArgs.Empty);
            LayerChanged?.Invoke(null, new PageLayerEventArgs() { ActionType =  pl.IsEnabled? "EnableLayer": "DisableLayer" });
        }

        private void butEdit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pl = ((FrameworkElement)sender).DataContext as PageLayer;
            LayerChanged?.Invoke(null, new PageLayerEventArgs() { ActionType = "EditLayer", Layer = pl });
        }
    }


    public class PageLayerEventArgs : EventArgs
    {
        public string ActionType;
        public PageLayer Layer;
    }
}
