using System;
using System.Collections.Generic;
using System.Reflection;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public abstract class BaseEntity{
        [SQLite.PrimaryKey]
        public Guid UniqueId { get; set; }
        public int _internalRowId;
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
        }

        public int Save(T instance)
        {
            if (_isReadOnly) return 0;
            
            
            if (instance.UniqueId != Guid.Empty)
            {
                _table.UpdateEntity(instance.UniqueId, instance);
            }
            else
            {
                if (instance.UniqueId == Guid.Empty) instance.UniqueId = Guid.NewGuid();
                _table.AddEntity(instance);
            }
            
            return instance._internalRowId;
        }
        public T RetrieveEntity(Guid id)
        {
            return _table.GetEntity<T>(id);
        }
        public List<T> RetrieveEntities(string where)
        {
            return _table.GetEntities<T>(where);
        }
        public List<T> RetrieveAllEntities()
        {
            return _table.GetAllEntities<T>();
        }
        public void DeleteAllEntities()
        {
            _table.DeleteAllEntities<T>();
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

        public T Retrieve(int id)
        {
            throw new NotImplementedException();
        }

        public int Find(string whereQuery)
        {
            throw new NotImplementedException();
        }

        public int FindAll()
        {
            throw new NotImplementedException();
        }

        public void Delete(T instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
