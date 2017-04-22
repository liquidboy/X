
using System.Collections.ObjectModel;
namespace SumoNinjaMonkey.Framework.Collections
{
    public class BaseDataGroup : BaseDataCommon
    {
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get
            {
                return this._items;
            }
        }
        public BaseDataGroup(string uniqueId, string title)
            : base(uniqueId, title)
        {
        }
    }
}
