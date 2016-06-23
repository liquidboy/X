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

        private PhotoCollection _FavouritePhotos;
        public PhotoCollection FavouritePhotos { get { return _FavouritePhotos; } set { _FavouritePhotos = value; RaisePropertyChanged(); } }


        public FlickrNet.Flickr _flickr = null;
        OAuthAccessToken AccessToken;
        OAuthRequestToken RequestToken;

        bool IsLoggedIn = false;
        APIKeyDataModel apiKey;

        Person _LoggedInUser;
        Visibility _IsLoginVisible;
        Visibility _IsAPIEditorVisible;


        public Visibility IsAPIEditorVisible { get { return _IsAPIEditorVisible; } set { _IsAPIEditorVisible = value; RaisePropertyChanged(); } }
        public Visibility IsLoginVisible { get { return _IsLoginVisible; } set { _IsLoginVisible = value; RaisePropertyChanged(); } } 
        public Person LoggedInUser { get { return _LoggedInUser; } set { _LoggedInUser = value; RaisePropertyChanged(); } }



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
            _flickr = new FlickrNet.Flickr();
            GetAPIData();
            PopulatePassportData();
        }

        private void GetAPIData() {
            var apis = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>();
            if (apis != null && apis.Count > 0) apiKey = apis.Where(x => x.Type == GroupingType).FirstOrDefault();

           
        }


        private async void PopulatePassportData()
        {
            var data = StorageService.Instance.Storage.RetrieveList<PassportDataModel>();
            if (data != null && data.Count > 0)
            {
                IsLoginVisible = Visibility.Visible;
                IsAPIEditorVisible = Visibility.Collapsed;

                var dm = data.Where(x => x.PassType == GroupingType).FirstOrDefault();
                if (dm != null) {
                    RequestToken = new OAuthRequestToken() { Token = dm.Token, TokenSecret = dm.TokenSecret };
                    AccessToken = new OAuthAccessToken()
                    {
                        Username = dm.UserName,
                        FullName = dm.FullName,
                        ScreenName = dm.ScreenName,
                        Token = dm.Token,
                        TokenSecret = dm.TokenSecret,
                        UserId = dm.UserId,
                    };
                    IsLoggedIn = true;

                    _flickr.OAuthAccessToken = AccessToken.Token;
                    _flickr.OAuthAccessTokenSecret = AccessToken.TokenSecret;

                    _flickr.ApiKey = apiKey.APIKey;
                    _flickr.ApiSecret = apiKey.APISecret;

                    var p = await _flickr.PeopleGetInfoAsync(AccessToken.UserId);
                    if (!p.HasError) LoggedInUser = p.Result;

                    var favs = await _flickr.FavoritesGetListAsync(AccessToken.UserId);
                    if (!favs.HasError) FavouritePhotos = favs.Result;
                }
            }
            else {
                //no passport so show login button
                IsLoginVisible = Visibility.Collapsed;
                IsAPIEditorVisible = Visibility.Visible;
            }
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
            catch (Exception Err)
            {
                //rootPage.NotifyUser("Error getting data from server." + Err.Message, NotifyType.StatusMessage);
            }

            return null;
        }

        public async void AttemptTwitterLogin()
        {


            //var auth = new ApplicationOnlyAuthorizer
            //{
            //    CredentialStore = new InMemoryCredentialStore
            //    {
            //        ConsumerKey = apiKey.APIKey,
            //        ConsumerSecret = apiKey.APISecret
            //    }
            //};

            //await auth.AuthorizeAsync();


            //var twitterCtx = new TwitterContext(auth);

            //var srch =
            //    (from search in twitterCtx.Search
            //     where search.Type == SearchType.Search &&
            //           search.Query == "d3d12"
            //     select search)
            //    .SingleOrDefault();


            //twitterCtx.Dispose();




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
    }
}
