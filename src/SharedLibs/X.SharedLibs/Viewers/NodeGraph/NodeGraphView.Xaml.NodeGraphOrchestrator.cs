﻿using Windows.Foundation;
using System.Numerics;
using System;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphOrchestrator
    {
        bool _shouldStopPropogatingPointerMoved;

        public void PointingStarted(Point point) {
            _shouldStopPropogatingPointerMoved = false;
            SetSelectedSlot(point);
            SetSelectedNode(point);
        }

        public void PointingCompleted(Point point) {
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

            _shouldStopPropogatingPointerMoved = false;
        }

        public void PointerUpdated(Vector2 distanceToMove, double scale) {
            _shouldStopPropogatingPointerMoved = false;

            if (IsSlotSelected)
            {
                //join slots
                MoveSelectedSlot(distanceToMove, nodeGraphZoomContainer.Scale);
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
            var defaultGuid = Guid.Empty.ToString();
            if (size == "small") {
                AddNodeToGraph(new Node("Node1", 100, 100, "Red", 1, 1, defaultGuid));
                AddNodeToGraph(new Node("Node2", 100, 300, "Green", 1, 1, defaultGuid));
                AddNodeToGraph(new Node("Node3", 400, 190, "Yellow", 2, 2, defaultGuid));
                AddNodeToGraph(new Node("Node4", 400, 0, "Purple", 1, 1, defaultGuid));
                AddNodeToGraph(new Node("Node5", 700, 100, "Blue", 2, 1, defaultGuid));
                AddNodeToGraph(new Node("Node6", 400, 400, "Pink", 1, 2, defaultGuid));
                AddNodeToGraph(new Node("Node7", 700, 600, "AliceBlue", 5, 8, defaultGuid));
                AddNodeToGraph(new Node("Node8", 700, 1000, "Aquamarine", 3, 1, defaultGuid));
                AddNodeToGraph(new Node("Node9", 1000, 500, "Beige", 2, 2, defaultGuid));
                AddNodeToGraph(new Node("Node10", 1000, 800, "Bisque", 3, 2, defaultGuid));
                AddNodeToGraph(new Node("Node11", 1100, 200, "Brown", 2, 3, defaultGuid));
                AddNodeToGraph(new Node("Node12", 1300, 500, "Coral", 2, 2, defaultGuid));
                AddNodeToGraph(new Node("Node13", 1300, 700, "DarkGoldenrod", 2, 3, defaultGuid));


                AddLinkToGraph(new NodeLink("Node1", 0, "Node3", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node2", 0, "Node3", 1, defaultGuid));
                AddLinkToGraph(new NodeLink("Node4", 0, "Node5", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node3", 0, "Node5", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node3", 1, "Node5", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node6", 0, "Node5", 1, defaultGuid));
                AddLinkToGraph(new NodeLink("Node6", 0, "Node7", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node6", 1, "Node8", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node5", 0, "Node11", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node7", 0, "Node11", 1, defaultGuid));
                AddLinkToGraph(new NodeLink("Node7", 1, "Node9", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node7", 2, "Node9", 1, defaultGuid));
                AddLinkToGraph(new NodeLink("Node7", 3, "Node10", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node8", 0, "Node10", 1, defaultGuid));
                AddLinkToGraph(new NodeLink("Node10", 0, "Node12", 0, defaultGuid));
                AddLinkToGraph(new NodeLink("Node10", 1, "Node13", 0, defaultGuid));
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
                        AddNodeToGraph(new Node($"Node{x}-{y}", x * 200, y * 200, "LightGray", 2, 2, defaultGuid));
                    }
                }
            }

            var newGraph = SaveGraph(defaultGuid, $"default-{size}");

            DrawNodeGraph();

            return newGraph;
        }
    }
}
