

using System;
using System.Collections;
using System.Collections.Generic;
namespace SumoNinjaMonkey.Framework.Collections
{
    public class VectorEnumerator<T> : IEnumerator<object>, IEnumerator, IDisposable
    {
        private IEnumerable<T> _Coll;
        private IEnumerator<T> _Enum;
        public object Current
        {
            get
            {
                return this._Enum.Current;
            }
        }
        public VectorEnumerator(IEnumerable<T> internalColl)
        {
            this._Coll = internalColl;
            this._Enum = this._Coll.GetEnumerator();
        }
        public bool MoveNext()
        {
            return this._Enum.MoveNext();
        }
        public void Reset()
        {
            this._Enum.Reset();
        }
        public void Dispose()
        {
            this._Enum.Dispose();
        }
    }
}
