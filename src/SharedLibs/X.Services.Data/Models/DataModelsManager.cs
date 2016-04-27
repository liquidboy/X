using Microsoft.WindowsAzure.MobileServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public enum eDataModels {
        WebPageDataModel,
        ExtensionManifestDataModel
    }


    static class DataModelsManager
    {
        public static void InitInDatabase(SQLiteConnection sqliteDb)
        {
            sqliteDb.CreateTable<WebPageDataModel>();
            sqliteDb.CreateTable<ExtensionManifestDataModel>();
        }

        public static void TruncateDatabase(SQLiteConnection sqliteDb) {
            sqliteDb.DeleteAll<WebPageDataModel>();
            sqliteDb.DeleteAll<ExtensionManifestDataModel>();
        }
      
        public static IList<T> RetrieveList<T>(SQLiteConnection sqliteDb)
        {
            object result = null;

            if(typeof(T).Equals(typeof(WebPageDataModel)))
                result = sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(T).Name);
            else if (typeof(T).Equals(typeof(ExtensionManifestDataModel)))
                result = sqliteDb.Query<ExtensionManifestDataModel>("SELECT * FROM " + typeof(T).Name);


            return result as IList<T>;
        }
        public static async Task<IList<T>> RetrieveList<T>(IMobileServiceClient client)
        {
            object result = null;
            
            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result = await client.GetTable<WebPageDataModel>().ToListAsync();
            else if (typeof(T).Equals(typeof(ExtensionManifestDataModel)))
                result = await client.GetTable<ExtensionManifestDataModel>().ToListAsync();


            return result as IList<T>;
        }
        public static IList<T> RetrieveByUid<T>(SQLiteConnection sqliteDb, string uid)
        {
            object result = null;

            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result =  sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(T).Name + " WHERE Uid = ?", uid);
            else if (typeof(T).Equals(typeof(ExtensionManifestDataModel)))
                result = sqliteDb.Query<ExtensionManifestDataModel>("SELECT * FROM " + typeof(T).Name + " WHERE Uid = ?", uid);

            return result as IList<T>;
        }
        public static async Task<IList<T>> RetrieveByUid<T>(IMobileServiceClient client, string uid)
        {
            object result = null;

            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result = await client.GetTable<WebPageDataModel>().Where(x => x.Uid == uid).ToListAsync();
            else if (typeof(T).Equals(typeof(ExtensionManifestDataModel)))
                result = await client.GetTable<ExtensionManifestDataModel>().Where(x=>x.Uid == uid).ToListAsync();

            return result as IList<T>;
        }

        public static IList<T> RetrieveListByIndex<T>(SQLiteConnection sqliteDb, string index1)
        {
            object result = null;

            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result = sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(T).Name + " WHERE index1 = ?", index1);
            else if (typeof(T).Equals(typeof(ExtensionManifestDataModel)))
                result = sqliteDb.Query<ExtensionManifestDataModel>("SELECT * FROM " + typeof(T).Name + " WHERE index1 = ?", index1);

            return result as IList<T>;

        }
        public static async Task< IList<T>> RetrieveListByIndex<T>(IMobileServiceClient client, string index1)
        {
            object result = null;

            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result = await client.GetTable<WebPageDataModel>().Where(x=>x.Index1 == index1).ToListAsync();
            else if (typeof(T).Equals(typeof(ExtensionManifestDataModel)))
                result = await client.GetTable<ExtensionManifestDataModel>().Where(x=>x.Index1 == index1).ToListAsync();

            return result as IList<T>;

        }

    }
}
