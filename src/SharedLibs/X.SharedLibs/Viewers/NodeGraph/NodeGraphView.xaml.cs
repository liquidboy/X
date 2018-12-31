using System;
using System.Numerics;
using Windows.UI.Input;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

namespace X.Viewer.NodeGraph
{
    public sealed partial class NodeGraphView : UserControl, IContentView
    {
        public event EventHandler<ContentViewEventArgs> SendMessage;

        bool IsPointerDown = false;
        

        public NodeGraphView()
        {
            InitializeComponent();
        }

        public void Load()
        {
            //var ct = nodeGraphCanvas.RenderTransform as CompositeTransform;
            //nodeGraphZoomContainer.Scale = ct.ScaleX;
            InitializeStorage();
            InitializeRenderer(nodeGraphCanvas);
            InitializeNodeGraph();
            InitializeGraphSelector(RetrieveGraphs());           
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
            IsPointerDown = true;
            ptStart = e.GetCurrentPoint(null);
            PointingStarted(ptStart.Position);
        }

        private void layoutRoot_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsPointerDown = false;

            ptDifXStart = ptDifX;
            ptDifYStart = ptDifY;

            PointingCompleted(e.GetCurrentPoint(null).Position);
        }
        
        double ptDifX = 0;
        double ptDifY = 0;


        private void layoutRoot_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var ptEnd = e.GetCurrentPoint(null);
            
            if (!IsPointerDown) return;

            //moving artboard
            var ct = nodeGraphCanvas.RenderTransform as CompositeTransform;
            
            Debug.WriteLine($"board cursor position : {e.GetCurrentPoint(null).RawPosition}");

            //process event by node graph 
            var distanceBetween2Points = Vector2.Subtract(ptEnd.Position.ToVector2(), ptStart.Position.ToVector2());
            PointerUpdated(distanceBetween2Points, nodeGraphZoomContainer.Scale);
            if (_shouldStopPropogatingPointerMoved) return;
            
            //move board
            ptDifX = ptDifXStart + ((ptStart.Position.X - ptEnd.Position.X) / nodeGraphZoomContainer.Scale);
            ptDifY = ptDifYStart + ((ptStart.Position.Y - ptEnd.Position.Y) / nodeGraphZoomContainer.Scale);
                
            ct.TranslateX = -1 * ptDifX;
            ct.TranslateY = -1 * ptDifY;
        }






        private void ButClear_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ClearBoard();
        }

        private void ButSave_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (IsGraphSelected) SaveGraph(_selectedGraph);
            else {
                //create new graph
            }
        }

        private void ButLoad_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!IsGraphSelected) GraphSelected(((Guid)cbSavedGraphs.SelectedValue).ToString());
        }

        private void ButClearStorage_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ClearStorage();
        }

    }


}


//http://plnkr.co/edit/II6lgj511fsQ7l0QCoRi?p=preview  <== i used this in the end
//http://stackoverflow.com/questions/2916081/zoom-in-on-a-point-using-scale-and-translate