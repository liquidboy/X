using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    public interface INodeSelector
    {
        void InitializeNodeSelector();
        void OnNodeTypeSelected(NodeTypeMetadata nodeTypeMetadata);
        IEnumerable<NodeTypeMetadata> GetNodeTypeMetaData();
    }
}