using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    public interface INodeSelector
    {
        void InitializeNodeSelector();
        void OnNodeTypeSelected(NodeType nodeType);
        IEnumerable<NodeTypeMetadata> GetNodeTypeMetaData();
    }
}