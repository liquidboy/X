using Microsoft.WindowsAzure.MobileServices;
using X.CoreLib.SQLite;
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
            sqliteDb.CreateTable<PassportDataModel>();
            sqliteDb.CreateTable<APIKeyDataModel>();
            sqliteDb.CreateTable<SketchDataModel>();
            sqliteDb.CreateTable<SketchPageDataModel>();
        }

        public static void InitInDatabase(IMobileServiceClient client)
        {

        }

        public static void TruncateDatabase(SQLiteConnection sqliteDb) {
            sqliteDb.DeleteAll<WebPageDataModel>();
            sqliteDb.DeleteAll<ExtensionManifestDataModel>();
            sqliteDb.DeleteAll<PassportDataModel>();
            sqliteDb.DeleteAll<APIKeyDataModel>();
            sqliteDb.DeleteAll<SketchDataModel>();
            sqliteDb.DeleteAll<SketchPageDataModel>();
        }
      
        public static IList<T> RetrieveList<T>(SQLiteConnection sqliteDb)
        {
            object result = null;
            var to = typeof(T);
            var qry = $"SELECT * FROM { to.Name }";

            if (to.Equals(typeof(WebPageDataModel))) result = sqliteDb.Query<WebPageDataModel>(qry);
            else if (to.Equals(typeof(ExtensionManifestDataModel))) result = sqliteDb.Query<ExtensionManifestDataModel>(qry);
            else if (to.Equals(typeof(PassportDataModel))) result = sqliteDb.Query<PassportDataModel>(qry);
            else if (to.Equals(typeof(APIKeyDataModel))) result = sqliteDb.Query<APIKeyDataModel>(qry);
            else if (to.Equals(typeof(SketchDataModel))) result = sqliteDb.Query<SketchDataModel>(qry);
            else if (to.Equals(typeof(SketchPageDataModel))) result = sqliteDb.Query<SketchPageDataModel>(qry);


            return result as IList<T>;
        }
        public static async Task<IList<T>> RetrieveList<T>(IMobileServiceClient client)
        {
            object result = null;
            var to = typeof(T);

            if (to.Equals(typeof(WebPageDataModel))) result = await client.GetTable<WebPageDataModel>().ToListAsync();
            else if (to.Equals(typeof(ExtensionManifestDataModel))) result = await client.GetTable<ExtensionManifestDataModel>().ToListAsync();
            else if (to.Equals(typeof(PassportDataModel))) result = await client.GetTable<PassportDataModel>().ToListAsync();
            else if (to.Equals(typeof(APIKeyDataModel))) result = await client.GetTable<APIKeyDataModel>().ToListAsync();
            else if (to.Equals(typeof(SketchDataModel))) result = await client.GetTable<SketchDataModel>().ToListAsync();
            else if (to.Equals(typeof(SketchPageDataModel))) result = await client.GetTable<SketchPageDataModel>().ToListAsync();


            return result as IList<T>;
        }
        public static IList<T> RetrieveByUid<T>(SQLiteConnection sqliteDb, string uid)
        {
            object result = null;
            var to = typeof(T);
            var qry = $"SELECT * FROM { to.Name } WHERE Uid = ?";

            if (to.Equals(typeof(WebPageDataModel))) result =  sqliteDb.Query<WebPageDataModel>(qry, uid);
            else if (to.Equals(typeof(ExtensionManifestDataModel))) result = sqliteDb.Query<ExtensionManifestDataModel>(qry, uid);
            else if (to.Equals(typeof(PassportDataModel))) result = sqliteDb.Query<PassportDataModel>(qry, uid);
            else if (to.Equals(typeof(APIKeyDataModel))) result = sqliteDb.Query<APIKeyDataModel>(qry, uid);
            else if (to.Equals(typeof(SketchDataModel))) result = sqliteDb.Query<SketchDataModel>(qry, uid);
            else if (to.Equals(typeof(SketchPageDataModel))) result = sqliteDb.Query<SketchPageDataModel>(qry, uid);

            return result as IList<T>;
        }
        public static async Task<IList<T>> RetrieveByUid<T>(IMobileServiceClient client, string uid)
        {
            object result = null;
            var to = typeof(T);

            if (to.Equals(typeof(WebPageDataModel))) result = await client.GetTable<WebPageDataModel>().Where(x => x.Uid == uid).ToListAsync();
            else if (to.Equals(typeof(ExtensionManifestDataModel))) result = await client.GetTable<ExtensionManifestDataModel>().Where(x=>x.Uid == uid).ToListAsync();
            else if (to.Equals(typeof(PassportDataModel))) result = await client.GetTable<PassportDataModel>().Where(x => x.Uid == uid).ToListAsync();
            else if (to.Equals(typeof(APIKeyDataModel))) result = await client.GetTable<APIKeyDataModel>().Where(x => x.Uid == uid).ToListAsync();
            else if (to.Equals(typeof(SketchDataModel))) result = await client.GetTable<SketchDataModel>().Where(x => x.Uid == uid).ToListAsync();
            else if (to.Equals(typeof(SketchPageDataModel))) result = await client.GetTable<SketchPageDataModel>().Where(x => x.Uid == uid).ToListAsync();

            return result as IList<T>;
        }

        public static IList<T> RetrieveListByIndex<T>(SQLiteConnection sqliteDb, string index1)
        {
            object result = null;
            var to = typeof(T);
            var qry = $"SELECT * FROM { to.Name } WHERE index1 = ?";

            if (to.Equals(typeof(WebPageDataModel))) result = sqliteDb.Query<WebPageDataModel>(qry, index1);
            else if (to.Equals(typeof(ExtensionManifestDataModel))) result = sqliteDb.Query<ExtensionManifestDataModel>(qry, index1);
            else if (to.Equals(typeof(PassportDataModel))) result = sqliteDb.Query<PassportDataModel>(qry, index1);
            else if (to.Equals(typeof(APIKeyDataModel))) result = sqliteDb.Query<APIKeyDataModel>(qry, index1);
            else if (to.Equals(typeof(SketchDataModel))) result = sqliteDb.Query<SketchDataModel>(qry, index1);
            else if (to.Equals(typeof(SketchPageDataModel))) result = sqliteDb.Query<SketchPageDataModel>(qry, index1);

            return result as IList<T>;

        }
        public static async Task< IList<T>> RetrieveListByIndex<T>(IMobileServiceClient client, string index1)
        {
            object result = null;
            var to = typeof(T);

            if (to.Equals(typeof(WebPageDataModel))) result = await client.GetTable<WebPageDataModel>().Where(x=>x.Index1 == index1).ToListAsync();
            else if (to.Equals(typeof(ExtensionManifestDataModel))) result = await client.GetTable<ExtensionManifestDataModel>().Where(x=>x.Index1 == index1).ToListAsync();
            else if (to.Equals(typeof(PassportDataModel))) result = await client.GetTable<PassportDataModel>().Where(x => x.Index1 == index1).ToListAsync();
            else if (to.Equals(typeof(APIKeyDataModel))) result = await client.GetTable<APIKeyDataModel>().Where(x => x.Index1 == index1).ToListAsync();
            else if (to.Equals(typeof(SketchDataModel))) result = await client.GetTable<SketchDataModel>().Where(x => x.Index1 == index1).ToListAsync();
            else if (to.Equals(typeof(SketchPageDataModel))) result = await client.GetTable<SketchPageDataModel>().Where(x => x.Index1 == index1).ToListAsync();

            return result as IList<T>;

        }

    }
}
