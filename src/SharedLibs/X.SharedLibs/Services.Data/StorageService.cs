using System;

namespace X.Services.Data
{
    public class StorageService : IDisposable
    {

        public StorageDatabase Storage { get; set; }

        public StorageService()
        {
            lock (SqliteDatabase.lockobj)
            {
                if (Storage == null)
                {
                    Storage = new StorageDatabase();
                }
            }

            //lock (StorageDatabaseAzure.lockobj)
            //{
            //    if (AzureStorage == null)
            //    {
            //        AzureStorage = new StorageDatabaseAzure();
            //    }
            //}
        }

        public StorageDatabaseAzure AzureStorage { get; set; }


        public void Dispose()
        {
            if (Storage != null) Storage.Dispose();
        }






        //=========================
        //singleton
        //=========================
        private static StorageService instance;
        
        public static StorageService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StorageService();
                }
                return instance;
            }
        }
    }
}
