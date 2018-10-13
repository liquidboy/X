using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Windows.Foundation;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using SumoNinjaMonkey.Framework;
using X.CoreLib.SQLite;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public partial class AppDatabase : SqliteDatabase
    {
        private static AppDatabase database = null;

        public static AppDatabase Current
        {
            get
            {
                AppDatabase result;
                lock (lockobj)
                {
                    if (database == null)
                    {
                        database = new AppDatabase();
                    }
                    result = database;
                }
                return result;
            }
        }


        public Dictionary<string, TableDatabase> Tables;

        private AppDatabase()
            : base("xapp.db")
        {


        }

        public void Init()
        {
            Tables = new Dictionary<string, TableDatabase>();
            this.SqliteDb.CreateTable<Table>();
            refreshTables();
        }

        public void Unload()
        {

            database.SqliteDb.Close();
            database.SqliteDb.Dispose();
            database = null;

        }

        public void AddTable(string tableName, string userName)
        {
            var exists = DoesTableExist(tableName);

            if (exists) return;

            var newTable = new Table()
            {
                Name = tableName,
                Type = (int)eTableType.UserDefined,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow.ToUniversalTime()
            };

            this.SqliteDb.Insert(newTable);
            try
            {
                refreshTables();
            }
            catch (Exception ex)
            {

            }

        }

        public bool DoesTableExist(string name)
        {
            var row = this.SqliteDb.Query<Table>("SELECT * FROM 'Table' WHERE Name = ?", name);
            return row.Count() > 0;
        }

        private void refreshTables()
        {
            var tables = this.SqliteDb.Query<Table>("SELECT * FROM 'Table'");

            foreach (var table in tables)
            {
                if (!Tables.ContainsKey(table.Name))
                {
                    TableDatabase tblDB = new TableDatabase(table.Name);
                    Tables.Add(table.Name, tblDB);
                }

            }
        }
    }
    
}
