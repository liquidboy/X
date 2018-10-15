using X.CoreLib.SQLite;

namespace SumoNinjaMonkey.Framework
{

    public abstract class SqliteDatabase
    {
        protected static object lockobj = new object();

        public SQLiteConnection Connection { get; private set; }

        public string Name { get; set; }
        public string Location { get; set; }

        public SqliteDatabase(string dbName){
            Name = dbName;
            Location = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, dbName);
            if (!DBManager.Current.DoesDatabaseExist(dbName)) {
                this.Connection = new SQLiteConnection(Location);
                DBManager.Current.RegisterDatabase(dbName, this);
            }
        }

        public void ExecuteStatement(string sql)
        {
            
            if (this.Connection != null && !this.Connection.IsInTransaction)
            {
                
                //Statement statement = this._sqlitedb.PrepareStatement(sql);
                //statement.Execute();
                this.Connection.Execute(sql);
                
            }
        }

        public void Close() {
            Connection.Close();
            Connection = null;
            Name = string.Empty;
            System.IO.File.Delete(Location);
            Location = string.Empty;
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
