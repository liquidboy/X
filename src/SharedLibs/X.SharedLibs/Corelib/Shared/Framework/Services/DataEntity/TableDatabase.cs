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
        //public TableLog Log;
        //public TableIndex Index;

        public TableDatabase(string tableName) : base(tableName + ".tbl")
        {
            this.Connection.CreateTable<TableRow>();
            this.Connection.CreateTable<TableColumn>();

            //Log = new TableLog(tableName);
            //Index = new TableIndex(tableName);
        }



        public int AddRow(string dataAsJson, string userName)
        {
            var newRow = new TableRow()
            {
                Data = dataAsJson,
                Type = (int)eTableType.UserDefined,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow.ToUniversalTime()
            };
            this.Connection.Insert(newRow);
            return newRow.Id;
        }

        public int AddEntity<T>(T newEntity) {
            this.Connection.CreateTable<T>();
            var createdEntityId = this.Connection.Insert(newEntity);
            return createdEntityId;
        }


        public void AddColumn(string name, string userName)
        {
            var exists = DoesColumnExist(name);

            if (exists) return;

            var newColumn = new TableColumn()
            {
                Name = name,
                Type = (int)eTableType.UserDefined,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow.ToUniversalTime()
            };



            this.Connection.Insert(newColumn);
        }

        public bool DoesColumnExist(string name)
        {
            var row = this.Connection.Query<TableRow>("SELECT * FROM 'TableColumn' WHERE Name = ?", name);
            return row.Count() > 0;
        }


        public JObject GetEmptyRowAsJson()
        {
            dynamic udo = new JObject();

            var cols = this.GetColumns();
            foreach (var col in cols)
            {
                udo[col.Name] = "";
            }

            return udo;
        }
        public List<TableRow> Find(string whereQuery)
        {
            return this.Connection.Query<TableRow>("SELECT * FROM TableRow WHERE " + whereQuery);
        }
        //public List<T> Find<T>(string whereQuery, params object[] args) where T : new()
        //{
        //    return Connection.Query<T>(whereQuery, args);
        //}
        public TableRow GetTableRow(int id)
        {
            var row = this.Connection.Query<TableRow>("SELECT * FROM TableRow WHERE Id = ?", id);
            return row.SingleOrDefault();
        }
        public dynamic GetRowDataAsJson(int id)
        {
            var row = this.Connection.Query<TableRow>("SELECT Data FROM TableRow WHERE Id = ?", id);
            dynamic json = JValue.Parse(row.SingleOrDefault().Data);
            return json;
        }
        public dynamic GetRowDataAsJson(Guid uniqueId)
        {
            var row = this.Connection.Query<TableRow>("SELECT Data FROM 'TableRow' WHERE UniqueId = ?", uniqueId.ToString());
            dynamic json = JValue.Parse(row.SingleOrDefault().Data);
            return json;
        }
        public List<TableColumn> GetColumns()
        {
            return this.Connection.Query<TableColumn>("SELECT * FROM 'TableColumn'");
        }
        public List<TableRow> GetRows()
        {
            return this.Connection.Query<TableRow>("SELECT * FROM 'TableRow'");
        }
        public List<dynamic> GetRowsDataAsJson()
        {
            var rows = GetRows();
            return rows.Select(d => {
                dynamic json = JValue.Parse(d.Data);
                json._id = d.Id;
                return json;
            }).ToList();
        }


        public void UpdateRowData(int id, dynamic dataAsJson)
        {
            var foundRow = GetTableRow(id);
            foundRow.Data = dataAsJson.ToString();
            this.Connection.Update(foundRow);
        }
        public void UpdateEntity<T>(int id, T entityToUpdate)
        {
            this.Connection.Update(entityToUpdate);
        }
        public void UpdateColumn(TableColumn column)
        {
            this.Connection.Update(column);
        }



        public void DeleteColumn(TableColumn column)
        {
            this.Connection.Delete(column);
        }
        public void DeleteAllColumns()
        {
            //this.DeleteAllRows();
            this.Connection.DeleteAll<TableColumn>();
        }
        public void DeleteAllRows()
        {
            this.Connection.DeleteAll<TableRow>();
        }
        public void DeleteRow(int id)
        {
            this.Connection.Delete(new TableRow() { Id = (int)id });
        }


    }
}
