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

        public bool DoesExist<T>(string UID)
        {
            var found = this.SqliteDb.Query<object>("SELECT * FROM " + typeof(T).Name + " WHERE Uid = ?", UID);
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

        public void DeleteByUid<T>(string uid)
        {
            this.SqliteDb.Execute("delete from " + typeof(T).Name + " where Uid = '" + uid + "'");
        }

        public void DeleteById<T>(string id)
        {
            this.SqliteDb.Execute("delete from ? where id = ?", typeof(T).Name, id);
        }

        public void UpdateFieldByUid<T>(string uid, string fieldName, object value)
        {
            this.SqliteDb.Execute("UPDATE ? set ? = ? where uid = ?", typeof(T).Name, fieldName, value, uid);
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

        public void Truncate<T>()
        {
            this.SqliteDb.Execute("delete from " + typeof(T).Name);
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
