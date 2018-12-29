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
        }
        
        private void SetupExampleNodes()
        {
            _nodes.Add("Node1", new Node("Node1", new NodePosition(100, 100), Colors.Red, 1, 1));
            _nodes.Add("Node2", new Node("Node2", new NodePosition(100, 300), Colors.Green, 1, 1));
            _nodes.Add("Node3", new Node("Node3", new NodePosition(400, 190), Colors.Yellow, 2, 2));
            _nodes.Add("Node4", new Node("Node4", new NodePosition(400, 0), Colors.Purple, 1, 1));
            _nodes.Add("Node5", new Node("Node5", new NodePosition(700, 100), Colors.Blue, 2, 1));
            _nodes.Add("Node6", new Node("Node6", new NodePosition(400, 400), Colors.Pink, 1, 1));

            _links.Add(new NodeLink("Node1", 0, "Node3", 0));
            _links.Add(new NodeLink("Node2", 0, "Node3", 1));
            _links.Add(new NodeLink("Node4", 0, "Node5", 0));
            _links.Add(new NodeLink("Node3", 0, "Node5", 0));
            _links.Add(new NodeLink("Node3", 1, "Node5", 0));
            _links.Add(new NodeLink("Node6", 0, "Node5", 1));

            DrawNodes();
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
            CheckIfNodeIsPressed(e.GetCurrentPoint(null).Position);

        }

        private void layoutRoot_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsMouseDown = false;

            ptDifXStart = ptDifX;
            ptDifYStart = ptDifY;
            
            ClearSelectedNode();
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
            
            if (!string.IsNullOrEmpty(_selectedNodeKey)) {
                //move node
                var distanceBetween2Points = Vector2.Subtract(ptEnd.Position.ToVector2(), ptStart.Position.ToVector2());
                MoveNode(distanceBetween2Points, nodeGraphZoomContainer.Scale);
                Debug.WriteLine($"vector distance : {distanceBetween2Points}  ptStart : {ptStart.Position}  ptEnd : {ptEnd.Position} ");
            }
            else {
                //move board
                ptDifX = ptDifXStart + ((ptStart.Position.X - ptEnd.Position.X) / nodeGraphZoomContainer.Scale);
                ptDifY = ptDifYStart + ((ptStart.Position.Y - ptEnd.Position.Y) / nodeGraphZoomContainer.Scale);
                
                ct.TranslateX = -1 * ptDifX;
                ct.TranslateY = -1 * ptDifY;
            }
            
        }
    }


}


//http://plnkr.co/edit/II6lgj511fsQ7l0QCoRi?p=preview  <== i used this in the end
//http://stackoverflow.com/questions/2916081/zoom-in-on-a-point-using-scale-and-translate