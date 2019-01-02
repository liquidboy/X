using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphSelector
    {
        string _selectedGraphGuid;

        public string SelectedGraphGuid => _selectedGraphGuid;
        public bool IsGraphSelected => !string.IsNullOrEmpty(_selectedGraphGuid);
        public void InitializeGraphSelector(List<SavedGraph> graphs)
        {
            if (graphs != null)
            {
                if (graphs.Count == 0)
                {
                    var newDefaultGraph = SetupExampleGraph("small");
                    graphs.Add(newDefaultGraph);
                    SetSelectedGraph(newDefaultGraph.UniqueId.ToString());
                }
                cbSavedGraphs.ItemsSource = graphs;
            }
        }

        public void ClearSelectedGraph()
        {
            _selectedGraphGuid = string.Empty;
            cbSavedGraphs.SelectedIndex = -1;
        }

        public void OnGraphSelected(string guid)
        {
            var found = LoadGraph(guid);
            if (found) SetSelectedGraph(guid);
        }

        public void SetSelectedGraph(string guid) {
            _selectedGraphGuid = guid;
        }
    }
}
