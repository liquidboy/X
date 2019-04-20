using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.NodeGraph
{
    public class NodeGraphGlobalStorage: INodeGraphGlobalStorage
    {
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;

        private static NodeGraphGlobalStorage _NodeGraphGlobalStorage;
        public static NodeGraphGlobalStorage Current {
            get {
                if (_NodeGraphGlobalStorage == null) _NodeGraphGlobalStorage = new NodeGraphGlobalStorage();
                return _NodeGraphGlobalStorage;
            }
            private set { }
        }
        
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

        public async Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveAllGlobalNodeTypes()
        {
            var foundTable = _tableClient.GetTableReference("GlobalNodeType");
            try
            {
                TableQuery<CloudNodeTypeEntity> query = new TableQuery<CloudNodeTypeEntity>();
                //query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
                var result = await foundTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
                return (result.Results.Count, result.Results);
            }
            catch (Exception ex) { }
            return (0, null);
        }

        public async Task<bool> ClearGlobalNodeTypes()
        {
            var foundTable = _tableClient.GetTableReference("GlobalNodeType");
            await foundTable.DeleteIfExistsAsync();
            return true;
        }

        public async Task<bool> InitGlobalNodeTypes(string[] typesToCreate) {
            try {
                var foundTable = _tableClient.GetTableReference("GlobalNodeType");
                await foundTable.CreateIfNotExistsAsync();
                foreach (var typeToCreate in typesToCreate)
                {
                    var parts = typeToCreate.Split(":".ToCharArray());
                    await foundTable.ExecuteAsync(TableOperation.InsertOrReplace(new CloudNodeTypeEntity(parts[0], parts[1], parts[2], int.Parse(parts[3]), parts[4], int.Parse(parts[5]), string.Empty, string.Empty) { CreatedDate = DateTime.Now, LastUpdated = DateTime.Now }));
                }
            }
            catch (Exception ex) { }
            
            //    CloudNodeTypeEntity insertedEntity = newEntity.Result as CloudNodeTypeEntity;
            return true;
        }

    }
}

//https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-develop-table-dotnet