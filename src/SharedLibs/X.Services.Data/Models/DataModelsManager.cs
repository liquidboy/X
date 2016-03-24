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

        public static IList<WebPageDataModel> RetrieveWebPageDataModels(SQLiteConnection sqliteDb)
        {
            return sqliteDb.Query<WebPageDataModel>("SELECT * FROM " + typeof(WebPageDataModel).Name);
        }


    }
}
