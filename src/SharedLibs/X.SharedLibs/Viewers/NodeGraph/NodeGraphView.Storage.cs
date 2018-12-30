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
        }
        
        public void SaveGraph(string guid, string name)
        {
            foreach (var node in _nodes) Save(node.Value);
            foreach (var link in _links) Save(link);
        }
        public void Save(Node node) => DBContext.Current.Save(node);
        public void Save(NodeLink link) => DBContext.Current.Save(link);

        public (bool GraphFound, List<Node> Nodes, List<NodeLink> NodeLinks) RetrieveGraph(string guid)
        {
            List<NodeLink> returnNodeLinks = new List<NodeLink>();
            if (DBContext.Current.DoesContextExist<NodeLink>())
            {
                var foundNodeLinks = DBContext.Current.RetrieveAllEntities<NodeLink>();
                if (foundNodeLinks != null) returnNodeLinks.AddRange(foundNodeLinks);
            }
            List<Node> returnNodes = new List<Node>();
            if (DBContext.Current.DoesContextExist<Node>())
            {
                var foundNodes = DBContext.Current.RetrieveAllEntities<Node>();
                if (foundNodes != null) returnNodes?.AddRange(foundNodes);
            }
            return ((returnNodes.Count > 0 || returnNodeLinks.Count > 0), returnNodes, returnNodeLinks);
        }

    }
}
