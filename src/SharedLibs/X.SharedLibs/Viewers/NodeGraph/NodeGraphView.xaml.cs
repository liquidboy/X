using System;
using System.Numerics;
using Windows.UI.Input;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Diagnostics;
using NodePosition = Windows.Foundation.Point;

namespace X.Viewer.NodeGraph
{
    public sealed partial class NodeGraphView : UserControl, IContentView
    {
        public event EventHandler<ContentViewEventArgs> SendMessage;

        bool IsMouseDown = false;

        public NodeGraphView()
        {
            InitializeComponent();
            Loaded += NodeGraphView_Loaded;
        }


        private void NodeGraphView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Load()
        {
            //var ct = nodeGraphCanvas.RenderTransform as CompositeTransform;
            //nodeGraphZoomContainer.Scale = ct.ScaleX;

            InitializeRenderer(nodeGraphCanvas);
            InitializeNodeGraph();
            SetupExampleNodes();
            //SetupLargeExampleNodes();
        }

        private void SetupLargeExampleNodes()
        {
            //100 failed
            //50 was slow
            //20 was slow but acceptable
            //10 was good

            var dimensionToTest = 10;

            for (int y = 0; y < dimensionToTest; y++)
            {
                for (int x = 0; x < dimensionToTest; x++) {
                    var key = $"Node{x}-{y}";
                    _nodes.Add(key, new Node(key, new NodePosition(x * 200, y * 200), Colors.LightGray, 2, 2));
                }
            }

            DrawNodeGraph();
        }

        private void SetupExampleNodes()
        {
            AddNodeToGraph(new Node("Node1", new NodePosition(100, 100), Colors.Red, 1, 1));
            AddNodeToGraph(new Node("Node2", new NodePosition(100, 300), Colors.Green, 1, 1));
            AddNodeToGraph(new Node("Node3", new NodePosition(400, 190), Colors.Yellow, 2, 2));
            AddNodeToGraph(new Node("Node4", new NodePosition(400, 0), Colors.Purple, 1, 1));
            AddNodeToGraph(new Node("Node5", new NodePosition(700, 100), Colors.Blue, 2, 1));
            AddNodeToGraph(new Node("Node6", new NodePosition(400, 400), Colors.Pink, 1, 2));
            AddNodeToGraph(new Node("Node7", new NodePosition(700, 600), Colors.AliceBlue, 5, 8));
            AddNodeToGraph(new Node("Node8", new NodePosition(700, 1000), Colors.Aquamarine, 3, 1));
            AddNodeToGraph(new Node("Node9", new NodePosition(1000, 500), Colors.Beige, 2, 2));
            AddNodeToGraph(new Node("Node10", new NodePosition(1000, 800), Colors.Bisque, 3, 2));
            AddNodeToGraph(new Node("Node11", new NodePosition(1100, 200), Colors.Brown, 2, 3));
            AddNodeToGraph(new Node("Node12", new NodePosition(1300, 500), Colors.Coral, 2, 2));
            AddNodeToGraph(new Node("Node13", new NodePosition(1300, 700), Colors.DarkGoldenrod, 2, 3));


            AddLinkToGraph(new NodeLink("Node1", 0, "Node3", 0));
            AddLinkToGraph(new NodeLink("Node2", 0, "Node3", 1));
            AddLinkToGraph(new NodeLink("Node4", 0, "Node5", 0));
            AddLinkToGraph(new NodeLink("Node3", 0, "Node5", 0));
            AddLinkToGraph(new NodeLink("Node3", 1, "Node5", 0));
            AddLinkToGraph(new NodeLink("Node6", 0, "Node5", 1));
            AddLinkToGraph(new NodeLink("Node6", 0, "Node7", 0));
            AddLinkToGraph(new NodeLink("Node6", 1, "Node8", 0));
            AddLinkToGraph(new NodeLink("Node5", 0, "Node11", 0));
            AddLinkToGraph(new NodeLink("Node7", 0, "Node11", 1));
            AddLinkToGraph(new NodeLink("Node7", 1, "Node9", 0));
            AddLinkToGraph(new NodeLink("Node7", 2, "Node9", 1));
            AddLinkToGraph(new NodeLink("Node7", 3, "Node10", 0));
            AddLinkToGraph(new NodeLink("Node8", 0, "Node10", 1));
            AddLinkToGraph(new NodeLink("Node10", 0, "Node12", 0));
            AddLinkToGraph(new NodeLink("Node10", 1, "Node13", 0));

            DrawNodeGraph();
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
            PointingStarted(ptStart.Position);
        }

        private void layoutRoot_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsMouseDown = false;

            ptDifXStart = ptDifX;
            ptDifYStart = ptDifY;

            PointingCompleted(e.GetCurrentPoint(null).Position);
        }
        
        double ptDifX = 0;
        double ptDifY = 0;


        private void layoutRoot_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var ptEnd = e.GetCurrentPoint(null);
            
            if (!IsMouseDown) return;

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
    }


}


//http://plnkr.co/edit/II6lgj511fsQ7l0QCoRi?p=preview  <== i used this in the end
//http://stackoverflow.com/questions/2916081/zoom-in-on-a-point-using-scale-and-translate