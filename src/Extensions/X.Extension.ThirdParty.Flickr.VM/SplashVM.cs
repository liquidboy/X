using FlickrNet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
using X.CoreLib.GenericMessages;
using X.Services.Data;
using X.UI.LiteTab;

namespace X.Extension.ThirdParty.Flickr.VM
{
    public class SplashVM :ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public string GroupingType { get; set; } = "Flickr";
        public string HostPackageFamilyName { get; set; } = "6d918283-1709-4827-80b0-bce015b85d11_tcz6rsq1yejk0";


        private PhotoCollection _FavouritePhotos;
        public PhotoCollection FavouritePhotos { get { return _FavouritePhotos; } set { _FavouritePhotos = value; RaisePropertyChanged(); } }


        public FlickrNet.Flickr _flickr = null;
        OAuthAccessToken AccessToken;
        OAuthRequestToken RequestToken;

        //bool IsLoggedIn = false;
        APIKeyDataModel apiKey;

        Person _LoggedInUser;


        Visibility _IsLoginVisible = Visibility.Collapsed;
        Visibility _IsAPIKeyVisible = Visibility.Collapsed;
        Visibility _IsTabsVisible = Visibility.Collapsed;
        Visibility _IsFavouritesVisible = Visibility.Visible;
        Visibility _IsPublicVisible = Visibility.Collapsed;


        public Visibility IsPublicVisible { get { return _IsPublicVisible; } set { _IsPublicVisible = value; RaisePropertyChanged(); } }
        public Visibility IsFavouritesVisible { get { return _IsFavouritesVisible; } set { _IsFavouritesVisible = value; RaisePropertyChanged(); } }
        public Visibility IsLoginVisible { get { return _IsLoginVisible; } set { _IsLoginVisible = value; RaisePropertyChanged(); } } 
        public Visibility IsAPIKeyVisible { get { return _IsAPIKeyVisible; } set { _IsAPIKeyVisible = value; RaisePropertyChanged(); } }
        public Person LoggedInUser { get { return _LoggedInUser; } set { _LoggedInUser = value; RaisePropertyChanged(); } }
        public Visibility IsTabsVisible { get { return _IsTabsVisible; } set { _IsTabsVisible = value; RaisePropertyChanged(); } }

        public List<Tab> Tabs { get; set; } = new List<Tab>();

        private RelayCommand<string> _requestFlickrLogin;
        private RelayCommand<Photo> _pictureSelectedCommand;
        private RelayCommand<Tab> _tabChangedCommand;

        
        public RelayCommand<Photo> PictureSelectedCommand
        {
            get
            {
                return _pictureSelectedCommand ?? (_pictureSelectedCommand = new RelayCommand<Photo>((arg) => { RetrievePhotoDetails(arg);  }));
            }
        }
        public RelayCommand<string> RequestFlickrLogin
        {
            get
            {
                return _requestFlickrLogin ?? (_requestFlickrLogin = new RelayCommand<string>((arg)=> { AttemptFlickrLogin(); }));
            }
        }
        public RelayCommand<Tab> TabChangedCommand
        {
            get
            {
                return _tabChangedCommand ?? (_tabChangedCommand = new RelayCommand<Tab>((arg) => {
                    switch (arg.Name) {
                        case "Public": IsPublicVisible = Visibility.Visible; IsFavouritesVisible = Visibility.Collapsed; break;
                        case "Your Favourites": IsFavouritesVisible = Visibility.Visible; IsPublicVisible = Visibility.Collapsed; break;
                        default:break;
                    }
                }));
            }
        }






        public SplashVM() {
            _flickr = new FlickrNet.Flickr();
            Tabs.Add(new Tab() { Name = "Your Favourites", IsSelected = true});
            Tabs.Add(new Tab() { Name = "Public" });
            GetAPIData();
            PopulatePassportData();
        }

        private void GetAPIData() {
            var apis = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>();
            if (apis != null && apis.Count > 0) apiKey = apis.Where(x => x.Type == GroupingType).FirstOrDefault();

            // StorageService.Instance.Storage.DeleteById<APIKeyDataModel>(apiKey.Id.ToString());

            if (apiKey == null) IsAPIKeyVisible = Visibility.Visible;
            else IsAPIKeyVisible = Visibility.Collapsed;
        }


