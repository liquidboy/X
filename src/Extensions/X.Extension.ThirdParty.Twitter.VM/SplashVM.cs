using FlickrNet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using X.Services.Data;
using X.UI.LiteTab;

namespace X.Extension.ThirdParty.Twitter.VM
{
    public class SplashVM :ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public string GroupingType { get; set; } = "Twitter";

        private List<Status> _Tweets;
        public List<Status> Tweets { get { return _Tweets; } set { _Tweets = value; RaisePropertyChanged(); TweetUsers = _Tweets.Select(x => x.User.ProfileImageUrl).Distinct().ToList();  } }

        private List<string> _TweetUsers;
        public List<string> TweetUsers { get { return _TweetUsers; } set { _TweetUsers = value; RaisePropertyChanged(); } }

        public TwitterContext _twitterCtx = null;
        
        APIKeyDataModel apiKey;

        User _LoggedInUser;
        Visibility _IsLoggedInVisibility;
        Visibility _IsLoginVisible;
        Visibility _IsAPIEditorVisible;


        public Visibility IsAPIEditorVisible { get { return _IsAPIEditorVisible; } set { _IsAPIEditorVisible = value; RaisePropertyChanged(); } }
        public Visibility IsLoginVisible { get { return _IsLoginVisible; } set { _IsLoginVisible = value; RaisePropertyChanged(); } }
        public Visibility IsLoggedInVisibility { get { return _IsLoggedInVisibility; } set { _IsLoggedInVisibility = value; RaisePropertyChanged(); } }
        public User LoggedInUser { get { return _LoggedInUser; } set { _LoggedInUser = value; RaisePropertyChanged(); } }



        private RelayCommand<string> _requestTwitterLogin;

        public RelayCommand<string> RequestTwitterLogin
        {
            get
            {
                return _requestTwitterLogin ?? (_requestTwitterLogin = new RelayCommand<string>((arg) => { AttemptTwitterLogin(); }));
            }
        }


        public SplashVM() {
            IsLoginVisible = Visibility.Collapsed;
            IsAPIEditorVisible = Visibility.Visible;
            IsLoggedInVisibility = Visibility.Collapsed;
            GetAPIData();
            PopulatePassportData();
        }

        private void GetAPIData() {
            var apis = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>();
            if (apis != null && apis.Count > 0) apiKey = apis.Where(x => x.Type == GroupingType).FirstOrDefault();

            if (apiKey == null) IsAPIEditorVisible = Visibility.Visible;
            else IsAPIEditorVisible = Visibility.Collapsed;
        }


        private async void PopulatePassportData()
        {
            if (apiKey == null) { IsLoginVisible = Visibility.Visible; return; }

            var data = StorageService.Instance.Storage.RetrieveList<PassportDataModel>();
            if (data != null && data.Count > 0)
            {
                IsLoginVisible = Visibility.Visible;
                IsAPIEditorVisible = Visibility.Collapsed;
                IsLoggedInVisibility = Visibility.Collapsed;

                var dm = data.Where(x => x.PassType == GroupingType).FirstOrDefault();
                if (dm != null) {
                    
                    if (_twitterCtx == null)
                    {
                        var authorizer = new UniversalAuthorizer
                        {
                            CredentialStore = new SingleUserInMemoryCredentialStore
                            {
                                AccessToken = dm.Token,
                                AccessTokenSecret = dm.TokenSecret,
                                ConsumerKey = apiKey.APIKey,
                                ConsumerSecret = apiKey.APISecret
                            },
                            SupportsCompression = true,
                            Callback = apiKey.APICallbackUrl
                        };

                        await authorizer.CredentialStore.LoadAsync();

                        _twitterCtx = new TwitterContext(authorizer);
                    }

                    
                    var verifyResponse = await (from acct in _twitterCtx.Account
                            where acct.Type == AccountType.VerifyCredentials
                            select acct)
                           .SingleOrDefaultAsync();

                    LoggedInUser = verifyResponse.User;

                    if (_LoggedInUser != null) {
                        IsLoggedInVisibility = Visibility.Visible;
                        IsLoginVisible = Visibility.Collapsed;
                    }

                    await UpdateTweetsAsync();
                    
                }
            }
            else {
                //no passport so show login button
                IsLoginVisible = Visibility.Visible;
                IsAPIEditorVisible = Visibility.Collapsed;
            }
        }


        private async Task UpdateTweetsAsync() {

            if (_twitterCtx == null) return;

            Tweets = await (from tweet in _twitterCtx.Status
                 where tweet.Type == StatusType.Home
                 select tweet)
                .ToListAsync();
        }

        private async Task<string> SendDataAsync(String Url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                //var resp = await httpClient.GetAsync(new Uri(Url));
                //return await resp.Content.ReadAsStringAsync();
                
                return await httpClient.GetStringAsync(new Uri(Url));
            }
            catch //(Exception Err)
            {
                //rootPage.NotifyUser("Error getting data from server." + Err.Message, NotifyType.StatusMessage);
            }

            return null;
        }

        public async void AttemptTwitterLogin()
        {
            
            var authorizer = new UniversalAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = apiKey.APIKey,
                    ConsumerSecret = apiKey.APISecret
                },
                SupportsCompression = true,
                Callback = apiKey.APICallbackUrl
            };

            await authorizer.AuthorizeAsync();

            _twitterCtx = new TwitterContext(authorizer);

            var dm = new PassportDataModel();
            dm.Token = authorizer.CredentialStore.OAuthToken;
            dm.TokenSecret = authorizer.CredentialStore.OAuthTokenSecret;
            dm.Verifier = authorizer.Parameters["oauth_verifier"];
            dm.PassType = GroupingType;

            dm.UserId = authorizer.CredentialStore.UserID.ToString();
            dm.UserName = authorizer.CredentialStore.ScreenName;
            dm.FullName = authorizer.CredentialStore.ScreenName;
            dm.ScreenName = authorizer.CredentialStore.ScreenName;

            dm.APIKeyFKID = apiKey.Id;


            StorageService.Instance.Storage.Insert(dm);

            PopulatePassportData();

            
        }




        private void OutputToken(string TokenUri)
        {
            //FlickrReturnedToken.Text = TokenUri;
        }


        private void Unload() {

            _twitterCtx?.Dispose();
            _twitterCtx = null;
        }
    }
}
