using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public class SQLiteDataEntity
    {

        public Guid UniqueId { get; set; }
        private int _internalRowId;

        string _tableName;
        public SQLiteDataEntity()
        {
            _tableName = this.GetType().Name;
            InitEntityDatabase();
        }

        public void InitEntityDatabase()
        {
            AppDatabase.Current.AddTable(_tableName, "jose");
            var table = AppDatabase.Current.Tables[_tableName];

            table.DeleteAllColumns();


            var props = this.GetType().GetTypeInfo().DeclaredProperties;
            foreach (var prop in props)
            {
                table.AddColumn(prop.Name, "jose");
            }

            table.AddColumn("UniqueId", "jose");
        }

        public int Save()
        {
            var table = AppDatabase.Current.Tables[_tableName];

            if (UniqueId == Guid.Empty) UniqueId = Guid.NewGuid();

            if (_internalRowId > 0)
            {
                //update
                var udo = table.GetRowDataAsJson(_internalRowId);

                var props = this.GetType().GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    udo[prop.Name] = prop.GetValue(this, null).ToString();
                }
                udo["UniqueId"] = UniqueId.ToString();
            }
            else
            {
                //add

                var udo = table.GetEmptyRowAsJson();

                var props = this.GetType().GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    udo[prop.Name] = prop.GetValue(this, null).ToString();
                }

                udo["UniqueId"] = UniqueId.ToString();

                _internalRowId = table.AddRow(udo, "jose");
            }





            return _internalRowId;
        }
        public bool Retrieve(int id)
        {

            clear();

            try
            {
                var table = AppDatabase.Current.Tables[_tableName];
                var udo = table.GetRowDataAsJson(id);


                _internalRowId = id;

                var props = this.GetType().GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "String": prop.SetValue(this, (string)udo[prop.Name]); break;
                        case "Guid": prop.SetValue(this, Guid.Parse((string)udo[prop.Name])); break;
                        case "DateTime": prop.SetValue(this, DateTime.Parse((string)udo[prop.Name])); break;
                        case "Long": prop.SetValue(this, long.Parse((string)udo[prop.Name])); break;
                        case "Int": prop.SetValue(this, int.Parse((string)udo[prop.Name])); break;
                        case "Double": prop.SetValue(this, Double.Parse((string)udo[prop.Name])); break;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                _internalRowId = 0;
                UniqueId = Guid.Empty;
            }

            return false;
        }

        public List<TableRow> FindResult;
        public int Find(string whereQuery)
        {
            var table = AppDatabase.Current.Tables[_tableName];
            FindResult = table.Find(whereQuery);
            return FindResult.Count;
        }
        public void Delete()
        {
            var table = AppDatabase.Current.Tables[_tableName];

            if (_internalRowId > 0)
            {
                table.DeleteRow(_internalRowId);
            }

            clear();
        }
        public void Delete(int id)
        {
            var table = AppDatabase.Current.Tables[_tableName];
            table.DeleteRow(id);
        }
        public void DeleteAll()
        {
            var table = AppDatabase.Current.Tables[_tableName];
            table.DeleteAllRows();
        }

        private void clear()
        {
            var props = this.GetType().GetTypeInfo().DeclaredProperties;
            foreach (var prop in props)
            {
                prop.SetValue(this, null);
            }

            this._internalRowId = 0;
            this.UniqueId = Guid.Empty;
        }
    }
}
