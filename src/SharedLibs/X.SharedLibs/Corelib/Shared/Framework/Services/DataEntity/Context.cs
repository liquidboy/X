using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using X.CoreLib.SQLite;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{

    public class Context
    {
        private static Context _instance = null;
        private static object lockobj = new object();

        public static Context Current
        {
            get
            {
                Context result;
                lock (lockobj)
                {
                    if (_instance == null)
                    {
                        _instance = new Context("xappdbs");
                    }
                    result = _instance;
                }
                return result;
            }
        }

        private Dictionary<string, object> _entities;

        private Context(string instanceName)
        {
            _entities = new Dictionary<string, object>();
            // todo :   use reflection to go through ALL classes that inherity abstract class BaseEntity
            //          and Register that entity
            // var typesFound = ReflectiveEnumerator.GetEnumerableOfType<BaseEntity>();
        }
        
        public bool DoesEntityExist(string name) { return _entities.ContainsKey(name); }
        public void RegisterEntity<T>()
        {
            //eg. inline c# looks like this
            //      public class OrderHeaderContext : DataEntity<OrderHeader> { }
            //codify it looks like ...
            var name = typeof(T).Name;
            if (!DoesEntityExist(name)) {
                var d1 = typeof(DataEntity<>);
                Type[] typeArgs = { typeof(T) };
                var makeme = d1.MakeGenericType(typeArgs);
                object o = Activator.CreateInstance(makeme);
                _entities.Add(name, o);
            }
        }

        public int Save<T>(T entityToSave) {
            var name = typeof(T).Name;
            dynamic ctx = _entities[name];
            return ctx.Save(entityToSave);
        }
        public T Retrieve<T>(int idToRetrieve) {
            var name = typeof(T).Name;
            dynamic ctx = _entities[name];
            return ctx.Retrieve(idToRetrieve);
        }
        public int Find<T>(string query)
        {
            var name = typeof(T).Name;
            dynamic ctx = _entities[name];
            return ctx.Find(query);
        }
        public void DeleteAll<T>() {
            var name = typeof(T).Name;
            dynamic ctx = _entities[name];
            ctx.DeleteAll();
        }
        public void Delete<T>(int idToDelete) {
            var name = typeof(T).Name;
            dynamic ctx = _entities[name];
            ctx.Delete(idToDelete);
        }

    }


    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                //Assembly.GetAssembly(typeof(T)).GetTypes()  <== ONLY THIS ASSEMBLY
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(a=> a.GetTypes())  // <== ACROSS ALL LOADED ASSEMBLIES
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            //objects.Sort();
            return objects;
        }
    }
}
