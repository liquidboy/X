using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphGlobalStorage
    {
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;

        public void InitializeGlobalStorage()
        {
            string storageConnectionString = "xxxx";

            // Retrieve storage account information from connection string.
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create a table client for interacting with the table service
            _tableClient = _storageAccount.CreateCloudTableClient();

        }

        public async Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveGlobalNodeTypes(string partitionKey)
        {
            var foundTable = _tableClient.GetTableReference("GlobalNodeType");
            try
            {
                TableQuery<CloudNodeTypeEntity> query = new TableQuery<CloudNodeTypeEntity>();
                query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
                var result = await foundTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
                return (result.Results.Count, result.Results);
            }
            catch (Exception ex) { }
            return (0, null);
        }
    }
}
