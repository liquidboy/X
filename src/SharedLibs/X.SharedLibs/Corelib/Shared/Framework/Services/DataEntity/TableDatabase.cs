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
            this.SqliteDb.CreateTable<TableRow>();
            this.SqliteDb.CreateTable<TableColumn>();

            //Log = new TableLog(tableName);
            //Index = new TableIndex(tableName);
        }



        public int AddRow(dynamic dataAsJson, string userName)
        {
            var newRow = new TableRow()
            {
                Data = dataAsJson.ToString(),
                Type = (int)eTableType.UserDefined,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow.ToUniversalTime()
            };
            this.SqliteDb.Insert(newRow);
            return newRow.Id;
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



            this.SqliteDb.Insert(newColumn);
        }

        public bool DoesColumnExist(string name)
        {
            var row = this.SqliteDb.Query<TableRow>("SELECT * FROM 'TableColumn' WHERE Name = ?", name);
            return row.Count() > 0;
        }


        public dynamic GetEmptyRowAsJson()
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
            return this.SqliteDb.Query<TableRow>("SELECT * FROM TableRow WHERE " + whereQuery);
        }
        public TableRow GetTableRow(int id)
        {
            var row = this.SqliteDb.Query<TableRow>("SELECT * FROM TableRow WHERE Id = ?", id);
            return row.SingleOrDefault();
        }
        public dynamic GetRowDataAsJson(int id)
        {
            var row = this.SqliteDb.Query<TableRow>("SELECT Data FROM TableRow WHERE Id = ?", id);
            dynamic json = JValue.Parse(row.SingleOrDefault().Data);
            return json;
        }
        public dynamic GetRowDataAsJson(Guid uniqueId)
        {
            var row = this.SqliteDb.Query<TableRow>("SELECT Data FROM 'TableRow' WHERE UniqueId = ?", uniqueId.ToString());
            dynamic json = JValue.Parse(row.SingleOrDefault().Data);
            return json;
        }
        public List<TableColumn> GetColumns()
        {
            return this.SqliteDb.Query<TableColumn>("SELECT * FROM 'TableColumn'");
        }
        public List<TableRow> GetRows()
        {
            return this.SqliteDb.Query<TableRow>("SELECT * FROM 'TableRow'");
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
            this.SqliteDb.Update(foundRow);
        }
        public void UpdateColumn(TableColumn column)
        {
            this.SqliteDb.Update(column);
        }



        public void DeleteColumn(TableColumn column)
        {
            this.SqliteDb.Delete(column);
        }
        public void DeleteAllColumns()
        {
            //this.DeleteAllRows();
            this.SqliteDb.DeleteAll<TableColumn>();
        }
        public void DeleteAllRows()
        {
            this.SqliteDb.DeleteAll<TableRow>();
        }
        public void DeleteRow(int id)
        {
            this.SqliteDb.Delete(new TableRow() { Id = (int)id });
        }


    }
}
