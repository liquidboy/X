using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class StorageDatabase : SqliteDatabase, IDisposable
    {
        private const string database_name = "Storage.db";

        
        public StorageDatabase() : base(database_name)
        {
            DataModelsManager.InitInDatabase(this.SqliteDb);
        }

        public bool DoesExist(string UID, string tableName)
        {
            var found = this.SqliteDb.Query<object>("SELECT * FROM " + tableName + " WHERE Uid = ?", UID);
            if (found != null && found.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public IList<T> RetrieveList<T>()
        {             
            return DataModelsManager.RetrieveList<T>(this.SqliteDb);
        }

        public IList<T> RetrieveListByIndex<T>(string index1)
        {
            return DataModelsManager.RetrieveListByIndex<T>(this.SqliteDb, index1);
        }

        public IList<T> RetrieveByUid<T>(string uid)
        {
            return DataModelsManager.RetrieveByUid<T>( this.SqliteDb, uid);
        }

        public List<object> RetrieveByUid<T>(string uid, string tableName)
        {
            return this.SqliteDb.Query<object>("SELECT * FROM " + tableName + " WHERE Uid = ?", uid);
        }

        public void DeleteByUid(string uid, string tableName)
        {
            this.SqliteDb.Execute("delete from " + tableName + " where Uid = '" + uid + "'");
        }

        public void DeleteById(string id, string tableName)
        {
            this.SqliteDb.Execute("delete from ? where id = ?", tableName, id);
        }

        public void UpdateFieldByUid(string uid, string tableName, string fieldName, object value)
        {
            this.SqliteDb.Execute("UPDATE ? set ? = ? where uid = ?", tableName, fieldName, value, uid);
        }

        public void Update<T>(T value)
        {
            this.SqliteDb.Update(value);
        }

        public async void Insert(ISqliteBase item)
        {
            try
            {
                var newId = this.SqliteDb.Insert(item);
            }
            catch (Exception ex) { }
        }

        public void Truncate(string tableName)
        {
            this.SqliteDb.Execute("delete from " + tableName);
        }

        public void TruncateAll()
        {
            DataModelsManager.TruncateDatabase(SqliteDb);
        }


        public void Dispose()
        {
            this.SqliteDb.Close();
            this.SqliteDb.Dispose();
        }
    }
}
