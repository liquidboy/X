using System.Collections.Generic;
using X.Viewer.NodeGraph;

namespace X.UI.NodeGraph
{
    // this knows only about storing a nodegraph
    public interface INodeGraphStorage
    {
        void InitializeStorage();
        void DeleteStorage();
        void ClearStorage();

        SavedGraph CreateNewGraph(string guid, string name, string category);
        SavedGraph UpdateExistingGraph(string guid);
        void Save(Node node);
        void Save(NodeLink link);
        void Save(SavedGraph graph);

        void Delete(NodeLink link);

        List<SavedGraph> RetrieveGraphs();
        (bool GraphFound, List<Node> Nodes, List<NodeLink> NodeLinks) RetrieveGraph(string guid);
    }
}
