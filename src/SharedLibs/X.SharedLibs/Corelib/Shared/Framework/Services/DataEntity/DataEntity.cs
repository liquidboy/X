using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public abstract class BaseEntity{
        public Guid UniqueId { get; set; }
        public int _internalRowId;
        public int _internalEntityId;

    }

    public class DataEntity<T>  : IDataEntity<T>
        where T : BaseEntity, new()
    {
        public string _defaultCreator = "admin";
        private bool _isReadOnly = false;
        
        string _tableName;
        TableDatabase _table;

        public DataEntity() { SQLiteDataEntityImpl(true); }
        public DataEntity(bool initDb, bool isReadOnly) { SQLiteDataEntityImpl(initDb, isReadOnly); }
        private void SQLiteDataEntityImpl(bool initDb = true, bool isReadOnly = false)
        {
            _tableName = typeof(T).Name;
            _isReadOnly = isReadOnly;
            if (initDb) { InitEntityDatabase(); }
            else { _table = AppDatabase.Current.Tables[_tableName]; }
            //Context.Current.RegisterEntity<T>();
        }


        public static DataEntity<T> Create() {
            return new DataEntity<T>();
        }
        public static DataEntity<T> CreateReadOnly()
        {
            return new DataEntity<T>(false, true);
        }

        //todo: optimization on columns to determine if they changed and thus do a DeleteAllColumns and rebuild
        private void InitEntityDatabase()
        {
            AppDatabase.Current.AddTable(_tableName, _defaultCreator);
            _table = AppDatabase.Current.Tables[_tableName];

            _table.DeleteAllColumns();  //note: we delete all columns and rebuild them from the current class to ensure new columns exist

            var props = typeof(T).GetTypeInfo().DeclaredProperties;
            foreach (var prop in props)
            {
                _table.AddColumn(prop.Name, _defaultCreator);
            }

            _table.AddColumn("UniqueId", _defaultCreator);
        }

        public int Save(T instance)
        {
            if (_isReadOnly) return 0;
            
            if (instance.UniqueId == Guid.Empty) instance.UniqueId = Guid.NewGuid();

            if (instance._internalRowId > 0)
            {
                //update
                var udo = _table.GetRowDataAsJson(instance._internalRowId);

                var props = typeof(T).GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    udo[prop.Name] = prop.GetValue(instance, null).ToString();
                }
                udo["UniqueId"] = instance.UniqueId.ToString();
                _table.UpdateRowData(instance._internalRowId, udo);
                _table.UpdateEntity(instance._internalEntityId, instance);
            }
            else
            {
                //add

                var udo = _table.GetEmptyRowAsJson();

                var props = typeof(T).GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    udo[prop.Name] = prop.GetValue(instance, null).ToString();
                }

                udo["UniqueId"] = instance.UniqueId.ToString();

                instance._internalRowId = _table.AddRow(udo.ToString(), _defaultCreator);
                instance._internalEntityId = _table.AddEntity(instance);
            }
            
            return instance._internalRowId;
        }
        public T Retrieve(int id)
        {
            T obj = new T();

            // clear();

            try
            {
                var udo = _table.GetRowDataAsJson(id);

                obj._internalRowId = id;
                
                var props = typeof(T).GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "String": prop.SetValue(obj, (string)udo[prop.Name]); break;
                        case "Guid": prop.SetValue(obj, Guid.Parse((string)udo[prop.Name])); break;
                        case "DateTime": prop.SetValue(obj, DateTime.Parse((string)udo[prop.Name])); break;
                        case "Long": prop.SetValue(obj, long.Parse((string)udo[prop.Name])); break;
                        case "Int": prop.SetValue(obj, int.Parse((string)udo[prop.Name])); break;
                        case "Double": prop.SetValue(obj, Double.Parse((string)udo[prop.Name])); break;
                    }
                }
                obj.UniqueId = Guid.Parse((string)udo["UniqueId"]);

                return obj;
            }
            catch (Exception ex)
            {
                obj._internalRowId = 0;
                obj.UniqueId = Guid.Empty;
            }

            return obj;
        }

        public List<TableRow> FindResult;
        public int Find(string whereQuery)
        {
            FindResult = _table.Find(whereQuery);
            return FindResult.Count;
        }
        public int FindAll()
        {
            FindResult = _table.GetRows();
            return FindResult.Count;
        }
        public void Delete(T instance)
        {
            if (_isReadOnly) return;

            if (instance._internalRowId > 0)
            {
                _table.DeleteRow(instance._internalRowId);
            }

            clear(instance);
        }
        public void Delete(int id)
        {
            if (_isReadOnly) return;
            _table.DeleteRow(id);
        }
        public void DeleteAll()
        {
            _table.DeleteAllRows();
        }

        private void clear(T instance)
        {
            var props = typeof(T).GetTypeInfo().DeclaredProperties;
            foreach (var prop in props)
            {
                prop.SetValue(instance, null);
            }

            instance._internalRowId = 0;
            instance.UniqueId = Guid.Empty;
        }
    }
}
