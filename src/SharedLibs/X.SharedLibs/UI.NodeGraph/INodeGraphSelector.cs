using System.Collections.Generic;
using X.Viewer.NodeGraph;

namespace X.UI.NodeGraph
{
    // this knows about rendering technology BUT is limited to the SELECTED NODE
    public interface INodeGraphSelector
    {
        bool IsGraphSelected { get; }
        void InitializeGraphSelector(List<SavedGraph> graphs);
        void ClearSelectedGraph();
        void OnGraphSelected(string guid);
        string SelectedGraphGuid { get; }
        void SetSelectedGraph(string guid);
    }
}