        private async void PopulatePassportData()
        {
            if (apiKey == null) { IsLoginVisible = Visibility.Visible; return; }

            var data = StorageService.Instance.Storage.RetrieveList<PassportDataModel>();
            if (data != null && data.Count > 0)
            {
                IsLoginVisible = Visibility.Visible;
                IsTabsVisible = Visibility.Collapsed;

                var dm = data.Where(x => x.PassType == GroupingType).FirstOrDefault();
                if (dm != null) {
                    IsLoginVisible = Visibility.Collapsed;
                    IsTabsVisible = Visibility.Visible;
                    
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
                    //IsLoggedIn = true;

                    _flickr.OAuthAccessToken = AccessToken.Token;
                    _flickr.OAuthAccessTokenSecret = AccessToken.TokenSecret;

                    _flickr.ApiKey = apiKey.APIKey;
                    _flickr.ApiSecret = apiKey.APISecret;

                    var p = await _flickr.PeopleGetInfoAsync(AccessToken.UserId);
                    if (!p.HasError) LoggedInUser = p.Result;

                    var favs = await _flickr.FavoritesGetListAsync(AccessToken.UserId);
                    if (!favs.HasError) {
                        var temp = new PhotoCollection();
                        foreach (var fav in favs.Result)
                        {
                            //fav.MachineTags = "https://c1.staticflickr.com/1/523/buddyicons/118877287@N03_l.jpg?1437204284#118877287@N03";
                            fav.MachineTags = $"https://c1.staticflickr.com/{fav.IconFarm}/{fav.IconServer}/buddyicons/{fav.UserId}.jpg?";
                            temp.Add(fav);
                        }
                        FavouritePhotos = temp;
                    }
                    
                }
            }
            else {
                //no passport so show login button
                IsLoginVisible = Visibility.Visible;
                IsTabsVisible = Visibility.Collapsed;
            }
        }

        private async Task<string> SendDataAsync(String Url)
        {
            try
            {
                // Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                HttpClient httpClient = new HttpClient();
                return await httpClient.GetStringAsync(new Uri(Url));
            }
            catch // (Exception Err)
            {
                //rootPage.NotifyUser("Error getting data from server." + Err.Message, NotifyType.StatusMessage);
            }

            return null;
        }

