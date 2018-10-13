using System.Collections.Generic;
using X.CoreLib.SQLite;

namespace SumoNinjaMonkey.Framework
{

    public class SqliteDatabaseManager
    {
        private static SqliteDatabaseManager _instance = null;
        private static object lockobj = new object();

        public static SqliteDatabaseManager Current
        {
            get
            {
                SqliteDatabaseManager result;
                lock (lockobj)
                {
                    if (_instance == null)
                    {
                        _instance = new SqliteDatabaseManager("xappdbs");
                    }
                    result = _instance;
                }
                return result;
            }
        }

        private Dictionary<string, SqliteDatabase> _databases;

        public SqliteDatabase GetDatabase(string name) { return _databases[name]; }
        public bool DoesDatabaseExist(string name) { return _databases.ContainsKey(name); }
        public void RegisterDatabase(string dbName, SqliteDatabase db) {
            if (!DoesDatabaseExist(dbName)) {
                _databases.Add(dbName, db);
            }
        }
        private SqliteDatabaseManager(string instanceName){ _databases = new Dictionary<string, SqliteDatabase>(); }

         
    }
     
}
