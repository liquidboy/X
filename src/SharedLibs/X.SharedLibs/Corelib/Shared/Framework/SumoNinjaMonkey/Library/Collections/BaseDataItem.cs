

namespace SumoNinjaMonkey.Framework.Collections
{
    public class BaseDataItem : BaseDataCommon
    {
        private string _content = string.Empty;
        private BaseDataGroup _group;
        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }
        public BaseDataGroup Group
        {
            get
            {
                return this._group;
            }
            set
            {
                this._group = value;
            }
        }
        public BaseDataItem(string uniqueId, string title, string content, BaseDataGroup group)
            : base(uniqueId, title)
        {
            this._content = content;
            this._group = group;
        }
    }
}