        private async void AttemptFlickrLogin()
        {
            //apiKey = ctlApiEditor.APIKey;

            try
            {
                _flickr.ApiKey = apiKey.APIKey;
                _flickr.ApiSecret = apiKey.APISecret;


                // Acquiring a request token
                TimeSpan SinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
                Random Rand = new Random();
                String FlickrUrl = "https://secure.flickr.com/services/oauth/request_token";
                Int32 Nonce = Rand.Next(1000000000);

                // Compute base signature string and sign it.
                // This is a common operation that is required for all requests even after the token is obtained.
                // Parameters need to be sorted in alphabetical order
                // Keys and values should be URL Encoded.
                String SigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(apiKey.APICallbackUrl);
                SigBaseStringParams += "&" + "oauth_consumer_key=" + apiKey.APIKey;
                SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                SigBaseStringParams += "&" + "oauth_version=1.0";
                String SigBaseString = "GET&";
                SigBaseString += Uri.EscapeDataString(FlickrUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);

                IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(apiKey.APISecret + "&", BinaryStringEncoding.Utf8);
                MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
                CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
                IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(SigBaseString, BinaryStringEncoding.Utf8);
                IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
                String Signature = CryptographicBuffer.EncodeToBase64String(SignatureBuffer);

                FlickrUrl += "?" + SigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(Signature);
                string GetResponse = await SendDataAsync(FlickrUrl);
                //rootPage.NotifyUser("Received Data: " + GetResponse, NotifyType.StatusMessage);


                if (GetResponse != null)
                {
                    String oauth_token = null;
                    String oauth_token_secret = null;
                    String[] keyValPairs = GetResponse.Split('&');

                    for (int i = 0; i < keyValPairs.Length; i++)
                    {
                        String[] splits = keyValPairs[i].Split('=');
                        switch (splits[0])
                        {
                            case "oauth_token":
                                oauth_token = splits[1];
                                break;
                            case "oauth_token_secret":
                                oauth_token_secret = splits[1];
                                break;
                        }
                    }

                    RequestToken = new OAuthRequestToken() { Token = oauth_token, TokenSecret = oauth_token_secret };

                    if (oauth_token != null)
                    {
                        FlickrUrl = "https://secure.flickr.com/services/oauth/authorize?oauth_token=" + oauth_token + "&perms=read";
                        System.Uri StartUri = new Uri(FlickrUrl);
                        System.Uri EndUri = new Uri(apiKey.APICallbackUrl.Contains("http") ? apiKey.APICallbackUrl : $"http://{apiKey.APICallbackUrl}");

                        //rootPage.NotifyUser("Navigating to: " + FlickrUrl, NotifyType.StatusMessage);
                        WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                                WebAuthenticationOptions.None,
                                                                StartUri,
                                                                EndUri);
                        if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                        {
                            OutputToken(WebAuthenticationResult.ResponseData.ToString());
                            String[] partsa = WebAuthenticationResult.ResponseData.ToString().Split('?');
                            String[] partsb = partsa[1].Split('&');

                            var xoauth_token = "";
                            var xoauth_verifier = "";

                            for (int i = 0; i < partsb.Length; i++)
                            {
                                String[] partsc = partsb[i].Split('=');
                                switch (partsc[0])
                                {
                                    case "oauth_token":
                                        xoauth_token = partsc[1];
                                        break;
                                    case "oauth_verifier":
                                        xoauth_verifier = partsc[1];
                                        break;
                                }
                            }


                            var rat = await _flickr.OAuthGetAccessTokenAsync(RequestToken, xoauth_verifier);
                            if (!rat.HasError)
                            {
                                AccessToken = rat.Result;

                                var dm = new PassportDataModel();
                                dm.Token = AccessToken.Token;
                                dm.TokenSecret = AccessToken.TokenSecret;
                                dm.Verifier = xoauth_verifier;
                                dm.PassType = GroupingType;

                                dm.UserId = AccessToken.UserId;
                                dm.UserName = AccessToken.Username;
                                dm.FullName = AccessToken.FullName;
                                dm.ScreenName = AccessToken.ScreenName;

                                dm.APIKeyFKID = apiKey.Id;


                                StorageService.Instance.Storage.Insert(dm);

                                PopulatePassportData();
                            }

                        }
                        else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                        {
                            OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                        }
                        else
                        {
                            OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                        }
                    }
                }
            }
            catch //(Exception Error)
            {
                //rootPage.NotifyUser(Error.Message, NotifyType.ErrorMessage);
            }
        }

        private void OutputToken(string TokenUri)
        {
            //FlickrReturnedToken.Text = TokenUri;
        }

        private async void RetrievePhotoDetails(Photo photo) {

            //var dataToPass = new List<KeyValuePair<string, object>>();

            //dataToPass.Add(new KeyValuePair<string, object>("Title", photo.Title));
            //dataToPass.Add(new KeyValuePair<string, object>("ThumbnailUrl", photo.ThumbnailUrl));
            //dataToPass.Add(new KeyValuePair<string, object>("LargeUrl", photo.LargeUrl));
            //dataToPass.Add(new KeyValuePair<string, object>("Medium640Url", photo.Medium640Url));
            //dataToPass.Add(new KeyValuePair<string, object>("MediumUrl", photo.MediumUrl));
            //dataToPass.Add(new KeyValuePair<string, object>("SmallUrl", photo.SmallUrl));
            //dataToPass.Add(new KeyValuePair<string, object>("IconServer", photo.IconServer));
            //dataToPass.Add(new KeyValuePair<string, object>("Server", photo.Server));
            //dataToPass.Add(new KeyValuePair<string, object>("IconFarm", photo.IconFarm));
            //dataToPass.Add(new KeyValuePair<string, object>("Farm", photo.Farm));
            //dataToPass.Add(new KeyValuePair<string, object>("PhotoId", photo.PhotoId));

            //await MakeUWPCommandCall("LoadFlickrPhoto", "Call", dataToPass);


            //note : amazingly mvvmlight message bus works perfectly with extensions talking to the host
            Messenger.Default.Send(new LoadPhoto() { Photo = photo });
        }



        public async Task<Windows.Foundation.Collections.ValueSet> MakeUWPCommandCall(string commandCall, string serviceName, List<KeyValuePair<string, object>> dataToPass)
        {

            using (var connection = new Windows.ApplicationModel.AppService.AppServiceConnection())
            {
                connection.AppServiceName = serviceName;
                connection.PackageFamilyName = HostPackageFamilyName;
                var status = await connection.OpenAsync();
                if (status != Windows.ApplicationModel.AppService.AppServiceConnectionStatus.Success)
                {
                    throw new NotImplementedException("Failed app service connection");
                }
                else
                {
                    var request = new Windows.Foundation.Collections.ValueSet();
                    request.Add("Command", commandCall);

                    if (dataToPass != null && dataToPass.Count > 0)
                    {
                        foreach (var data in dataToPass)
                        {
                            //message.Add(new KeyValuePair<string, object>("AppExtensionDisplayName", AppExtension.AppInfo.DisplayInfo.DisplayName));
                            request.Add(new KeyValuePair<string, object>(data.Key, data.Value));
                        }
                    }

                    Windows.ApplicationModel.AppService.AppServiceResponse response = await connection.SendMessageAsync(request);
                    if (response.Status == Windows.ApplicationModel.AppService.AppServiceResponseStatus.Success)
                    {
                        var message = response.Message as Windows.Foundation.Collections.ValueSet;
                        if (message != null && message.Count > 0)
                        {
                            
                        }
                        return (message != null && message.Count > 0) ? message : null;
                    }
                }

            }
            return null;
        }
    }
}
