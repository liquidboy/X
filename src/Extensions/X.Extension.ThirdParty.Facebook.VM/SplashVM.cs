using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Windows.Security.Authentication.Web;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using X.Services.Data;

namespace X.Extension.ThirdParty.Facebook.VM
{
    public class SplashVM : ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public string GroupingType { get; set; } = "Facebook";

        APIKeyDataModel apiKey;
        private string _accessToken = "";
        private DateTime _tokenExpiry;


        FacebookClient _client;
        //https://blogs.msdn.microsoft.com/wsdevsol/2015/02/12/integrating-facebook-authentication-in-universal-windows-apps/
        //http://bsubramanyamraju.blogspot.com.au/2014/12/windowsphone-store-81-facebook.html
        //https://developers.facebook.com/docs/


        private RelayCommand<string> _requestLogin;

        public RelayCommand<string> RequestLogin { get { return _requestLogin ?? (_requestLogin = new RelayCommand<string>((arg) => { AttemptLogin(); })); } }

        Visibility _IsLoginVisible;
        public Visibility IsLoginVisible { get { return _IsLoginVisible; } set { _IsLoginVisible = value; RaisePropertyChanged(); } }

        Visibility _IsAPIEditorVisible;
        public Visibility IsAPIEditorVisible { get { return _IsAPIEditorVisible; } set { _IsAPIEditorVisible = value; RaisePropertyChanged(); } }

        Visibility _IsLoggedInVisible;
        public Visibility IsLoggedInVisible { get { return _IsLoggedInVisible; } set { _IsLoggedInVisible = value; RaisePropertyChanged(); } }

        string _ProfileImageUrl;
        public string ProfileImageUrl { get { return _ProfileImageUrl; } set { _ProfileImageUrl = value; RaisePropertyChanged(); } }

        string _UserId;
        public string UserId { get { return _UserId; } set { _UserId = value; RaisePropertyChanged(); } }

        string _UserName;
        public string UserName { get { return _UserName; } set { _UserName = value; RaisePropertyChanged(); } }

        dynamic facebookStatuses;
        public dynamic FacebookStatuses { get { return facebookStatuses; } set { facebookStatuses = value; RaisePropertyChanged(); } }


        public SplashVM()
        {
            IsLoginVisible = Visibility.Collapsed;
            IsAPIEditorVisible = Visibility.Visible;
            IsLoggedInVisible = Visibility.Collapsed;
            GetAPIData();
            PopulatePassportData();
        }

        private void GetAPIData()
        {
            var apis = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>();
            if (apis != null && apis.Count > 0) apiKey = apis.Where(x => x.Type == GroupingType).FirstOrDefault();
        }

        private async void PopulatePassportData()
        {
            
            //if theres a filled in passport then user is already logged in
            var data = StorageService.Instance.Storage.RetrieveList<PassportDataModel>().Where(x=>x.PassType == GroupingType).ToList();
            if (data != null && data.Count > 0) { 
                var dm = data.Where(x => x.PassType == GroupingType).FirstOrDefault();
                if (dm != null)
                {
                    //delete if necessar
                    //StorageService.Instance.Storage.DeleteById<PassportDataModel>(dm.Id.ToString());

                    _accessToken = dm.Token;
                    //_tokenExpiry = dm.TokenExpiry;


                    IsLoginVisible = Visibility.Collapsed;
                    IsAPIEditorVisible = Visibility.Collapsed;
                    IsLoggedInVisible = Visibility.Visible;

                    await GetUserAsync();
                    ProfileImageUrl =  GetProfileImageUrl(UserId);
                    await GetUserFeedAsync(UserId);
                    return;
                }
            }

            //no filled in passport so user is NOT logged in
            IsLoggedInVisible = Visibility.Collapsed;

            if (apiKey != null && !string.IsNullOrEmpty(apiKey.APIKey))
            {
                //an apikey has been filled in
                IsLoginVisible = Visibility.Visible;
                IsAPIEditorVisible = Visibility.Collapsed;
            }
            else
            {
                //an apikey has NOT been filled in
                IsLoginVisible = Visibility.Collapsed;
                IsAPIEditorVisible = Visibility.Visible;
            }
        }

        private async void AttemptLogin() {
            if (_client != null) return;
            if (apiKey == null) return;

            _client = new FacebookClient();
            _client.AppId = apiKey.APIKey;
            _client.AppSecret = apiKey.APISecret;

            //var scope = "public_profile, email";
            var scope = "public_profile,user_friends,email, user_about_me, user_hometown, user_location, user_photos, user_posts, user_status, user_videos, user_website";

            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var fb = new FacebookClient();
            Uri loginUrl = fb.GetLoginUrl(new
            {
                client_id = apiKey.APIKey,
                redirect_uri = redirectUri,
                response_type = "token",
                scope = scope
            });

            Uri startUri = loginUrl;
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            ParseAuthenticationResult(result);


        }

        private string GetProfileImageUrl(string userId, string type = "square") {
            return $"https://graph.facebook.com/{userId}/picture?type={type}&access_token={_accessToken}";
        }

        private async Task GetUserAsync()
        {
            _client = new FacebookClient(_accessToken);
            dynamic result = await _client.GetTaskAsync("me");
            UserId = result.id;
            UserName = result.name;
        }

        private async Task GetUserFeedAsync(string userid)
        {
            _client = new FacebookClient(_accessToken);
            //dynamic result = await _client.GetTaskAsync("me/feed");
            dynamic result = await _client.GetTaskAsync($"{userid}/feed");
            if (result.Count == 2) {
                FacebookStatuses = result[0];
            }
        }

        public void ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    //Debug.WriteLine("Error");
                    break;
                case WebAuthenticationStatus.Success:
                    var pattern = string.Format("{0}#access_token={1}&expires_in={2}", WebAuthenticationBroker.GetCurrentApplicationCallbackUri(), "(?<access_token>.+)", "(?<expires_in>.+)");
                    var match = Regex.Match(result.ResponseData, pattern);

                    var access_token = match.Groups["access_token"];
                    var expires_in = match.Groups["expires_in"];

                    _accessToken = access_token.Value;
                    _tokenExpiry = DateTime.Now.AddSeconds(double.Parse(expires_in.Value));
                    
                    var dm = new PassportDataModel();
                    dm.Token = _accessToken;
                    dm.TokenExpiry = _tokenExpiry;
                    //dm.TokenSecret = AccessToken.TokenSecret;
                    //dm.Verifier = xoauth_verifier;
                    dm.PassType = GroupingType;
                    //dm.UserId = AccessToken.UserId;
                    //dm.UserName = AccessToken.Username;
                    //dm.FullName = AccessToken.FullName;
                    //dm.ScreenName = AccessToken.ScreenName;

                    dm.APIKeyFKID = apiKey.Id;
                    
                    StorageService.Instance.Storage.Insert(dm);
                    
                    IsLoggedInVisible = Visibility.Visible;
                    IsLoginVisible = Visibility.Collapsed;
                    IsAPIEditorVisible = Visibility.Collapsed;
                    
                    break;
                case WebAuthenticationStatus.UserCancel:
                    //Debug.WriteLine("Operation aborted");
                    break;
                default:
                    break;
            }
        }
    }
}
