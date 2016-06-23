using FlickrNet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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



        public Visibility IsLoginVisible { get { return _IsLoginVisible; } set { _IsLoginVisible = value; RaisePropertyChanged(); } } 
        public Person LoggedInUser { get { return _LoggedInUser; } set { _LoggedInUser = value; RaisePropertyChanged(); } }
       
        public List<Tab> Tabs { get; set; } = new List<Tab>();



        




        public SplashVM() {
            IsLoginVisible = Visibility.Collapsed;
            _flickr = new FlickrNet.Flickr();
            Tabs.Add(new Tab() { Name = "Your Favourites", IsSelected = true});
            Tabs.Add(new Tab() { Name = "Public" });
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
             
            }
        }

        private async Task<string> SendDataAsync(String Url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
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
            try
            {
                //
                // Acquiring a request token
                //
                TimeSpan SinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
                Random Rand = new Random();
                String TwitterUrl = "https://api.twitter.com/oauth/request_token/";
                Int32 Nonce = Rand.Next(1000000000);
                //
                // Compute base signature string and sign it.
                //    This is a common operation that is required for all requests even after the token is obtained.
                //    Parameters need to be sorted in alphabetical order
                //    Keys and values should be URL Encoded.
                //
                String SigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(apiKey.APICallbackUrl); 
                SigBaseStringParams += "&" + "oauth_consumer_key=" + apiKey.APIKey; 
                SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                SigBaseStringParams += "&" + "oauth_version=1.0";
                String SigBaseString = "GET&";
                SigBaseString += Uri.EscapeDataString(TwitterUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);

                Windows.Storage.Streams.IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(apiKey.APISecret + "&", BinaryStringEncoding.Utf8);
                MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
                CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
                Windows.Storage.Streams.IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(SigBaseString, BinaryStringEncoding.Utf8);
                Windows.Storage.Streams.IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
                String Signature = CryptographicBuffer.EncodeToBase64String(SignatureBuffer);

                TwitterUrl += "?" + SigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(Signature);
                string GetResponse = await SendDataAsync(TwitterUrl);



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

                    if (oauth_token != null)
                    {

                        RequestToken = new OAuthRequestToken() { Token = oauth_token, TokenSecret = oauth_token_secret };


                        TwitterUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth_token + "&perms=write";
                        System.Uri StartUri = new Uri(TwitterUrl);
                        System.Uri EndUri = new Uri(apiKey.APICallbackUrl.Contains("http") ? apiKey.APICallbackUrl : $"http://{apiKey.APICallbackUrl}");





                        //Desktop 
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
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //

            }
        }




        private void OutputToken(string TokenUri)
        {
            //FlickrReturnedToken.Text = TokenUri;
        }
    }
}
