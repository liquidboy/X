﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphGlobalStorage
    {
        void InitializeGlobalStorage(string connectionString);
        Task<bool> ClearGlobalNodeTypes();
        Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveAllGlobalNodeTypes();
        Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveGlobalNodeTypes(string partitionKey);
        Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveGlobalNodeType(string partitionKey, string rowKey);
        Task<bool> InitGlobalNodeTypes(string[] typesToCreate);
    }
}
