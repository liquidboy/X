﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace X.Viewer.NodeGraph
{
    public interface INodeGraphGlobalStorage
    {
        void InitializeGlobalStorage(string connectionString);

        Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveGlobalNodeTypes(string partitionKey);
    }
}
