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

                //Statement statement = this._sqlitedb.PrepareStatement(sql);
                //statement.Execute();
                this._sqlitedb.Execute(sql);

            }
        }


        //public static void TestDB()
        //{

        //    Database database = new Database("sqlite.test");
        //    if (database.Ready)
        //    {
        //        Statement statement = database.PrepareStatement("CREATE TABLE IF NOT EXISTS people (name TEXT, age INTEGER)");
        //        if (statement.Execute())
        //        {
        //            statement = database.PrepareStatement("INSERT INTO people (name, age) VALUES (?, ?)");
        //            statement.BindText(1, "John Smith");
        //            statement.BindInt(2, 33);
        //            if (statement.Execute())
        //            {
        //                statement = database.PrepareStatement("SELECT name, age FROM people");
        //                while (statement.HasMore())
        //                {
        //                    statement.ColumnAsTextAt(0);
        //                    statement.ColumnAsIntAt(1);
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
