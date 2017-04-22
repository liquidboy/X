
using System.Collections.ObjectModel;
namespace SumoNinjaMonkey.Framework.Collections
{
    public class BaseDataSource
    {
        public ObservableCollection<BaseDataGroup> ItemGroups
        {
            get;
            set;
        }
    }
}
