using System.Collections.Generic;
using X.CoreLib.SQLite;

namespace SumoNinjaMonkey.Framework
{

    public class DBManager
    {
        private static DBManager _instance = null;
        private static object lockobj = new object();

        public static DBManager Current
        {
            get
            {
                DBManager result;
                lock (lockobj)
                {
                    if (_instance == null)
                    {
                        _instance = new DBManager("xappdbs");
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
        private DBManager(string instanceName){ _databases = new Dictionary<string, SqliteDatabase>(); }

        public void DeleteDatabase(string dbName) {
            if (DoesDatabaseExist(dbName))
            {
                var db = _databases[dbName];
                db.Close();
                _databases.Remove(dbName);
            }
        }

        public void DeleteAllDatabases()
        {
            string[] keys = new string[_databases.Keys.Count];
            _databases.Keys.CopyTo(keys, 0) ;
            foreach(var dbName in keys)
            {
                DeleteDatabase(dbName);
            }
        }
    }
     
}
