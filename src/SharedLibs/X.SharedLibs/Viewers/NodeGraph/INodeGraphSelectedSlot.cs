﻿using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    // this knows about rendering technology BUT is limited to the SELECTED NODE
    public interface INodeGraphSelectedSlot
    {
        void SetSelectedSlot(Point point);
        void ClearSelectedSlot();
        void MoveSelectedSlot(Vector2 distanceToMove, double scale);   
    }
}
