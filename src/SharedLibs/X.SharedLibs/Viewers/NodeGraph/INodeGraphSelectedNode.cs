﻿using System.Numerics;
using Windows.Foundation;

namespace X.Viewer.NodeGraph
{
    // this knows about rendering technology BUT is limited to the SELECTED NODE
    public interface INodeGraphSelectedNode
    {
        bool IsNodeSelected { get; }
        void SetSelectedNode(Point point);
        void ClearSelectedNode();
        void MoveSelectedNode(Vector2 distanceToMove, double scale);   
    }
}
