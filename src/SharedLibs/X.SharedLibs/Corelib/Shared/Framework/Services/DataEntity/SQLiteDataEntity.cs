using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public class BaseEntity {
        public Guid UniqueId { get; set; }
        public int internalRowId;
    }

    public class SQLiteDataEntity<T> where T : BaseEntity, new()
    {
        public string _defaultCreator = "admin";
        // public Guid UniqueId { get; set; }
        // private int _internalRowId;

        string _tableName;
        public SQLiteDataEntity() { SQLiteDataEntityImpl(true); }
        public SQLiteDataEntity(bool initDb) { SQLiteDataEntityImpl(initDb); }
        private void SQLiteDataEntityImpl(bool initDb = true)
        {
            _tableName = typeof(T).Name;
            if (initDb) { InitEntityDatabase(); }
        }


        public static SQLiteDataEntity<T> Create() {
            return new SQLiteDataEntity<T>();
        }
        

        //todo: optimization on columns to determine if they changed and thus do a DeleteAllColumns and rebuild
        private void InitEntityDatabase()
        {
            AppDatabase.Current.AddTable(_tableName, _defaultCreator);
            var table = AppDatabase.Current.Tables[_tableName];

            table.DeleteAllColumns();  //note: we delete all columns and rebuild them from the current class to ensure new columns exist

            var props = typeof(T).GetTypeInfo().DeclaredProperties;
            foreach (var prop in props)
            {
                table.AddColumn(prop.Name, _defaultCreator);
            }

            table.AddColumn("UniqueId", _defaultCreator);
        }

        public int Save(T instance)
        {
            var table = AppDatabase.Current.Tables[_tableName];

            if (instance.UniqueId == Guid.Empty) instance.UniqueId = Guid.NewGuid();

            if (instance.internalRowId > 0)
            {
                //update
                var udo = table.GetRowDataAsJson(instance.internalRowId);

                var props = typeof(T).GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    udo[prop.Name] = prop.GetValue(instance, null).ToString();
                }
                udo["UniqueId"] = instance.UniqueId.ToString();
            }
            else
            {
                //add

                var udo = table.GetEmptyRowAsJson();

                var props = typeof(T).GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    udo[prop.Name] = prop.GetValue(instance, null).ToString();
                }

                udo["UniqueId"] = instance.UniqueId.ToString();

                instance.internalRowId = table.AddRow(udo, _defaultCreator);
            }
            
            return instance.internalRowId;
        }
        public T Retrieve(int id)
        {
            T obj = new T();

            // clear();

            try
            {
                var table = AppDatabase.Current.Tables[_tableName];
                var udo = table.GetRowDataAsJson(id);

                obj.internalRowId = id;
                
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
                obj.internalRowId = 0;
                obj.UniqueId = Guid.Empty;
            }

            return obj;
        }

        public List<TableRow> FindResult;
        public int Find(string whereQuery)
        {
            var table = AppDatabase.Current.Tables[_tableName];
            FindResult = table.Find(whereQuery);
            return FindResult.Count;
        }
        public int FindAll()
        {
            var table = AppDatabase.Current.Tables[_tableName];
            FindResult = table.GetRows();
            return FindResult.Count;
        }
        public void Delete(T instance)
        {
            var table = AppDatabase.Current.Tables[_tableName];

            if (instance.internalRowId > 0)
            {
                table.DeleteRow(instance.internalRowId);
            }

            clear(instance);
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

        private void clear(T instance)
        {
            var props = typeof(T).GetTypeInfo().DeclaredProperties;
            foreach (var prop in props)
            {
                prop.SetValue(instance, null);
            }

            instance.internalRowId = 0;
            instance.UniqueId = Guid.Empty;
        }
    }
}
