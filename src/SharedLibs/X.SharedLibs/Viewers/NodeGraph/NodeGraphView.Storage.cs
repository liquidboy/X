using System;
using System.Collections.Generic;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphStorage
    {
        public void InitializeStorage() => AppDatabase.Current.Init();
        public void DeleteStorage() => DBContext.Current.Manager.DeleteAllDatabases();
        public void ClearStorage()
        {
            DBContext.Current.DeleteAll<Node>();
            DBContext.Current.DeleteAll<NodeLink>();
            DBContext.Current.DeleteAll<SavedGraph>();
        }

        public SavedGraph SaveGraph(string guid) => SaveGraph(guid, string.Empty);
        public SavedGraph SaveGraph(string guid, string name) {
            var graph = new SavedGraph(name, DateTime.UtcNow, DateTime.UtcNow);
            graph.UniqueId = Guid.Parse(guid);
            if (string.IsNullOrEmpty(guid) || guid.Equals(Guid.Empty.ToString()))
            {
                Save(graph);
                foreach (var node in _nodes) node.Value.Grouping = graph.UniqueId.ToString();
                foreach (var link in _links) link.Grouping = graph.UniqueId.ToString();
            }
            foreach (var node in _nodes) Save(node.Value);
            foreach (var link in _links)
            {
                if(_linksToDelete.Contains(link.UniqueId)) Delete(link);
                else Save(link);
            }
            return graph;
        }

        public void Save(Node node) => DBContext.Current.Save(node);
        public void Save(NodeLink link) => DBContext.Current.Save(link);
        public void Save(SavedGraph graph) => DBContext.Current.Save(graph);

        public void Delete(NodeLink link) => DBContext.Current.DeleteEntity<NodeLink>(link.UniqueId);

        public List<SavedGraph> RetrieveGraphs() => DBContext.Current.RetrieveAllEntities<SavedGraph>();
        public (bool GraphFound, List<Node> Nodes, List<NodeLink> NodeLinks) RetrieveGraph(string guid)
        {
            List<NodeLink> returnNodeLinks = new List<NodeLink>();
            List<Node> returnNodes = new List<Node>();
            var foundGraph = DBContext.Current.RetrieveEntity<SavedGraph>(Guid.Parse(guid));
            if (foundGraph != null) {
                if (DBContext.Current.DoesContextExist<NodeLink>())
                {
                    var foundNodeLinks = DBContext.Current.RetrieveEntities<NodeLink>($"grouping='{guid}'");
                    if (foundNodeLinks != null) returnNodeLinks.AddRange(foundNodeLinks);
                }
                
                if (DBContext.Current.DoesContextExist<Node>())
                {
                    var foundNodes = DBContext.Current.RetrieveEntities<Node>($"grouping='{guid}'");
                    if (foundNodes != null) returnNodes?.AddRange(foundNodes);
                }
            }
            return ((returnNodes.Count > 0 || returnNodeLinks.Count > 0), returnNodes, returnNodeLinks);
        }

        
    }
}
