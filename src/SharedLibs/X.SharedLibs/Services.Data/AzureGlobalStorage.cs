using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.CoreLib.SQLite;

namespace X.Services.Data
{
    public class AzureGlobalStorage
    {
        private static AzureGlobalStorage _storage;
        public static AzureGlobalStorage Current
        {
            get
            {
                if (_storage == null) _storage = new AzureGlobalStorage();
                return _storage;
            }
            private set { }
        }

        public CloudStorageAccount StorageAccount { get; private set; }
        public CloudTableClient TableClient { get; private set; }

        public void InitializeGlobalStorage(string connectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);
            TableClient = StorageAccount.CreateCloudTableClient();
        }
    }
}
