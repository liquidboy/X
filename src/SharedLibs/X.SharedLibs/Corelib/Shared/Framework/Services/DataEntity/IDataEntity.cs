using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{

    public interface IDataEntity<T>
    {
        int Save(T instance);
        T RetrieveEntity(Guid id);
        List<T> RetrieveEntities(string where);
        T Retrieve(int id);
        int Find(string whereQuery);
        int FindAll();
        void Delete(T instance);
        void Delete(int id);
        void DeleteAll();
    }
}
