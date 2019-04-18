using Microsoft.WindowsAzure.Storage.Table;
using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public class CloudNodeTypeEntity : TableEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        
        public CloudNodeTypeEntity() { }
        public CloudNodeTypeEntity(string name, string category)
        {
            PartitionKey = category;
            RowKey = name;
        }
    }
}
