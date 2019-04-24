using Windows.Foundation;
using System.Numerics;
using System;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphOrchestrator
    {
        bool _shouldStopPropogatingPointerMoved;

        public bool PointingStarted(Point point) {
            var pointingStarted = true;
            _shouldStopPropogatingPointerMoved = false;
            SetSelectedSlot(point);
            SetSelectedNode(point);
            pointingStarted = SetSelectedNodeLink(point, pointingStarted);
            return pointingStarted;
        }

        public void PointingCompleted(Point point) {
            ClearNodeOrSlot(point);
            _shouldStopPropogatingPointerMoved = false;
        }

        public void PointerMovingAndNotPressed(Point currentPoint, double scale) {
            HoverOverNodeGraph(currentPoint, scale);
        }

        public void PointerUpdated(Vector2 distanceToMove, double scale) {
            _shouldStopPropogatingPointerMoved = false;

            if (IsSlotSelected)
            {
                //join slots
                MoveSelectedSlot(distanceToMove, nodeGraphZoomContainer.Scale);
                _shouldStopPropogatingPointerMoved = true;
            } else if (IsNodeLinkSelected)
            {
                //changing node-link
                _shouldStopPropogatingPointerMoved = true;
            }
            else if (IsNodeSelected)
            {
                //move node
                MoveSelectedNode(distanceToMove, nodeGraphZoomContainer.Scale);
                _shouldStopPropogatingPointerMoved = true;
            }
        }

        public bool LoadGraph(string guid)
        {
            var foundGraph = RetrieveGraph(guid);
            foundGraph.NodeLinks.ForEach(nl => AddLinkToGraph(nl));
            foundGraph.Nodes.ForEach(n => AddNodeToGraph(n));
            DrawNodeGraph();
            return foundGraph.GraphFound;
        }

        public SavedGraph SetupExampleGraph(string size)
        {
            var defaultWidth = 200d;
            var defaultGuid = Guid.Empty.ToString();
            if (size == "small") {
                
                AddNodeToGraph(new Node("Node1", 100, 100, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.TextboxValue, "Value"));
                AddNodeToGraph(new Node("Node2", 100, 300, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.TextboxValue, "Value"));
                AddNodeToGraph(new Node("Node3", 400, 0, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 1, "filename", 1, "image", defaultGuid, (int)NodeType.TextureAsset, "Texture Asset"));
                AddNodeToGraph(new Node("Node4", 400, 190, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 1, "filename", 1, "image", defaultGuid, (int)NodeType.TextureAsset, "Texture Asset"));
                AddNodeToGraph(new Node("Node5", 800, 50, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 2, "source,mask", 1, "", defaultGuid, (int)NodeType.AlphaMaskEffect, "Alpha Mask"));

                AddNodeToGraph(new Node("Node6", 1100, 200, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 1, "source", 1, "", defaultGuid, (int)NodeType.GrayscaleEffect, "Grayscale"));

                AddNodeToGraph(new Node("Node7", 800, 300, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 2, "source,angle", 1, "", defaultGuid, (int)NodeType.HueRotationEffect, "Hue Rotation"));
                AddNodeToGraph(new Node("Node8", 350, 500, 300d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.SliderValue, "Value", 0d, 1d, 0.1d));

                AddNodeToGraph(new Node("Node9", 1100, 500, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 2, "source,contrast", 1, "", defaultGuid, (int)NodeType.ContrastEffect, "Contrast"));
                AddNodeToGraph(new Node("Node10", 700, 700, 300d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.SliderValue, "Value", -1d, 1d, 0.1d));

                AddNodeToGraph(new Node("Node11", 1400, 100, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.TextboxValue, "Value"));
                AddNodeToGraph(new Node("Node12", 1700, 400, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 1, "filename", 1, "image", defaultGuid, (int)NodeType.TextureAsset, "Texture Asset"));

                AddNodeToGraph(new Node("Node13", 2000, 700, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 6, "source1,source1Amount,source2,source2Amount,multiplyAmount,offset", 1, "", defaultGuid, (int)NodeType.ArithmeticEffect, "Arithmentic Composite"));
                AddNodeToGraph(new Node("Node14", 1500, 750, 150d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.SliderValue, "Value", -1d, 1d, 0.1d));
                AddNodeToGraph(new Node("Node15", 1500, 900, 150d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.SliderValue, "Value", -1d, 1d, 0.1d));
                AddNodeToGraph(new Node("Node16", 1500, 1050, 150d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.SliderValue, "Value", 0d, 1d, 0.1d));
                AddNodeToGraph(new Node("Node17", 1500, 1200, 150d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.SliderValue, "Value", 0d, 1d, 0.1d));
                
                AddNodeToGraph(new Node("Node18", 2000, 300, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 3, "background,foreground,mode", 1, "", defaultGuid, (int)NodeType.BlendEffect, "Blend"));
                AddNodeToGraph(new Node("Node19", 1800, 100, 600d, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 0, "", 1, "", defaultGuid, (int)NodeType.BlendEffectModeValue, "Value", 0d, 25d, 1d));


                AddLinkToGraph(new NodeLink("Node1", 0, "Node3", 0, defaultGuid, "x-dog.jpg"));
                AddLinkToGraph(new NodeLink("Node2", 0, "Node4", 0, defaultGuid, "x-mask-circle.png"));
                AddLinkToGraph(new NodeLink("Node4", 0, "Node5", 1, defaultGuid));
                AddLinkToGraph(new NodeLink("Node3", 0, "Node5", 0, defaultGuid));

                AddLinkToGraph(new NodeLink("Node5", 0, "Node6", 0, defaultGuid));

                AddLinkToGraph(new NodeLink("Node3", 0, "Node7", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node8", 0, "Node7", 1, defaultGuid));

                AddLinkToGraph(new NodeLink("Node11", 0, "Node12", 0, defaultGuid, "x-ferns.jpg"));

                //AddLinkToGraph(new NodeLink("Node6", 1, "Node8", 0, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node5", 0, "Node11", 0, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node7", 0, "Node11", 1, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node7", 1, "Node9", 0, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node7", 2, "Node9", 1, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node7", 3, "Node10", 0, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node8", 0, "Node10", 1, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node10", 0, "Node12", 0, defaultGuid));
                //AddLinkToGraph(new NodeLink("Node10", 1, "Node13", 0, defaultGuid));
            }
            else {
                //100 failed
                //50 was slow
                //20 was slow but acceptable
                //10 was good

                var dimensionToTest = 10;

                for (int y = 0; y < dimensionToTest; y++)
                {
                    for (int x = 0; x < dimensionToTest; x++)
                    {
                        AddNodeToGraph(new Node($"Node{x}-{y}", x * 200, y * 200, defaultWidth, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", 2,"", 2, "", defaultGuid, (int)NodeType.Empty, string.Empty));
                    }
                }
            }

            var newGraph = SaveGraph(defaultGuid, $"default-{size}");

            DrawNodeGraph();

            return newGraph;
        }

        public void ClearBoard()
        {
            ClearNodeOrSlot(new Point(0, 0));
            ClearGraph();
            ClearCompositor();
            ClearRenderer();
            ClearSelectedGraph();
            ClearSelectedNodeLink();
        }

        void ClearNodeOrSlot(Point point)
        {
            if (IsSlotSelected)
            {
                CompleteGhostLink(point);
                ClearGhostLink();
                ClearSelectedSlot(point);
            }

            if (IsNodeSelected)
            {
                ClearSelectedNode();
            }
            
        }
    }
}
