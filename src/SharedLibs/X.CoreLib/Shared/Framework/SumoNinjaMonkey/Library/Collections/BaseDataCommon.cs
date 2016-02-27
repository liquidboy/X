namespace SumoNinjaMonkey.Framework.Collections
{
    public abstract class BaseDataCommon : BindableBase
    {
        private string _uniqueId = string.Empty;
        private string _title = string.Empty;
        public string UniqueId
        {
            get
            {
                return this._uniqueId;
            }
            set
            {
                base.SetProperty<string>(ref this._uniqueId, value, "UniqueId");
            }
        }
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                base.SetProperty<string>(ref this._title, value, "Title");
            }
        }
        public BaseDataCommon(string uniqueId, string title)
        {
            this._uniqueId = uniqueId;
            this._title = title;
        }
    }
}
