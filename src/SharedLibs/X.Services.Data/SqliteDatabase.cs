using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public abstract class SqliteDatabase
    {
        private SQLiteConnection _sqlitedb;
        public static object lockobj = new object();


        public SQLiteConnection SqliteDb
        {
            get { return _sqlitedb; }
        }

        public SqliteDatabase(string dbName)
        {
            string dbNameAndLocation = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, dbName);
            this._sqlitedb = new SQLiteConnection(dbNameAndLocation);
        }

        public void ExecuteStatement(string sql)
        {

            if (this._sqlitedb != null && !this._sqlitedb.IsInTransaction)
            {
                this._sqlitedb.Execute(sql);
            }
        }

    }
}
