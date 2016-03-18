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

        //private void UpdateUIOfAction()
        //{
        //    Messenger.Default.Send<BeatHeartMessage>(new BeatHeartMessage() { });
        //}

        public StorageDatabase() : base(database_name)
        {
            this.SqliteDb.CreateTable<WebPageModel>();
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

        public List<WebPageModel> RetrieveList(string tableName)
        {
            //UpdateUIOfAction();
            return this.SqliteDb.Query<WebPageModel>("SELECT * FROM " + tableName);
        }

        public List<object> RetrieveListByIndex<T>(string index1, string tableName)
        {
            //UpdateUIOfAction();
            return this.SqliteDb.Query<object>("SELECT * FROM " + tableName + " WHERE index1 = ?", index1);
        }

        public List<WebPageModel> RetrieveByUid(string uid)
        {
            //UpdateUIOfAction();
            return this.SqliteDb.Query<WebPageModel>("SELECT * FROM WebPageModel WHERE Uid = ?", uid);
        }

        public List<object> RetrieveByUid<T>(string uid, string tableName)
        {
            //UpdateUIOfAction();
            return this.SqliteDb.Query<object>("SELECT * FROM " + tableName + " WHERE Uid = ?", uid);
        }

        public void DeleteByUid(string uid, string tableName)
        {
            //UpdateUIOfAction();
            this.SqliteDb.Execute("delete from " + tableName + " where Uid = '" + uid + "'");
        }

        public void DeleteById(string id, string tableName)
        {
            //UpdateUIOfAction();
            this.SqliteDb.Execute("delete from ? where id = ?", tableName, id);
        }

        public void UpdateFieldByUid(string uid, string tableName, string fieldName, object value)
        {
            //UpdateUIOfAction();
            this.SqliteDb.Execute("UPDATE ? set ? = ? where uid = ?", tableName, fieldName, value, uid);
        }

        public void Update<T>(T value)
        {
            //UpdateUIOfAction();
            this.SqliteDb.Update(value);
        }

        public async void Insert(ISqliteBase item)
        {
            //UpdateUIOfAction();
            try
            {
                var newId = this.SqliteDb.Insert(item);
            }
            catch (Exception ex) { }
        }

        public void Truncate(string tableName)
        {
            //UpdateUIOfAction();
            this.SqliteDb.Execute("delete from " + tableName);
        }


        public void Dispose()
        {
            this.SqliteDb.Close();
            this.SqliteDb.Dispose();
        }
    }
}
