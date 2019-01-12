using System.Collections.Generic;
using System.Linq;
using System;
using X.CoreLib.SQLite;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public partial class TableSameDatabase
    {
        private SQLiteConnection _connection;

        public TableSameDatabase(string tableName, SQLiteConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public int AddEntity<T>(T newEntity) {
            //this.Connection.CreateTable<T>(CreateFlags.FullTextSearch4);
            this._connection.CreateTable<T>();
            var createdEntityId = this._connection.Insert(newEntity);
            return createdEntityId;
        }

        public T GetEntity<T>(Guid uniqueId) where T : new()
        {
            //var resultsCount = this.Connection.Table<T>().Count();
            var name = typeof(T).Name;
            var qry = $"SELECT * FROM '{name}' WHERE uniqueid = '{uniqueId}'";
            var found = _connection.Query<T>(qry);
            return found.FirstOrDefault();
        }
        public List<T> GetEntities<T>(string where) where T : new()
        {
            //var resultsCount = this.Connection.Table<T>().Count();
            var name = typeof(T).Name;
            var qry = $"SELECT * FROM '{name}' WHERE {where}";
            return _connection.Query<T>(qry);
        }
        public List<T> GetAllEntities<T>() where T : new()
        {
            //var resultsCount = this.Connection.Table<T>().Count();
            var name = typeof(T).Name;
            var qry = $"SELECT ROWID as _rowId, * FROM '{name}'";
            try { 
                return _connection.Query<T>(qry);
            }catch{ return new List<T>(); }
        }
        public void DeleteAllEntities<T>() => _connection.DeleteAll<T>();
        public void UpdateEntity<T>(Guid id, T entityToUpdate) => _connection.Update(entityToUpdate);

        public void DeleteEntity<T>(Guid uniqueId) where T : new()
        {
            //var name = typeof(T).Name;
            //var qry = $"SELECT * FROM '{name}' WHERE uniqueid='{uniqueId}'";
            //var found = this.Connection.Query<T>(qry);
            //var found = GetEntity<T>(uniqueId);
            var result = _connection.Delete<T>(uniqueId);


        }
        

    }
}
