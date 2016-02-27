namespace FavouriteMX.Shared.Models
{
    public class AppContext
    {
        public static readonly AppContext Current = new AppContext();
        private bool _isLoaded;
        public void Load()
        {
            if (this._isLoaded)
            {
                return;
            }
            this._isLoaded = true;
        }
    }
}
