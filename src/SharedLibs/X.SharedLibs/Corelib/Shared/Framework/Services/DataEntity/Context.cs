using SumoNinjaMonkey.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using X.CoreLib.SQLite;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{

    [DefaultProperty("Current")]
    public class DBContext
    {
        private static DBContext _instance = null;
        private static object lockobj = new object();

        public static DBContext Current
        {
            get
            {
                DBContext result;
                lock (lockobj)
                {
                    if (_instance == null)
                    {
                        _instance = new DBContext("xappdbs");
                    }
                    result = _instance;
                }
                return result;
            }
        }

        public DBManager Manager { get { return DBManager.Current; } }

        private Dictionary<string, object> _entities;

        private DBContext(string instanceName)
        {
            _entities = new Dictionary<string, object>();
            // todo :   use reflection to go through ALL classes that inherity abstract class BaseEntity
            //          and Register that entity
            var typesFound = ReflectiveEnumerator.GetEnumerableOfType<BaseEntity>();
            foreach (var type in typesFound) {                
                MethodInfo method = typeof(DBContext).GetMethod("RegisterContext");
                MethodInfo generic = method.MakeGenericMethod(type.GetType());
                generic.Invoke(this, null);
            }
            
        }
        
        public bool DoesContextExist(string name) { return _entities.ContainsKey(name); }
        public bool DoesContextExist<T>() { return _entities.ContainsKey(typeof(T).Name); }

        public void RegisterContext<T>()
        {
            //eg. inline c# looks like this
            //      public class OrderHeaderContext : DataEntity<OrderHeader> { }
            //codify it looks like ...

            if (!DoesContextExist<T>()) {
                var d1 = typeof(DataEntity<>);
                Type[] typeArgs = { typeof(T) };
                var makeme = d1.MakeGenericType(typeArgs);
                object o = Activator.CreateInstance(makeme);
                _entities.Add(typeof(T).Name, o);
            }
        }

        private IDataEntity<T> retrieveContext<T>() {
            return (IDataEntity<T>)_entities[typeof(T).Name];
        }

        public int Save<T>(T entityToSave) {
            return retrieveContext<T>().Save(entityToSave);
        }

        //public T Retrieve<T>(int idToRetrieve) {
        //    return retrieveContext<T>().Retrieve(idToRetrieve);
        //}
        public T RetrieveEntity<T>(Guid idToRetrieve)
        {
            return retrieveContext<T>().RetrieveEntity(idToRetrieve);
        }
        public List<T> RetrieveEntities<T>(string where)
        {
            return retrieveContext<T>().RetrieveEntities(where);
        }
        public List<T> RetrieveAllEntities<T>()
        {
            return retrieveContext<T>().RetrieveAllEntities();
        }
        //public int Find<T>(string query)
        //{
        //    return retrieveContext<T>().Find(query);
        //}

        //public int Find<T>(string query, params object[] args) where T : new()
        //{
        //    return retrieveContext<T>().Find<T>(query, args);
        //}

        public void DeleteAll<T>() {
            retrieveContext<T>().DeleteAll();
            retrieveContext<T>().DeleteAllEntities();
        }

        public void Delete<T>(int idToDelete) {
            retrieveContext<T>().Delete(idToDelete);
        }

        public void DeleteEntity<T>(Guid guid) {
            retrieveContext<T>().DeleteEntity(guid);
        }
    }


    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try {
                    foreach (var type in assembly.GetTypes()
                        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
                    {
                        objects.Add((T)Activator.CreateInstance(type, constructorArgs));
                    }
                }
                catch { }
            }
            

            //foreach (Type type in
            //    //Assembly.GetAssembly(typeof(T)).GetTypes()  <== ONLY THIS ASSEMBLY
            //    AppDomain.CurrentDomain.GetAssemblies().SelectMany(a=> a.GetTypes())  // <== ACROSS ALL LOADED ASSEMBLIES
            //    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            //{
            //    objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            //}
            //objects.Sort();
            return objects;
        }
    }
}
