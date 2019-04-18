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

        public void InitializeGlobalStorage(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
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

        public async Task<bool> InitGlobalNodeTypes() {
            var foundTable = _tableClient.GetTableReference("GlobalNodeType");
            var typesToCreate = new []{ "Entity", "Component", "System" };

            foreach (var typeToCreate in typesToCreate) {
                await foundTable.ExecuteAsync(TableOperation.InsertOrReplace(new CloudNodeTypeEntity(typeToCreate, "Dots") { CreatedDate = DateTime.Now, LastUpdated = DateTime.Now }));
            }

            //    CloudNodeTypeEntity insertedEntity = newEntity.Result as CloudNodeTypeEntity;
            return true;
        }
    }
}

//https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-develop-table-dotnet