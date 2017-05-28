using FlickrNet;
using GalaSoft.MvvmLight;
using SumoNinjaMonkey.Framework.Controls.Messages;
using System.Linq;
using System.Threading;
using X.CoreLib.Shared.Services;
using X.NeonShell.Services;

namespace X.NeonShell.ViewModels
{
    public class DefaultViewModel : ViewModelBase
    {
        public OAuthAccessToken _atFlickr;
        public FlickrNet.Flickr _flickr = null;
        private string apiKey = "";
        private string apiSecret = "";

        public OAuthAccessToken _atTwitter;
        public FlickrNet.Twitter _twitter = null;
        private string apiKey2 = "";
        private string apiSecret2 = "";


        private CancellationTokenSource _OneFlickrCallAtATimeCancellationToken;

        public Person FlickrPerson { get; set; }

        internal CancellationTokenSource MakeIdemniPotentAsyncCall()
        {
            _OneFlickrCallAtATimeCancellationToken?.Cancel();
            var cancellationTokenSource = new CancellationTokenSource();
            _OneFlickrCallAtATimeCancellationToken = cancellationTokenSource;
            return cancellationTokenSource;
        }

        public string FullName { get { return (_atFlickr == null) ? "Anonymous" : _atFlickr.FullName; } }
        public string ScreenName { get { return (_atFlickr == null) ? "" : _atFlickr.ScreenName; } }
        public string Username { get { return (_atFlickr == null) ? "Anonymous" : _atFlickr.Username; } }

        private string _BuddyIconUrl;
        public string BuddyIconUrl { get { return _BuddyIconUrl; } set { _BuddyIconUrl = value; RaisePropertyChanged("BuddyIconUrl"); } }


        private string _BuddyIconUrlTwitter;
        public string BuddyIconUrlTwitter { get { return _BuddyIconUrlTwitter; } set { _BuddyIconUrlTwitter = value; RaisePropertyChanged("BuddyIconUrlTwitter"); } }


        public bool IsAnonymous { get { return (FullName == "Anonymous") ? true : false; } }

        public DefaultViewModel()
        {
            apiKey = AppService.AppSetting.FlickrKey;
            apiSecret = AppService.AppSetting.FlickrSecret;
            
            _flickr = new FlickrNet.Flickr(apiKey, apiSecret);
            _twitter = new FlickrNet.Twitter(apiKey2, apiSecret2);

        }

        public void RaisePropertyChangedBubble(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

        public void ContextMenuBox(
           string message,
           string title,
           ContextMenuService.ContextMenuType type,
           GeneralSystemWideMessage msgToPassAlong = null)
        {
            ContextMenuService.Show(
                message,
                title,
                type,
                msgToPassAlong: msgToPassAlong
            );
        }


        public void ViewInit()
        {
            if (IsFlickrLoginDetailsCached())
            {
                _atFlickr = new OAuthAccessToken();
                var found = AppDatabase.Current.AppStates.Where(x => x.Name == "atFlickr.FullName").FirstOrDefault();
                _atFlickr.FullName = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atFlickr.ScreenName").FirstOrDefault();
                _atFlickr.ScreenName = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atFlickr.UserId").FirstOrDefault();
                _atFlickr.UserId = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atFlickr.Username").FirstOrDefault();
                _atFlickr.Username = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atFlickr.Token").FirstOrDefault();
                _atFlickr.Token = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atFlickr.TokenSecret").FirstOrDefault();
                _atFlickr.TokenSecret = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "fp.BuddyIconUrl").FirstOrDefault();
                BuddyIconUrl = found != null ? found.Value : string.Empty;

                _flickr.OAuthAccessToken = _atFlickr.Token;
                _flickr.OAuthAccessTokenSecret = _atFlickr.TokenSecret;

            }


            if (IsTwitterLoginDetailsCached())
            {
                _atTwitter = new OAuthAccessToken();
                var found = AppDatabase.Current.AppStates.Where(x => x.Name == "atTwitter.FullName").FirstOrDefault();
                _atTwitter.FullName = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atTwitter.ScreenName").FirstOrDefault();
                _atTwitter.ScreenName = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atTwitter.UserId").FirstOrDefault();
                _atTwitter.UserId = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atTwitter.Username").FirstOrDefault();
                _atTwitter.Username = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atTwitter.Token").FirstOrDefault();
                _atTwitter.Token = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "atTwitter.TokenSecret").FirstOrDefault();
                _atTwitter.TokenSecret = found.Value;
                found = AppDatabase.Current.AppStates.Where(x => x.Name == "tp.BuddyIconUrl").FirstOrDefault();
                BuddyIconUrlTwitter = found != null ? found.Value : string.Empty;

                _twitter.OAuthAccessToken = _atTwitter.Token;
                _twitter.OAuthAccessTokenSecret = _atTwitter.TokenSecret;

            }
        }


        public bool IsFlickrLoginDetailsCached()
        {
            var found = AppDatabase.Current.AppStates?.Where(x => x.Name == "atFlickr.FullName").FirstOrDefault();
            if (found != null)
            {
                return true;
            }

            return false;
        }

        public bool IsTwitterLoginDetailsCached()
        {
            var found = AppDatabase.Current.AppStates?.Where(x => x.Name == "atTwitter.FullName").FirstOrDefault();
            if (found != null)
            {
                return true;
            }

            return false;
        }
    }
}
