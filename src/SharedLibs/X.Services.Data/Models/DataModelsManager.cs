using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public enum eDataModels {
        WebPageDataModel
    }


    static class DataModelsManager
    {

        public static void InitInDatabase(SQLiteConnection sqliteDb)
        {
            sqliteDb.CreateTable<WebPageDataModel>();
        }

        public static void TruncateDatabase(SQLiteConnection sqliteDb) {
            sqliteDb.DeleteAll<WebPageDataModel>();
        }

        public static IList<T> RetrieveList<T>(SQLiteConnection sqliteDb)
        {
            object result = null;

            if(typeof(T).Equals(typeof(WebPageDataModel)))
                result = sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(T).Name);
            
            return result as IList<T>;
        }

        public static IList<T> RetrieveByUid<T>(SQLiteConnection sqliteDb, string uid)
        {
            object result = null;

            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result =  sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(T).Name + " WHERE Uid = ?", uid);

            return result as IList<T>;
        }

        public static IList<T> RetrieveListByIndex<T>(SQLiteConnection sqliteDb, string index1)
        {
            object result = null;

            if (typeof(T).Equals(typeof(WebPageDataModel)))
                result = sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(T).Name + " WHERE index1 = ?", index1);

            return result as IList<T>;

        }
    }
}
