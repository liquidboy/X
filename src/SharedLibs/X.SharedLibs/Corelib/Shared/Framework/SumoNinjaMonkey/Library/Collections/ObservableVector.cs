
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
namespace SumoNinjaMonkey.Framework.Collections
{
    public class ObservableVector<T> : ObservableCollection<T>, IObservableVector<object>, IList<object>, ICollection<object>, IEnumerable<object>, IEnumerable , ISupportIncrementalLoading
    {
        
        private VectorEnumerator<T> _Enum;
        public event VectorChangedEventHandler<object> VectorChanged;


        public new object this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = (T)value;
                this.RaiseChange(CollectionChange.ItemChanged, (uint)index);
            }
        }
        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public ObservableVector()
		{
			//this.VectorChanged = new EventRegistrationTokenTable<VectorChangedEventHandler<object>>();
			//base..ctor();
		}
        public ObservableVector(IEnumerable<T> list)
		{
			//this.VectorChanged = new EventRegistrationTokenTable<VectorChangedEventHandler<object>>();
			//base..ctor();
			foreach (T current in list)
			{
				base.Add(current);
			}
		}
        public int IndexOf(object item)
        {
            return base.IndexOf((T)item);
        }
        public void Insert(int index, object item)
        {
            base.Insert(index, (T)item);
            this.RaiseChange(CollectionChange.ItemInserted, (uint)index);
        }
        private void RaiseChange(CollectionChange type, uint index)
        {
            if (this.VectorChanged != null) this.VectorChanged(this, new VectorChangedEventArgs(type, index));
            
        }
        public void Add(object item)
        {
            
            base.Add((T)item);
            
            this.RaiseChange(CollectionChange.ItemInserted, (uint)(base.Count - 1));
        }
        public bool Contains(object item)
        {
            return base.Contains((T)item);
        }
        public void CopyTo(object[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public bool Remove(object item)
        {
            int num = base.IndexOf((T)item);
            if (num > 0)
            {
                bool result = base.Remove((T)item);
                this.RaiseChange(CollectionChange.ItemRemoved, (uint)num);
                return result;
            }
            return false;
        }
        public new IEnumerator<object> GetEnumerator()
        {
            if (this._Enum == null)
            {
                this._Enum = new VectorEnumerator<T>(this);
            }
            return this._Enum;
        }



        public bool HasMoreItems
        {
            get { return true; }
        }

        public Delegate LoadMoreMethod;

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run<LoadMoreItemsResult>(() =>
            {
                LoadMoreMethod.DynamicInvoke();
                return new LoadMoreItemsResult() { Count = count };
                
            }).AsAsyncOperation();

        }
    }
}
