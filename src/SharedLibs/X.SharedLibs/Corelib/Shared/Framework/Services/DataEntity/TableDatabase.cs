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
    public partial class TableDatabase : SqliteDatabase
    {
        public TableDatabase(string tableName) : base(tableName + ".tbl")
        {
            
        }

        public int AddEntity<T>(T newEntity) {
            //this.Connection.CreateTable<T>(CreateFlags.FullTextSearch4);
            this.Connection.CreateTable<T>();
            var createdEntityId = this.Connection.Insert(newEntity);
            return createdEntityId;
        }

        public T GetEntity<T>(Guid uniqueId) where T : new()
        {
            //var resultsCount = this.Connection.Table<T>().Count();
            var name = typeof(T).Name;
            var qry = $"SELECT * FROM '{name}' WHERE uniqueid = '{uniqueId}'";
            var found = Connection.Query<T>(qry);
            return found.FirstOrDefault();
        }
        public List<T> GetEntities<T>(string where) where T : new()
        {
            //var resultsCount = this.Connection.Table<T>().Count();
            var name = typeof(T).Name;
            var qry = $"SELECT * FROM '{name}' WHERE {where}";
            return Connection.Query<T>(qry);
        }
        public List<T> GetAllEntities<T>() where T : new()
        {
            //var resultsCount = this.Connection.Table<T>().Count();
            var name = typeof(T).Name;
            var qry = $"SELECT ROWID as _rowId, * FROM '{name}'";
            try { 
                return Connection.Query<T>(qry);
            }catch{ return null; }
        }
        public void DeleteAllEntities<T>()
        {
            this.Connection.DeleteAll<T>();
        }
        
        public void UpdateEntity<T>(Guid id, T entityToUpdate)
        {
            this.Connection.Update(entityToUpdate);
        }
        

    }
}
