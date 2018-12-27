using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Input;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using X.Viewer.SketchFlow.Controls;
using X.Viewer.SketchFlow.Controls.Stamps;
using Windows.UI;
using Windows.Foundation;
using X.Services.Data;
using X.UI.ZoomCanvas;

namespace X.Viewer.NodeGraph
{
    public sealed partial class NodeGraphView : UserControl, IContentView
    {
        public event EventHandler<ContentViewEventArgs> SendMessage;

        bool IsMouseDown = false;

        public NodeGraphView()
        {
            this.InitializeComponent();

            this.Loaded += NodeGraphView_Loaded;

        }


        private void NodeGraphView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Load()
        {
            //var ct = nodeGraphCanvas.RenderTransform as CompositeTransform;
            //nodeGraphZoomContainer.Scale = ct.ScaleX;

            //sample node visual
            nodeGraphCanvas.Children.Add(new Windows.UI.Xaml.Shapes.Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Red) });
        }

        public void Unload()
        {
        }


        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var ret = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "stamp_" + ret;
        }

        private void layoutRoot_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            nodeGraphZoomContainer.Zoom(sender, e);
        }

        PointerPoint ptStart;  //artboard moving
        double ptDifXStart = 0;
        double ptDifYStart = 0;
        private void layoutRoot_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsMouseDown = true;
            ptStart = e.GetCurrentPoint(null);


        }

        private void layoutRoot_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsMouseDown = false;
            
            ptDifXStart = ptDifX;
            ptDifYStart = ptDifY;

        }
        
        double ptDifX = 0;
        double ptDifY = 0;


        private void layoutRoot_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            var ptEnd = e.GetCurrentPoint(null);
            
            if (!IsMouseDown) return;

            //moving artboard
            var ct = nodeGraphCanvas.RenderTransform as CompositeTransform;

           
            ptDifX = ptDifXStart + ((ptStart.Position.X - ptEnd.Position.X) / nodeGraphZoomContainer.Scale);
            ptDifY = ptDifYStart + ((ptStart.Position.Y - ptEnd.Position.Y) / nodeGraphZoomContainer.Scale);


            ct.TranslateX = -1 * ptDifX;
            ct.TranslateY = -1 * ptDifY;
            
        }
    }


}


//http://plnkr.co/edit/II6lgj511fsQ7l0QCoRi?p=preview  <== i used this in the end
//http://stackoverflow.com/questions/2916081/zoom-in-on-a-point-using-scale-and-translate