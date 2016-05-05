using FavouriteMX.Shared.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class StorageDatabaseAzure: IDisposable
    {
        public static object lockobj = new object();

        public MobileServiceClient MobileService = new MobileServiceClient(AppService.AppSetting.AMSUrl);

        public StorageDatabaseAzure() 
        {
            DataModelsManager.InitInDatabase(this.MobileService);
        }

        public void Dispose()
        {
            MobileService.Dispose();
        }

        public void Save<T>(T entity) {
            //MobileService.GetTable<T>()

            var tbl = MobileService.GetTable<T>();
            if (tbl != null) {
                tbl.InsertAsync(entity);
            }
        }

    }
}
