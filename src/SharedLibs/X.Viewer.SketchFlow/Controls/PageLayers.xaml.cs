using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
            LayerChanged?.Invoke(null, new PageLayerEventArgs() { ActionType = pl.IsEnabled ? "EnableLayer" : "DisableLayer" });
        }


        Stack<int> _AvailableLayerNumbers = new Stack<int>(new List<int> { 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });

        int noInEditMode = 0;
        private void butEdit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var but = (FrameworkElement)sender as Border;
            var butCont = but.Child as TextBlock;
            var pl = ((FrameworkElement)sender).DataContext as PageLayer;
            var sp = this.DataContext as SketchPage;
            
            pl.IsExpanded = !pl.IsExpanded;
            if (pl.IsExpanded)
            {
                but.Background = new SolidColorBrush(Colors.DarkOrange);
                sp.Left += 55;
                var layerNo = _AvailableLayerNumbers.Pop().ToString();
                pl.LayerNumber = layerNo.ToString();
                butCont.Text = layerNo.ToString();
                noInEditMode++;
            }
            else
            {
                but.Background = new SolidColorBrush(Colors.Gray);
                sp.Left -= 55;
                _AvailableLayerNumbers.Push( int.Parse(pl.LayerNumber));
                butCont.Text = string.Empty;
                pl.LayerNumber = string.Empty;
                noInEditMode--;
            }
            layoutRoot.Margin = new Thickness(-1 * (55 * (noInEditMode + 1)), 0, 0, 0);
            LayerChanged?.Invoke(null, new PageLayerEventArgs() { ActionType = "EditLayer", Layer = pl });
            sp.ExternalPC("ExpandedLayers");
        }

        private void butXamlFragLayer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var but = (FrameworkElement)sender as Border;
            var xf = ((FrameworkElement)sender).DataContext as XamlFragment;
            xf.IsEnabled = !xf.IsEnabled;
            LayerChanged?.Invoke(null, new PageLayerEventArgs() { ActionType = xf.IsEnabled ? "EnableXamlFragment" : "DisableXamlFragment" });
        }
    }


    public class PageLayerEventArgs : EventArgs
    {
        public string ActionType;
        public PageLayer Layer;
    }
}
