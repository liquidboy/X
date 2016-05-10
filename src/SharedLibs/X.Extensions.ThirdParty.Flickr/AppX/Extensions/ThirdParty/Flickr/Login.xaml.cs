using FlickrNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.Services.Data;

namespace X.Extensions.ThirdParty.Flickr
{
    public sealed partial class Login : UserControl
    {
        public FlickrNet.Flickr _flickr = null;
        OAuthAccessToken AccessToken;
        OAuthRequestToken RequestToken;

        bool IsLoggedIn = false;
        Person LoggedInUser { get; set; }
        APIKeyDataModel apiKey;
        

        public Login()
        {
            this.InitializeComponent();

            _flickr = new FlickrNet.Flickr();
            CheckAlreadyExists();
        }


        private async void CheckAlreadyExists() {

           
            
            var data = StorageService.Instance.Storage.RetrieveList<PassportDataModel>();
            if (data != null && data.Count > 0) {
                var dm = data[0];

                var apis = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>();
                if (apis != null && apis.Count > 0) apiKey = apis.Where(x => x.Id == dm.APIKeyFKID).FirstOrDefault();
                
                RequestToken = new OAuthRequestToken() { Token = dm.Token, TokenSecret = dm.TokenSecret };
                AccessToken = new OAuthAccessToken() {
                    Username = dm.UserName,
                    FullName = dm.FullName,
                    ScreenName = dm.ScreenName,
                    Token = dm.Token,
                    TokenSecret = dm.TokenSecret,
                    UserId = dm.UserId,
                };
                IsLoggedIn = true;

                _flickr.ApiKey = apiKey.APIKey;
                _flickr.ApiSecret = apiKey.APISecret;
                var p = await _flickr.PeopleGetInfoAsync(AccessToken.UserId);
                if(!p.HasError) LoggedInUser = p.Result;

            }
        }

        private async void Launch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _flickr.ApiKey = FlickrClientID.Text;
                _flickr.ApiSecret = FlickrClientSecret.Text;
                apiKey = new APIKeyDataModel() { APIKey = FlickrClientID.Text, APISecret = FlickrClientSecret.Text, APICallbackUrl = FlickrCallbackUrl.Text, Type = tbAPIType.Text , APIName = tbAPIName.Text, DeveloperUri = tbDeveloperUri.Text };
                StorageService.Instance.Storage.Insert(apiKey);


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

                IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(FlickrClientSecret.Text + "&", BinaryStringEncoding.Utf8);
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
                        System.Uri EndUri = new Uri(apiKey.APICallbackUrl.Contains("http")? apiKey.APICallbackUrl : $"http://{apiKey.APICallbackUrl}");

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
                                dm.PassType = "Flickr";

                                dm.UserId = AccessToken.UserId;
                                dm.UserName = AccessToken.Username;
                                dm.FullName = AccessToken.FullName;
                                dm.ScreenName = AccessToken.ScreenName;

                                dm.APIKeyFKID = apiKey.Id;


                                StorageService.Instance.Storage.Insert(dm);
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
                //rootPage.NotifyUser(Error.Message, NotifyType.ErrorMessage);
            }
        }

        private void OutputToken(string TokenUri)
        {
            FlickrReturnedToken.Text = TokenUri;
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

    }
}
