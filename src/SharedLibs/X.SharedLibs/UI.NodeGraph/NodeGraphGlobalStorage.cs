using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using X.Services.Data;
using X.UI.NodeGraph;

namespace X.UI.NodeGraph
{
    public class NodeGraphGlobalStorage: INodeGraphGlobalStorage
    {

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
            AzureGlobalStorage.Current.InitializeGlobalStorage(connectionString);
        }

        public async Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveGlobalNodeTypes(string partitionKey)
        {
            var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
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
            var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
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

        public async Task<(int Count, IList<CloudNodeTypeEntity> Results)> RetrieveGlobalNodeType(string partitionKey, string rowKey)
        {
            var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
            try
            {
                TableQuery<CloudNodeTypeEntity> query = new TableQuery<CloudNodeTypeEntity>();
                var filter1 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
                var filter2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
                query.Where(TableQuery.CombineFilters(filter1, TableOperators.And , filter2));
                var result = await foundTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
                return (result.Results.Count, result.Results);
            }
            catch (Exception ex) { }
            return (0, null);
        }

        public async Task<bool> ClearGlobalNodeTypes()
        {
            var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
            await foundTable.DeleteIfExistsAsync();
            return true;
        }

        public async Task<bool> InitGlobalNodeTypes(string[] typesToCreate) {
            try {
                var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
                await foundTable.CreateIfNotExistsAsync();
                foreach (var typeToCreate in typesToCreate)
                {
                    var parts = typeToCreate.Split(":".ToCharArray());
                    await foundTable.ExecuteAsync(TableOperation.InsertOrReplace(new CloudNodeTypeEntity(parts[0], parts[1], parts[2], int.Parse(parts[3]), parts[4], int.Parse(parts[5]), string.Empty, "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { CreatedDate = DateTime.Now, LastUpdated = DateTime.Now }));
                }
            }
            catch (Exception ex) { }
            
            //    CloudNodeTypeEntity insertedEntity = newEntity.Result as CloudNodeTypeEntity;
            return true;
        }
        public async Task<bool> DeleteGlobalNodeType(CloudNodeTypeEntity cloudNodeTypeEntity) {
            try
            {
                var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
                await foundTable.ExecuteAsync(TableOperation.Delete(cloudNodeTypeEntity));
            }
            catch (Exception ex) { }
            return true;
        }

        public async Task<bool> SaveGlobalNodeType(CloudNodeTypeEntity cloudNodeTypeEntity) {

            try
            {
                var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalNodeType");
                await foundTable.ExecuteAsync(TableOperation.InsertOrMerge(cloudNodeTypeEntity));
            }
            catch (Exception ex) { }
            return true;
        }

    }
}

//https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-develop-table-dotnet