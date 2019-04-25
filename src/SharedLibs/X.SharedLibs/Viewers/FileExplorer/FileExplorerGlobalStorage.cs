using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using X.Services.Data;

namespace X.Viewer.FileExplorer
{
    public class FileExplorerGlobalStorage 
    {

        private static FileExplorerGlobalStorage _storage;
        public static FileExplorerGlobalStorage Current
        {
            get
            {
                if (_storage == null) _storage = new FileExplorerGlobalStorage();
                return _storage;
            }
            private set { }
        }

        public void InitializeGlobalStorage(string connectionString)
        {
            AzureGlobalStorage.Current.InitializeGlobalStorage(connectionString);
        }

        public async Task<bool> ClearAll()
        {
            var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalMediaFiles");
            await foundTable.DeleteIfExistsAsync();
            return true;
        }


    }
}