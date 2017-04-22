

using Windows.Foundation.Collections;
namespace SumoNinjaMonkey.Framework.Collections
{
    public class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        public CollectionChange CollectionChange
        {
            get;
            set;
        }
        public uint Index
        {
            get;
            set;
        }
        public VectorChangedEventArgs(CollectionChange type, uint index)
        {
            this.CollectionChange = type;
            this.Index = index;
        }
    }
}
