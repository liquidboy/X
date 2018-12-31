﻿using System;
using System.Collections.Generic;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelector
    {
        string _selectedGraph;

        public void InitializeGraphSelector(List<SavedGraph> graphs)
        {
            if (graphs != null)
            {
                if (graphs.Count == 0)
                {
                    graphs.Add(SetupExampleGraph("small"));
                }
                cbSavedGraphs.ItemsSource = graphs;
            }
        }

        public void ClearSelectedGraph()
        {
            _selectedGraph = string.Empty;
        }

        public void GraphSelected(string guid)
        {
            var found = LoadGraph(guid);
            if (found) _selectedGraph = guid;
        }
    }
}
