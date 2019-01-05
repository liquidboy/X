using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    // this knows only about storing a nodegraph
    public interface INodeGraphStorage
    {
        void InitializeStorage();
        void DeleteStorage();
        void ClearStorage();

        SavedGraph SaveGraph(string guid);
        SavedGraph SaveGraph(string guid, string name);
        void Save(Node node);
        void Save(NodeLink link);
        void Save(SavedGraph graph);

        void Delete(NodeLink link);

        List<SavedGraph> RetrieveGraphs();
        (bool GraphFound, List<Node> Nodes, List<NodeLink> NodeLinks) RetrieveGraph(string guid);
    }
}
