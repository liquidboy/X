using FlickrNet;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI.Core;
using X.CoreLib.Shared.Services;
using X.NeonShell.Services;

namespace X.NeonShell.ViewModels
{

    public partial class FlickrViewModel : DefaultViewModel, IDisposable
    {

        public event EventHandler ChangeState;

        string frob = string.Empty;
        OAuthRequestToken _rtFlickr;
        OAuthRequestToken _rtTwitter;

        //Auth flickr_Auth;


        public string FavouritePhotosUserId { get; set; }
        public string FavouritePhotosUserName { get; set; }
        public PhotoCollection FavouritePhotos { get; set; }

        public string FlickrPersonPhotosUserId { get; set; }
        public string FlickrPersonPhotosUserName { get; set; }
        public PhotoCollection FlickrPersonPhotos { get; set; }

        public string FlickrPhotoStreamPhotosUserId { get; set; }
        public string FlickrPhotoStreamPhotosUserName { get; set; }
        public PhotoCollection FlickrPhotoStreamPhotos { get; set; }

        public string SearchPhotosText { get; set; }
        public PhotoCollection SearchedPhotos { get; set; }

        public string GroupsPhotosText { get; set; }
        public GroupSearchResultCollection GroupsPhotos { get; set; }

        public PhotoCommentCollection SelectedPhotoComments { get; set; }
        public PhotoNoteCollection SelectedPhotoNotes { get; set; }

        public Photo SelectedPhoto { get; set; }
        public PhotoInfo SelectedPhotoInfo { get; set; }
        public ExifTagCollection SelectedExifInfo { get; set; }

        public ObservableCollection<Favourite> PublicFavourites { get; set; }

        public ObservableCollection<Promote> PublicPromoted { get; set; }

        Windows.UI.Core.CoreDispatcher _dispatcher;
        public string AuthorizationUrl { get; set; }
        public string TwitterAuthorizationUrl { get; set; }


        public void FreeUpMemory()
        {

            if (FavouritePhotos != null) FavouritePhotos.Clear();
            if (FlickrPersonPhotos != null) FlickrPersonPhotos.Clear();
            if (FlickrPhotoStreamPhotos != null) FlickrPhotoStreamPhotos.Clear();
            if (SearchedPhotos != null) SearchedPhotos.Clear();
            if (GroupsPhotos != null) GroupsPhotos.Clear();


            if (SelectedPhotoComments != null) SelectedPhotoComments.Clear();
            if (SelectedPhotoNotes != null) SelectedPhotoNotes.Clear();

            SelectedPhoto = null;
            SelectedPhotoInfo = null;
            SelectedExifInfo = null;

            if (PublicFavourites != null) PublicFavourites.Clear();
            if (PublicPromoted != null) PublicPromoted.Clear();

        }

        public OAuthAccessToken AccessToken
        {
            get { return _atFlickr; }
            private set { }
        }



        public FlickrViewModel(Windows.UI.Core.CoreDispatcher dispatcher)
        {
            //refresh based on new appstates added
            AppDatabase.Current.LoadInstances();

            _dispatcher = dispatcher;

        }





        public async void RequestLogout()
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                AppDatabase.Current.DeleteAppStates();
                AppDatabase.Current.LoadInstances();
                _atFlickr = null;
                //_atTwitter = null;

                if (ChangeState != null) ChangeState("LogoutComplete", EventArgs.Empty);
            });


        }
        public async void RequestTwitterLogout()
        {
            //throw new NotImplementedException("RequestTwitterLogout");
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {

                _atTwitter = null;

                AppDatabase.Current.DeleteAppState("atTwitter.FullName");
                AppDatabase.Current.DeleteAppState("atTwitter.ScreenName");
                AppDatabase.Current.DeleteAppState("atTwitter.UserId");
                AppDatabase.Current.DeleteAppState("atTwitter.Username");
                AppDatabase.Current.DeleteAppState("atTwitter.Token");
                AppDatabase.Current.DeleteAppState("atTwitter.TokenSecret");

                //buddy icon is populated when we retrieve loggedin users details ( GetLoggedInUserDetails )
                AppDatabase.Current.DeleteAppState("tp.BuddyIconUrl");

                if (ChangeState != null) ChangeState("LogoutComplete", EventArgs.Empty);
            });


        }

        public async void RequestAuthorization()
        {

            //1. GET THE OAUTH REQUEST TOKEN
            //2. CONSTRUCT A URL & LAUNCH IT TO GET AN "AUTHORIZATION" TOKEN
            var x = await _flickr.OAuthGetRequestTokenAsync("oob");

            if (!x.HasError)
            {
                _rtFlickr = x.Result;
                string url = _flickr.OAuthCalculateAuthorizationUrl(_rtFlickr.Token, FlickrNet.AuthLevel.Write);
                try
                {
                    await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        ////await Windows.System.Launcher.LaunchUriAsync(new Uri(url), new Windows.System.LauncherOptions() { DisplayApplicationPicker = true });
                        //grdWebView.Visibility = Visibility.Visible;
                        //tbConfirmationCode.Visibility = Visibility.Visible;
                        //wvLoginRequest.Source = new Uri(url);

                    });


                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.High,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            AuthorizationUrl = url;
                            if (ChangeState != null) ChangeState("RequestGiven", EventArgs.Empty);

                        })
                    );
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            };


        }

        public async void RequestFlickrAuthorization()
        {
            try
            {
                //
                // Acquiring a request token
                //
                TimeSpan SinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
                Random Rand = new Random();
                String FlickrUrl = "https://secure.flickr.com/services/oauth/request_token";
                FlickrUrl = "https://www.flickr.com/services/oauth/request_token";
                Int32 Nonce = Rand.Next(1000000000);
                //
                // Compute base signature string and sign it.
                //    This is a common operation that is required for all requests even after the token is obtained.
                //    Parameters need to be sorted in alphabetical order
                //    Keys and values should be URL Encoded.
                //
                String SigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(AppService.AppSetting.AMSUrl); //"http://FavouriteMx.azure.com"
                SigBaseStringParams += "&" + "oauth_consumer_key=" + AppService.AppSetting.FlickrKey; //"102e389a942747faebb958c4db95c098";
                SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                SigBaseStringParams += "&" + "oauth_version=1.0";
                String SigBaseString = "GET&";
                SigBaseString += Uri.EscapeDataString(FlickrUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);

                Windows.Storage.Streams.IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(AppService.AppSetting.FlickrSecret+ "&", BinaryStringEncoding.Utf8);
                MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
                CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
                Windows.Storage.Streams.IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(SigBaseString, BinaryStringEncoding.Utf8);
                Windows.Storage.Streams.IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
                String Signature = CryptographicBuffer.EncodeToBase64String(SignatureBuffer);

                FlickrUrl += "?" + SigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(Signature);
                string GetResponse = await SendDataAsync(FlickrUrl);



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

                        _rtFlickr = new OAuthRequestToken() { Token = oauth_token, TokenSecret = oauth_token_secret };


                        FlickrUrl = "https://secure.flickr.com/services/oauth/authorize?oauth_token=" + oauth_token + "&perms=write";
                        System.Uri StartUri = new Uri(FlickrUrl);
                        System.Uri EndUri = new Uri(AppService.AppSetting.AMSUrl); //"http://FavouriteMx.azure.com"


                        ////MOBILE < --legacy approach
                        //Windows.Foundation.Collections.ValueSet dataRet = new Windows.Foundation.Collections.ValueSet();
                        //dataRet.Add("Operation", "Dashboard");
                        //WebAuthenticationBroker.AuthenticateAndContinue(StartUri, EndUri, dataRet, WebAuthenticationOptions.None);




                        //Desktop 
                        WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                                WebAuthenticationOptions.None,
                                                                StartUri,
                                                                EndUri);

                        await RequestFlickrAuthorizationComplete(WebAuthenticationResult.ResponseStatus, WebAuthenticationResult.ResponseData, WebAuthenticationResult.ResponseErrorDetail);



                    }
                }
            }
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                _raiseError("error : massive flickr oauth failure - " + Error.Message);
                Services.NavigationService.Current.Navigate(eViews.FlickrLogin);
            }
        }

        public async void RequestTwitterAuthorization()
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
                String SigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString("https://flowmx.azurewebsites.net/");//AppService.AppSetting.AMSUrl); //"http://FavouriteMx.azure.com"
                SigBaseStringParams += "&" + "oauth_consumer_key=" + AppService.AppSetting.TwitterKey; //"102e389a942747faebb958c4db95c098";
                SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                SigBaseStringParams += "&" + "oauth_version=1.0";
                String SigBaseString = "GET&";
                SigBaseString += Uri.EscapeDataString(TwitterUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);

                Windows.Storage.Streams.IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(AppService.AppSetting.TwitterSecret + "&", BinaryStringEncoding.Utf8);
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

                        _rtTwitter = new OAuthRequestToken() { Token = oauth_token, TokenSecret = oauth_token_secret };


                        TwitterUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth_token + "&perms=write";
                        System.Uri StartUri = new Uri(TwitterUrl);
                        System.Uri EndUri = new Uri(" https://flowmx.azurewebsites.net/");//AppService.AppSetting.AMSUrl); //"http://FavouriteMx.azure.com"


                        //MOBILE < -- legacy approach
                        //Windows.Foundation.Collections.ValueSet dataRet = new Windows.Foundation.Collections.ValueSet();
                        //dataRet.Add("Operation", "Home");
                        //WebAuthenticationBroker.AuthenticateAndContinue(StartUri, EndUri, dataRet, WebAuthenticationOptions.None);



                        //Desktop 
                        WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                                WebAuthenticationOptions.None,
                                                                StartUri,
                                                                EndUri);
                        await RequestTwitterAuthorizationComplete(WebAuthenticationResult.ResponseStatus, WebAuthenticationResult.ResponseData, WebAuthenticationResult.ResponseErrorDetail);



                    }
                }
            }
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                _raiseError("error : massive twitter oauth failure - " + Error.Message);
                Services.NavigationService.Current.Navigate(eViews.FlickrLogin);
            }
        }

        public async Task<bool> RequestFlickrAuthorizationComplete(WebAuthenticationStatus ResponseStatus, string ResponseData, uint ResponseErrorDetail)
        {
            if (ResponseStatus == WebAuthenticationStatus.Success)
            {


                String oauth_verifier = null;
                String[] keyValPairs2 = ResponseData.Split('&');

                for (int i = 0; i < keyValPairs2.Length; i++)
                {
                    String[] splits2 = keyValPairs2[i].Split('=');
                    switch (splits2[0])
                    {
                        case "oauth_verifier":
                            oauth_verifier = splits2[1];
                            break;
                    }
                }

                if (oauth_verifier != null)
                {
                    var rat = await _flickr.OAuthGetAccessTokenAsync(_rtFlickr, oauth_verifier);
                    if (!rat.HasError)
                    {
                        _atFlickr = rat.Result;


                        await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                        {

                            AppDatabase.Current.AddAppState("atFlickr.FullName", _atFlickr.FullName);
                            AppDatabase.Current.AddAppState("atFlickr.ScreenName", _atFlickr.ScreenName);
                            AppDatabase.Current.AddAppState("atFlickr.UserId", _atFlickr.UserId);
                            AppDatabase.Current.AddAppState("atFlickr.Username", _atFlickr.Username);
                            AppDatabase.Current.AddAppState("atFlickr.Token", _atFlickr.Token);
                            AppDatabase.Current.AddAppState("atFlickr.TokenSecret", _atFlickr.TokenSecret);

                            //buddy icon is populated when we retrieve loggedin users details ( GetLoggedInUserDetails )
                            AppDatabase.Current.AddAppState("fp.BuddyIconUrl", "");

                            //_flickr.AuthToken = _atFlickr.Token;


                            //refresh based on new appstates added
                            AppDatabase.Current.LoadInstances();



                            if (ChangeState != null) ChangeState("ConfirmationComplete", EventArgs.Empty);

                        });



                    }

                }
                else
                {
                    //throw excption
                    _raiseError("error : there was no oauth verifier from flickr ???!!");
                    Services.NavigationService.Current.Navigate(eViews.FlickrLogin);
                }

            }
            else if (ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                //throw excpetion ("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                _raiseError("Error : " + ResponseErrorDetail.ToString());
                Services.NavigationService.Current.Navigate(eViews.FlickrLogin);
            }
            else
            {
                _raiseError(ResponseStatus.ToString());
                Services.NavigationService.Current.Navigate(eViews.FlickrLogin);
                // throw excpetion ("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
            }

            return true;
        }

        public async Task<bool> RequestTwitterAuthorizationComplete(WebAuthenticationStatus ResponseStatus, string ResponseData, uint ResponseErrorDetail)
        {
            if (ResponseStatus == WebAuthenticationStatus.Success)
            {


                String oauth_verifier = null;
                String[] keyValPairs2 = ResponseData.Split('&');

                for (int i = 0; i < keyValPairs2.Length; i++)
                {
                    String[] splits2 = keyValPairs2[i].Split('=');
                    switch (splits2[0])
                    {
                        case "oauth_verifier":
                            oauth_verifier = splits2[1];
                            break;
                    }
                }

                if (oauth_verifier != null)
                {
                    var rat = await _twitter.OAuthGetAccessTokenAsync(_rtTwitter, oauth_verifier);

                    if (!rat.HasError)
                    {
                        _atTwitter = rat.Result;


                        await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                        {

                            AppDatabase.Current.AddAppState("atTwitter.FullName", _atTwitter.FullName);
                            AppDatabase.Current.AddAppState("atTwitter.ScreenName", _atTwitter.ScreenName);
                            AppDatabase.Current.AddAppState("atTwitter.UserId", _atTwitter.UserId);
                            AppDatabase.Current.AddAppState("atTwitter.Username", _atTwitter.Username);
                            AppDatabase.Current.AddAppState("atTwitter.Token", _atTwitter.Token);
                            AppDatabase.Current.AddAppState("atTwitter.TokenSecret", _atTwitter.TokenSecret);

                            //buddy icon is populated when we retrieve loggedin users details ( GetLoggedInUserDetails )
                            AppDatabase.Current.AddAppState("tp.BuddyIconUrl", "");

                            //_flickr.AuthToken = _at.Token;


                            //refresh based on new appstates added
                            AppDatabase.Current.LoadInstances();



                            if (ChangeState != null) ChangeState("TwitterConfirmationComplete", EventArgs.Empty);

                        });



                    }
                }
                else
                {
                    //throw excption
                    _raiseError("error : there was no oauth verifier from flickr ???!!");
                    Services.NavigationService.Current.Navigate(eViews.TwitterLogin);
                }

            }
            else if (ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                //throw excpetion ("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                _raiseError("Error : " + ResponseErrorDetail.ToString());
                Services.NavigationService.Current.Navigate(eViews.TwitterLogin);
            }
            else
            {
                _raiseError(ResponseStatus.ToString());
                Services.NavigationService.Current.Navigate(eViews.TwitterLogin);
                // throw excpetion ("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
            }

            return true;
        }


        private async Task<string> SendDataAsync(String Url)
        {
            try
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                return await httpClient.GetStringAsync(new Uri(Url));
            }
            catch (Exception Err)
            {

            }

            return null;
        }





        public async void AuthorizationGiven(string confirmationCode)
        {
            //3. COPY THE VERIFICATION CODE FROM THE FLICKR PAGE AND USE IT TO GET AN "ACCESS" TOKEN
            if (confirmationCode.Length == 0) return;

            var rat = await _flickr.OAuthGetAccessTokenAsync(_rtFlickr, confirmationCode); // new Action<FlickrResult<OAuthAccessToken>>(rat =>
                                                                                           //await _flickr.OAuthGetAccessTokenAsync(rt, tbConfirmationCode.Text, new Action<FlickrResult<OAuthAccessToken>>(rat =>


            if (!rat.HasError)
            {
                _atFlickr = rat.Result;


                await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {

                    AppDatabase.Current.AddAppState("atFlickr.FullName", _atFlickr.FullName);
                    AppDatabase.Current.AddAppState("atFlickr.ScreenName", _atFlickr.ScreenName);
                    AppDatabase.Current.AddAppState("atFlickr.UserId", _atFlickr.UserId);
                    AppDatabase.Current.AddAppState("atFlickr.Username", _atFlickr.Username);
                    AppDatabase.Current.AddAppState("atFlickr.Token", _atFlickr.Token);
                    AppDatabase.Current.AddAppState("atFlickr.TokenSecret", _atFlickr.TokenSecret);

                    //buddy icon is populated when we retrieve loggedin users details ( GetLoggedInUserDetails )
                    AppDatabase.Current.AddAppState("fp.BuddyIconUrl", "");

                    //_flickr.AuthToken = _atFlickr.Token;


                    //refresh based on new appstates added
                    AppDatabase.Current.LoadInstances();



                    if (ChangeState != null) ChangeState("ConfirmationComplete", EventArgs.Empty);

                });


                //if (ChangeState != null) ChangeState("ConfirmationComplete", EventArgs.Empty);






                ////USE YOUR ACCESS TO START MAKING API CALLS
                //GetLoggedInUserDetails(at.UserId);


            }


        }




        public async Task GetLoggedInUserDetails(string userid)
        {

            IncDecNetworkCallCount(1);

            var p = await _flickr.PeopleGetInfoAsync(userid).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            IncDecNetworkCallCount(-1);

            if (!p.HasError)
            {
                FlickrPerson = p.Result;
                if (ChangeState != null) ChangeState("UserInfoRetrieved", EventArgs.Empty);
            }
            else
            {
                _raiseError(p.ErrorMessage);
            }
        }



        public async Task GetLoggedInUserDetailsTight(string userid)
        {
            IncDecNetworkCallCount(1);

            var p = await _flickr.PeopleGetInfoAsync(userid).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);
            IncDecNetworkCallCount(-1);

            if (!p.HasError)
            {
                FlickrPerson = p.Result;
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        //imgUser.Source = new BitmapImage(new Uri(p.Result.BuddyIconUrl));
                        //brdAvatar.Opacity = 1;

                        //lblName.Text = p.Result.UserName;

                        //spLogin.Visibility = Visibility.Collapsed;
                        //spLoggedIn.Visibility = Visibility.Visible;

                        //lblProject.Visibility = Visibility.Visible;

                        if (ChangeState != null) ChangeState("UserInfoRetrieved", EventArgs.Empty);
                    })
                );
            }
            else
            {
                _raiseError(p.ErrorMessage);
            }


        }


        public async Task GetPhotoStream(string userid, string username, string usericonurl, int page = 1, int pageSize = 50, bool fromPaging = false)
        {
            IncDecNetworkCallCount(1);

            FlickrPhotoStreamPhotosUserId = userid;
            FlickrPhotoStreamPhotosUserName = username;
            var pc = await _flickr.PeopleGetPhotosAsync(
                userid,
                PhotoSearchExtras.OwnerName | PhotoSearchExtras.IconServer,
                page,
                pageSize).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            IncDecNetworkCallCount(-1);
            if (!pc.HasError)
            {
                var t = pc.Result;

                if (t.Count > 0)
                {
                    await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        if (FlickrPhotoStreamPhotos != null) FlickrPhotoStreamPhotos.Clear();
                        else FlickrPhotoStreamPhotos = new PhotoCollection();
                    }));



                    _addPreviousNextPhotos(page, ref t);

                    FlickrPhotoStreamPhotos = t;

                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("PhotoStreamPhotosRetrieved", new CustomPagingEventArgs() { PageNo = page + 1, UserId = userid, UserName = username, UserBuddyIconUrl = usericonurl, FromPagingRequest = fromPaging });
                        })
                    );
                }
                else
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("PhotoStreamPhotosRetrievedNone", EventArgs.Empty);
                        })
                    );
                }

            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }

        }

        public async Task UploadPictureAsync(Stream stream, string fileName, string title, string description, string tags, bool isPublic, bool isFamily, bool isFriend, ContentType contentType, SafetyLevel safetyLevel, HiddenFromSearch hiddenFromSearch, Action<FlickrResult<string>> callback)
        {
            await _flickr.UploadPictureAsync(stream, fileName, title, description, tags, isPublic,
                isFamily, isFriend, contentType, safetyLevel, hiddenFromSearch, callback);
            return;
        }

        public async Task GetLoggedInFavourites(string userid, string username, string usericonurl, int page = 1)
        {
            IncDecNetworkCallCount(1);
            if (page < 1) page = 1;

            FlickrPersonPhotosUserId = userid;
            FlickrPersonPhotosUserName = username;
            var pc = await _flickr.FavoritesGetListAsync(userid, DateTime.MinValue, DateTime.MinValue,
                PhotoSearchExtras.None, page, 0).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);


            IncDecNetworkCallCount(-1);
            if (!pc.HasError)
            {
                var t = pc.Result;

                if (t.Count > 0)
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (FlickrPersonPhotos != null) FlickrPersonPhotos.Clear();
                            else FlickrPersonPhotos = new PhotoCollection();
                        }));

                    //if (FlickrPersonPhotos != null) FlickrPersonPhotos.Clear();
                    //else FlickrPersonPhotos = new PhotoCollection();

                    _addPreviousNextPhotos(page, ref t);

                    //foreach (var item in t) FlickrPersonPhotos.Add(item);
                    FlickrPersonPhotos = t;
                    //pc.Result.Clear();

                    if (ChangeState != null) ChangeState("UserPublicPhotosRetrieved", new CustomPagingEventArgs() { PageNo = page + 1, UserId = userid, UserName = username, UserBuddyIconUrl = usericonurl });
                }
                else
                {
                    if (ChangeState != null && page == 1) ChangeState("UserPublicPhotosRetrievedNone", EventArgs.Empty);
                }



            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }

        }

        private void _addPreviousNextPhotos(int page, ref PhotoCollection listToUpate)
        {
            string next_img = "ms-appx:///Assets/Arrowhead-Right-01.png";
            string previous_img = "ms-appx:///Assets/Arrowhead-Left-01.png";

            var nextPageNo = page + 1;
            var previousPageNo = page - 1;


            if (page == 1)
            {
                var p = new Photo() { Title = "Next Page", PhotoId = nextPageNo.ToString(), UserId = "XXXXX", IsPaging = true };
                p.ForceUrlUpdate(next_img);
                listToUpate.Add(p);
            }
            else
            {
                var p1 = new Photo() { Title = "Previous Page", PhotoId = previousPageNo.ToString(), UserId = "XXXXX", IsPaging = true };
                p1.ForceUrlUpdate(previous_img);
                var p2 = new Photo() { Title = "Next Page", PhotoId = nextPageNo.ToString(), UserId = "XXXXX", IsPaging = true };
                p2.ForceUrlUpdate(next_img);

                listToUpate.Insert(0, p1);
                listToUpate.Add(p2);
            }
        }

        public async Task GetAuthorFavourites(string userid, string username, int page = 1)
        {
            IncDecNetworkCallCount(1);


            //_flickr.FavoritesGetListAsync(userid, async (pc) =>
            FlickrPersonPhotosUserId = userid;
            FlickrPersonPhotosUserName = username;
            var pc = await _flickr.FavoritesGetListAsync(userid, DateTime.MinValue, DateTime.MinValue, PhotoSearchExtras.None, page, 0);

            IncDecNetworkCallCount(-1);
            if (!pc.HasError)
            {
                var t = pc.Result;

                if (t.Count > 0)
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (FlickrPersonPhotos != null) FlickrPersonPhotos.Clear();
                            else FlickrPersonPhotos = new PhotoCollection();
                        }));

                    _addPreviousNextPhotos(page, ref t);

                    FlickrPersonPhotos = t;

                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            //lbPhotos.ItemsSource = PersonPhotos;

                            if (ChangeState != null) ChangeState("AuthorPublicPhotosRetrieved", new CustomPagingEventArgs() { PageNo = page + 1 });
                        })
                    );
                }
                else
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("AuthorPublicPhotosRetrievedNone", EventArgs.Empty);
                        })
                    );
                }


            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }

        }

        public async Task SearchFill(PhotoCollection col)
        {
            SearchedPhotos = col;

            if (_dispatcher != null)
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {

                        if (ChangeState != null) ChangeState("SearchedPhotosRetrieved", new CustomPagingEventArgs() { PageNo = 1 });
                    })
                );
            }
        }

        public void SearchCached(string cacheCallResponseUrl)
        {
            var cacheCall = AppDatabase.Current.RetrieveCacheCallResponse(cacheCallResponseUrl);

            this._flickr.GetCachedResponse<FlickrNet.PhotoCollection>(cacheCall[0].Data, (result) =>
            {

                if (result.Count > 0)
                {
                    _dispatcher.RunAsync(
                     Windows.UI.Core.CoreDispatcherPriority.Normal,
                     new Windows.UI.Core.DispatchedHandler(() =>
                     {
                         if (SearchedPhotos != null) SearchedPhotos.Clear();
                         else SearchedPhotos = new PhotoCollection();

                         SearchedPhotos = result;

                     }));



                    if (_dispatcher != null)
                    {
                        _dispatcher.RunAsync(
                            Windows.UI.Core.CoreDispatcherPriority.Normal,
                            new Windows.UI.Core.DispatchedHandler(() =>
                            {
                                if (ChangeState != null) ChangeState("SearchedPhotosRetrieved", new CustomPagingEventArgs() { PageNo = 0 });
                            })
                        );
                    }
                }
            });
        }

        public async Task Search(string searchFor, int page = 1, int pageSize = 50)
        {
            IncDecNetworkCallCount(1);

            var options = new PhotoSearchOptions { PerPage = pageSize, Page = page, Text = searchFor, Extras = PhotoSearchExtras.IconServer | PhotoSearchExtras.OwnerName };

            SearchPhotosText = searchFor;

            var result = await _flickr.PhotosSearchAsync(options);

            IncDecNetworkCallCount(-1);

            if (!result.HasError)
            {
                var t = result.Result;

                if (t.Count > 0)
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (SearchedPhotos != null) SearchedPhotos.Clear();
                            else SearchedPhotos = new PhotoCollection();
                        }));

                    _addPreviousNextPhotos(page, ref t);

                    SearchedPhotos = t;

                    //STORE LOCALLY SEARCH HISTORY
                    if (SearchedPhotos != null && SearchedPhotos.Count > 1)
                    {
                        var firstPhoto = SearchedPhotos[1];

                        AppDatabase.Current.SaveSearchRequest(new SearchRequest()
                        {
                            Term = searchFor,
                            TimeStamp = DateTime.UtcNow,
                            MediaUrlSmall = firstPhoto.SmallUrl,
                            MediaUrlMedium = firstPhoto.MediumUrl,
                            EntityId = firstPhoto.PhotoId,
                            MediaDescription = firstPhoto.Description,
                            MediaTitle = firstPhoto.Title,
                            MediaLicense = firstPhoto.License.ToString(),
                            MediaUserAvatar = string.Format("https://farm{0}.staticflickr.com/{1}/buddyicons/{2}.jpg", firstPhoto.IconFarm, firstPhoto.IconServer, firstPhoto.UserId),
                            MediaUserName = firstPhoto.OwnerName,
                            CacheCallResponseUrl = result.HashCall
                        });
                    }



                    if (_dispatcher != null)
                    {
                        await _dispatcher.RunAsync(
                            Windows.UI.Core.CoreDispatcherPriority.Normal,
                            new Windows.UI.Core.DispatchedHandler(() =>
                            {

                                if (ChangeState != null) ChangeState("SearchedPhotosRetrieved", new CustomPagingEventArgs() { PageNo = page + 1 });
                            })
                        );
                    }
                }
                else
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("SearchedPhotosRetrievedNone", EventArgs.Empty);
                        })
                    );
                }
            }
            else
            {
                _raiseError(result.ErrorMessage);
            }



        }

        public async Task GroupsFill(GroupSearchResultCollection col)
        {
            GroupsPhotos = col;

            if (_dispatcher != null)
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {

                        if (ChangeState != null) ChangeState("GroupsPhotosRetrieved", new CustomPagingEventArgs() { PageNo = 1 });
                    })
                );
            }
        }

        public void GroupsCached(string cacheCallResponseUrl)
        {
            var cacheCall = AppDatabase.Current.RetrieveCacheCallResponse(cacheCallResponseUrl);

            this._flickr.GetCachedResponse<FlickrNet.GroupSearchResultCollection>(cacheCall[0].Data, (result) =>
            {

                if (result.Count > 0)
                {
                    _dispatcher.RunAsync(
                     Windows.UI.Core.CoreDispatcherPriority.Normal,
                     new Windows.UI.Core.DispatchedHandler(() =>
                     {
                         if (GroupsPhotos != null) GroupsPhotos.Clear();
                         else GroupsPhotos = new GroupSearchResultCollection();

                         GroupsPhotos = result;

                     }));



                    if (_dispatcher != null)
                    {
                        _dispatcher.RunAsync(
                            Windows.UI.Core.CoreDispatcherPriority.Normal,
                            new Windows.UI.Core.DispatchedHandler(() =>
                            {
                                if (ChangeState != null) ChangeState("GroupsPhotosRetrieved", new CustomPagingEventArgs() { PageNo = 0 });
                            })
                        );
                    }
                }
            });
        }

        public async Task Groups(string searchFor, int page = 1, int pageSize = 50)
        {
            IncDecNetworkCallCount(1);

            //var options = new PhotoSearchOptions { PerPage = pageSize, Page = page, Text = searchFor, Extras = PhotoSearchExtras.IconServer | PhotoSearchExtras.OwnerName };

            GroupsPhotosText = searchFor;

            var result = await _flickr.GroupsSearchAsync(searchFor, page, pageSize);

            IncDecNetworkCallCount(-1);

            if (!result.HasError)
            {
                var t = result.Result;

                if (t.Count > 0)
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (GroupsPhotos != null) GroupsPhotos.Clear();
                            else GroupsPhotos = new GroupSearchResultCollection();
                        }));

                    //_addPreviousNextPhotos(page, ref t);

                    GroupsPhotos = t;

                    //STORE LOCALLY SEARCH HISTORY
                    if (GroupsPhotos != null && GroupsPhotos.Count > 1)
                    {
                        var firstPhoto = GroupsPhotos[1];

                        AppDatabase.Current.SaveGroupsRequest(new GroupsRequest()
                        {
                            Term = searchFor,
                            TimeStamp = DateTime.UtcNow,
                            MediaUrlSmall = string.Format("https://farm{0}.staticflickr.com/{1}/buddyicons/{2}.jpg?", firstPhoto.IconFarm, firstPhoto.IconServer, firstPhoto.GroupId),
                            MediaUrlMedium = string.Format("https://farm{0}.staticflickr.com/{1}/buddyicons/{2}.jpg?", firstPhoto.IconFarm, firstPhoto.IconServer, firstPhoto.GroupId),
                            EntityId = firstPhoto.GroupId,
                            //MediaDescription = firstPhoto.Description,
                            MediaTitle = firstPhoto.GroupName,
                            //MediaLicense = firstPhoto..ToString(),
                            //MediaUserAvatar = string.Format("https://farm{0}.staticflickr.com/{1}/buddyicons/{2}.jpg", firstPhoto.IconFarm, firstPhoto.IconServer, firstPhoto.UserId),
                            //MediaUserName = firstPhoto.OwnerName,
                            CacheCallResponseUrl = result.HashCall
                        });
                    }



                    if (_dispatcher != null)
                    {
                        await _dispatcher.RunAsync(
                            Windows.UI.Core.CoreDispatcherPriority.Normal,
                            new Windows.UI.Core.DispatchedHandler(() =>
                            {

                                if (ChangeState != null) ChangeState("GroupsPhotosRetrieved", new CustomPagingEventArgs() { PageNo = page + 1 });
                            })
                        );
                    }
                }
                else
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("GroupsPhotosRetrievedNone", EventArgs.Empty);
                        })
                    );
                }
            }
            else
            {
                _raiseError(result.ErrorMessage);
            }



        }


        //public void GetFavourites(string userid, string username, int page = 1)
        public async Task GetFavourites(string userid, string username, string usericonurl, int page = 1, int pageSize = 50)
        {
            IncDecNetworkCallCount(1);

            FavouritePhotosUserId = userid;
            FavouritePhotosUserName = username;
            var pc = await _flickr.FavoritesGetListAsync(userid, DateTime.MinValue, DateTime.MinValue, PhotoSearchExtras.IconServer | PhotoSearchExtras.OwnerName, page, pageSize).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            IncDecNetworkCallCount(-1);
            if (!pc.HasError)
            {

                var t = pc.Result;

                if (t.Count > 0)
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (FavouritePhotos != null) FavouritePhotos.Clear();
                            else FavouritePhotos = new PhotoCollection();
                        }));

                    _addPreviousNextPhotos(page, ref t);

                    FavouritePhotos = t;

                    if (_dispatcher != null)
                    {
                        await _dispatcher.RunAsync(
                            Windows.UI.Core.CoreDispatcherPriority.Normal,
                            new Windows.UI.Core.DispatchedHandler(() =>
                            {
                                //lbPhotos.ItemsSource = PersonPhotos;
                                if (ChangeState != null) ChangeState("FavouritePhotosRetrieved", new CustomPagingEventArgs() { PageNo = page + 1, UserId = userid, UserName = username, UserBuddyIconUrl = usericonurl });
                            })
                        );
                    }

                }
                else
                {
                    await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("FavouritePhotosRetrievedNone", EventArgs.Empty);
                        })
                    );
                }




            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }

        }



        public async Task GetPublicFavouritesAsync(DateTime timeStamp, int direction = 0, int size = 50)
        {
            await _doGetPublicFavouritesAsync(timeStamp, direction, size).AsAsyncAction().AsTask(MakeIdemniPotentAsyncCall().Token);
        }

        //direction greaterthan = 1, lessthan = -1, nothing = 0
        public async Task _doGetPublicFavouritesAsync(DateTime timestamp, int direction, int size)
        {
            IncDecNetworkCallCount(1);


            List<Favourite> result = null;
            if (direction == 0)
            {
                result = await AzureMobileService.Current.RetrieveFavoritesFromCloudAsync(size);
            }
            else
            {
                if (direction == 1) //next page
                {
                    result = await AzureMobileService.Current.RetrieveFavoritesLessThanDateFromCloudAsync(timestamp, size);
                }
                else if (direction == -1) //previous page
                {
                    result = await AzureMobileService.Current.RetrieveFavoritesGreaterThanDateFromCloudAsync(timestamp, size);
                }
                else
                {
                    result = await AzureMobileService.Current.RetrieveFavoritesFromCloudAsync(size);
                }
            }


            if (result.Count() > 0)
            {
                var firstdate = DateTime.MinValue;
                var lastdate = DateTime.MinValue;

                if (direction == 0)
                {
                    lastdate = result.Last().TimeStamp;
                }
                else if (direction == 1)
                {
                    firstdate = result.First().TimeStamp;
                    lastdate = result.Last().TimeStamp;
                }
                else if (direction == -1)
                {
                    firstdate = result.First().TimeStamp;
                    lastdate = result.Last().TimeStamp;
                }

                _addPreviousNextPhotos(firstdate, lastdate, ref result);

                if (PublicFavourites == null) PublicFavourites = new ObservableCollection<Favourite>();
                else PublicFavourites.Clear();

                foreach (var item in result) PublicFavourites.Add(item);

                // PublicFavourites = result;
                result.Clear();
                result = null;

                await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.High,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("PublicFavouritesRetrieved", new CustomPagingEventArgs() { });
                        })
                    );
            }
            else
            {

                //remove arrows when applicable
                if (PublicFavourites.Count() > 0)
                {
                    if (direction == 1)
                    {
                        if (PublicFavourites.Last().Id == "YYYYY") PublicFavourites.Remove(PublicFavourites.Last());
                    }
                    else if (direction == -1)
                    {
                        if (PublicFavourites.First().Id == "YYYYY") PublicFavourites.Remove(PublicFavourites.First());
                    }
                }



                await _dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.High,
                        new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            if (ChangeState != null) ChangeState("PublicFavouritesRetrievedNone", new CustomPagingEventArgs() { });
                        })
                    );
            }




            IncDecNetworkCallCount(-1);



        }

        private void _addPreviousNextPhotos(DateTime firstDate, DateTime lastDate, ref List<Favourite> listToUpate)
        {
            string next_img = "ms-appx:///Assets/Arrowhead-Right-01-sml.png";
            string previous_img = "ms-appx:///Assets/Arrowhead-Left-01-sml.png";

            if (firstDate != DateTime.MinValue)
            {
                var p1 = new Favourite() { MediaTitle = "Previous Page", TimeStamp = firstDate, Id = "YYYYY", MediaUrlSmall = previous_img, MediaUrlMedium = previous_img, IsPaging = true, Type = 1 };
                listToUpate.Insert(0, p1);
            }

            if (lastDate != DateTime.MinValue)
            {
                var p2 = new Favourite() { MediaTitle = "Next Page", TimeStamp = lastDate, Id = "YYYYY", MediaUrlSmall = next_img, MediaUrlMedium = next_img, IsPaging = true, Type = 1 };
                listToUpate.Add(p2);
            }

        }












        public async Task GetPublicPromotedAsync(DateTime timeStamp, int direction = 0, int size = 50)
        {
            await _doGetPublicPromotedAsync(timeStamp, direction, size).AsAsyncAction().AsTask(MakeIdemniPotentAsyncCall().Token);
        }



        //direction greaterthan = 1, lessthan = -1, nothing = 0
        public async Task _doGetPublicPromotedAsync(DateTime timestamp, int direction, int size)
        {
            IncDecNetworkCallCount(1);

            List<Promote> result = null;
            if (direction == 0)
            {
                result = await AzureMobileService.Current.RetrievePromotedFromCloudAsync(size).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);
            }
            else
            {
                if (direction == 1) //next page
                {
                    result = await AzureMobileService.Current.RetrievePromotedLessThanDateFromCloudAsync(timestamp, size).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);
                }
                else if (direction == -1) //previous page
                {
                    result = await AzureMobileService.Current.RetrievePromotedGreaterThanDateFromCloudAsync(timestamp, size).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);
                }
                else
                {
                    result = await AzureMobileService.Current.RetrievePromotedFromCloudAsync(size).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);
                }
            }


            if (result.Count() > 0)
            {

                var firstdate = DateTime.MinValue;
                var lastdate = DateTime.MinValue;

                if (direction == 0)
                {
                    lastdate = result.Last().TimeStamp;
                }
                else if (direction == 1)
                {
                    firstdate = result.First().TimeStamp;
                    lastdate = result.Last().TimeStamp;
                }
                else if (direction == -1)
                {
                    firstdate = result.First().TimeStamp;
                    lastdate = result.Last().TimeStamp;
                }


                _addPreviousNextPhotos(firstdate, lastdate, ref result);

                if (PublicPromoted == null) PublicPromoted = new ObservableCollection<Promote>();
                else PublicPromoted.Clear();

                foreach (var item in result) PublicPromoted.Add(item);

                //PublicPromoted = result;
                result.Clear();
                result = null;

                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        if (ChangeState != null) ChangeState("PublicPromotedRetrieved", EventArgs.Empty);
                    })
                );
            }
            else
            {
                //remove arrows when applicable
                if (PublicPromoted.Count() > 0)
                {
                    if (direction == 1)
                    {
                        if (PublicPromoted.Last().Id == "YYYYY") PublicPromoted.Remove(PublicPromoted.Last());
                    }
                    else if (direction == -1)
                    {
                        if (PublicPromoted.First().Id == "YYYYY") PublicPromoted.Remove(PublicPromoted.First());
                    }
                }


                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        if (ChangeState != null) ChangeState("PublicPromotedRetrievedNone", EventArgs.Empty);
                    })
                );
            }

            IncDecNetworkCallCount(-1);



        }

        private void _addPreviousNextPhotos(DateTime firstDate, DateTime lastDate, ref List<Promote> listToUpate)
        {
            string next_img = "ms-appx:///Assets/Arrowhead-Right-01-sml.png";
            string previous_img = "ms-appx:///Assets/Arrowhead-Left-01-sml.png";


            if (firstDate != DateTime.MinValue)
            {
                var p1 = new Promote() { MediaTitle = "Previous Page", TimeStamp = firstDate, Id = "YYYYY", MediaUrlSmall = previous_img, MediaUrlMedium = previous_img, IsPaging = true, Type = 2 };
                listToUpate.Insert(0, p1);
            }

            if (lastDate != DateTime.MinValue)
            {
                var p2 = new Promote() { MediaTitle = "Next Page", TimeStamp = lastDate, Id = "YYYYY", MediaUrlSmall = next_img, MediaUrlMedium = next_img, IsPaging = true, Type = 2 };
                listToUpate.Add(p2);
            }


        }















        bool _GetPhotoInfo_IsRunning = false;
        public async Task GetPhotoInfo(Photo photo)
        {
            if (_GetPhotoInfo_IsRunning) return;

            _GetPhotoInfo_IsRunning = true;
            IncDecNetworkCallCount(1);

            SelectedPhoto = photo;

            var pc = await _flickr.PhotosGetInfoAsync(photo.PhotoId).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            _GetPhotoInfo_IsRunning = false;
            IncDecNetworkCallCount(-1);

            if (!pc.HasError)
            {
                SelectedPhotoInfo = pc.Result;

                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        if (ChangeState != null) ChangeState("PhotoInfoRetrieved", EventArgs.Empty);
                    })
                );
            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }


        }

        bool _GetPhotoExif_IsRunning = false;
        public async Task GetPhotoExifAsync(Photo photo)
        {
            if (_GetPhotoExif_IsRunning) return;

            _GetPhotoExif_IsRunning = true;
            IncDecNetworkCallCount(1);

            SelectedExifInfo = null;
            RaisePropertyChanged("SelectedExifInfo");


            var pc = await _flickr.PhotosGetExifAsync(photo.PhotoId).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            _GetPhotoExif_IsRunning = false;
            IncDecNetworkCallCount(-1);

            if (!pc.HasError)
            {
                SelectedExifInfo = pc.Result;

                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        RaisePropertyChanged("SelectedExifInfo");
                        if (ChangeState != null) ChangeState("PhotoExifRetrieved", EventArgs.Empty);

                    })
                );
            }
            else
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        _raiseError(pc.ErrorMessage);
                    })
                );

            }

        }


        public string getLicenseTypeName(LicenseType licenseType)
        {
            switch (licenseType)
            {
                case LicenseType.AllRightsReserved: return "All Rights Reserved.";
                case LicenseType.AttributionCC: return "Creative Commons: Attribution License.";
                case LicenseType.AttributionNoDerivativesCC: return "Creative Commons: Attribution No Derivatives License.";
                case LicenseType.AttributionNoncommercialCC: return "Creative Commons: Attribution Non-Commercial License.";
                case LicenseType.AttributionNoncommercialNoDerivativesCC: return "Creative Commons: Attribution Non-Commercial, No Derivatives License.";
                case LicenseType.AttributionNoncommercialShareAlikeCC: return "Creative Commons: Attribution Non-Commercial, Share-alike License.";
                case LicenseType.AttributionShareAlikeCC: return "Creative Commons: Attribution Share-alike License.";
                case LicenseType.NoKnownCopyrightRestrictions: return "No Known Copyright Resitrctions (Flickr Commons).";
                case LicenseType.UnitedStatesGovernmentWork: return "United States Government Work";
            }

            return string.Empty;
        }

        public LicenseType getLicenseType(string licenseTypeName)
        {
            switch (licenseTypeName)
            {
                case "All Rights Reserved.": return LicenseType.AllRightsReserved;
                case "Creative Commons: Attribution License.": return LicenseType.AttributionCC;
                case "Creative Commons: Attribution No Derivatives License.": return LicenseType.AttributionNoDerivativesCC;
                case "Creative Commons: Attribution Non-Commercial License.": return LicenseType.AttributionNoncommercialCC;
                case "Creative Commons: Attribution Non-Commercial, No Derivatives License.": return LicenseType.AttributionNoncommercialNoDerivativesCC;
                case "Creative Commons: Attribution Non-Commercial, Share-alike License.": return LicenseType.AttributionNoncommercialShareAlikeCC;
                case "Creative Commons: Attribution Share-alike License.": return LicenseType.AttributionShareAlikeCC;
                case "No Known Copyright Resitrctions (Flickr Commons).": return LicenseType.NoKnownCopyrightRestrictions;
                case "United States Government Work": return LicenseType.UnitedStatesGovernmentWork;
            }

            return LicenseType.NoKnownCopyrightRestrictions;
        }


        public Favourite ConvertPhotoToFavourite(FlickrNet.Photo photo, PhotoInfo photoInfo, string userAvatarUri)
        {
            var newFav = new Favourite()
            {
                MediaLicense = getLicenseTypeName(photo.License),
                AggregateId = Guid.NewGuid().ToString(),
                MediaDescription = photo.Description == null ? string.Empty : photo.Description,
                MediaTitle = photo.Title == null ? string.Empty : photo.Title,
                MediaUrlSmall = photo.SmallUrl == null ? string.Empty : photo.SmallUrl,
                MediaUrlMedium = photo.MediumUrl == null ? string.Empty : photo.MediumUrl,
                UserAvatar = userAvatarUri,
                UserName = FlickrPerson == null ? _atFlickr.Username : FlickrPerson.UserName,
                UserRealName = FlickrPerson == null ? _atFlickr.FullName : FlickrPerson.RealName,
                TimeStamp = DateTime.Now.ToUniversalTime(),
                EntityId = photo.PhotoId
            };

            if (photoInfo != null)
            {
                newFav.MediaUserAvatar = photoInfo.OwnerBuddyIcon;
                newFav.MediaUserName = photoInfo.OwnerUserName;
            }

            return newFav;
        }

        public FlickrNet.Photo ConvertFavouriteToPhoto(Favourite fav)
        {
            var newPhoto = new FlickrNet.Photo()
            {
                License = getLicenseType(fav.MediaLicense),
                //Description = fav.MediaDescription,
                Title = fav.MediaTitle,
                //OwnerBuddyIcon = fav.MediaUserAvatar,
                //OwnerUserName = fav.MediaUserName,
                //userAvatarUri = fav.UserAvatar,
                //UserName = fav.UserName ,
                //RealName = fav.UserRealName ,
                PhotoId = fav.EntityId
            };

            newPhoto.ForceUrlUpdate(fav.MediaUrlSmall);
            newPhoto.MediumUrl = fav.MediaUrlMedium;
            newPhoto.LargeUrl = fav.MediaUrlMedium;

            return newPhoto;
        }


        public async Task UnfavouritePhotoAsync(Photo photo, PhotoInfo photoInfo, string userAvatarUri)
        {
            //if (ChangeState != null) ChangeState("PhotoFavourited", new CustomEventArgs() { Photo = photo });
            //return;
            IncDecNetworkCallCount(1);


            var nr = await _flickr.FavoritesRemoveAsync(photo.PhotoId).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            IncDecNetworkCallCount(-1);

            //UPDATE UI THAT FAVOURITE HAS BEEN REMOVED
            if (!nr.HasError)
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        try
                        {
                            var foundToRemove = FlickrPersonPhotos.Where(x => x.PhotoId == photo.PhotoId).First();
                            FlickrPersonPhotos.Remove(foundToRemove);

                        }
                        catch (Exception ex)
                        {

                        }

                        if (ChangeState != null) ChangeState("PhotoUnfavourited", new CustomEventArgs() { Photo = photo });
                    })
                );
            }
            else
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        _raiseError(nr.ErrorMessage);
                    })
                );

            }

        }


        public async Task FavouritePhotoAsync(Photo photo, PhotoInfo photoInfo, string userAvatarUri)
        {
            //if (ChangeState != null) ChangeState("PhotoFavourited", new CustomEventArgs() { Photo = photo });
            //return;
            IncDecNetworkCallCount(1);

            var nr = await _flickr.FavoritesAddAsync(photo.PhotoId).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);


            IncDecNetworkCallCount(-1);

            if (!string.IsNullOrEmpty(userAvatarUri))
            {
                //ADD TO PUBLIC AZURE FAVOURTES
                AzureMobileService.Current.SaveFavouriteToCloud(ConvertPhotoToFavourite(photo, photoInfo, userAvatarUri));
            }

            //UPDATE UI THAT FAVOURITE HAS BEEN ADDED
            if (!nr.HasError)
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        if (ChangeState != null) ChangeState("PhotoFavourited", new CustomEventArgs() { Photo = photo });
                    })
                );
            }
            else
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        _raiseError(nr.ErrorMessage);
                    })
                );

            }

        }

        public async Task CommentOnPhotoAsync(Photo photo, PhotoInfo photoInfo, string userAvatarUri, string say)
        {
            IncDecNetworkCallCount(1);

            //ADD TO PUBLIC AZURE PROMOTIONS
            AzureMobileService.Current.SaveCommentToCloud(new Comment()
            {
                AggregateId = Guid.NewGuid().ToString(),

                MediaUrlSmall = photo.SmallUrl == null ? string.Empty : photo.SmallUrl,
                MediaUrlMedium = photo.MediumUrl == null ? string.Empty : photo.MediumUrl,
                MediaUserAvatar = photoInfo.OwnerBuddyIcon,
                MediaUserName = photoInfo.OwnerUserName,
                UserAvatar = userAvatarUri,
                UserName = FlickrPerson == null ? _atFlickr.Username : FlickrPerson.UserName,
                UserRealName = FlickrPerson == null ? _atFlickr.FullName : FlickrPerson.RealName,
                TimeStamp = DateTime.Now.ToUniversalTime(),
                EntityId = photo.PhotoId,
                Text1 = say
            });


            //Send comment to flickr
            var nr = await _flickr.PhotosCommentsAddCommentAsync(photo.PhotoId, say).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);


            IncDecNetworkCallCount(-1);

            //UPDATE UI THAT FAVOURITE HAS BEEN ADDED
            if (!nr.HasError)
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        if (ChangeState != null) ChangeState("CommentAdded", new CustomEventArgs() { Photo = photo });
                    })
                );
            }
            else
            {
                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        _raiseError(nr.ErrorMessage);
                    })
                );

            }

        }

        public async Task DeleteFavouritePhotoAsync(string id)
        {
            AzureMobileService.Current.DeleteFavouriteFromCloud(new Favourite() { Id = id });
        }
        public async Task PromotePhotoAsync(Photo photo, PhotoInfo photoInfo, string userAvatarUri, string say)
        {

            IncDecNetworkCallCount(1);

            //ADD TO PUBLIC AZURE PROMOTIONS
            AzureMobileService.Current.SavePromoteToCloud(new Promote()
            {
                MediaLicense = getLicenseTypeName(photo.License),
                AggregateId = Guid.NewGuid().ToString(),
                MediaDescription = photo.Description == null ? string.Empty : photo.Description,
                MediaTitle = photo.Title == null ? string.Empty : photo.Title,
                MediaUrlSmall = photo.SmallUrl == null ? string.Empty : photo.SmallUrl,
                MediaUrlMedium = photo.MediumUrl == null ? string.Empty : photo.MediumUrl,
                MediaUserAvatar = photoInfo.OwnerBuddyIcon,
                MediaUserName = photoInfo.OwnerUserName,
                UserAvatar = userAvatarUri,
                UserName = _atFlickr.Username,
                UserRealName = _atFlickr.FullName,
                TimeStamp = DateTime.Now.ToUniversalTime(),
                EntityId = photo.PhotoId,
                ExtraText1 = say
            });

            //UPDATE UI THAT FAVOURITE HAS BEEN ADDED
            await _dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.High,
                new Windows.UI.Core.DispatchedHandler(() =>
                {
                    if (ChangeState != null) ChangeState("PhotoPromoted", new CustomEventArgs() { Photo = photo });
                })
            );

            IncDecNetworkCallCount(-1);

        }

        public async Task GetPhotoCommentsAsync(Photo photo)
        {
            IncDecNetworkCallCount(1);

            if (SelectedPhotoComments != null) SelectedPhotoComments.Clear();
            else SelectedPhotoComments = new PhotoCommentCollection();

            var pc = await _flickr.PhotosCommentsGetListAsync(photo.PhotoId).AsAsyncOperation().AsTask(MakeIdemniPotentAsyncCall().Token);

            IncDecNetworkCallCount(-1);
            if (!pc.HasError)
            {
                SelectedPhotoComments = pc.Result;


                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        this.RaisePropertyChanged("SelectedPhotoComments");
                        if (ChangeState != null) ChangeState("PhotoCommentsRetrieved", EventArgs.Empty);
                    })
                );


            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }

        }

        public async Task GetPhotoNotesAsync(Photo photo)
        {
            IncDecNetworkCallCount(1);

            if (SelectedPhotoNotes != null) SelectedPhotoNotes.Clear();
            else SelectedPhotoNotes = new PhotoNoteCollection();

            var pc = await _flickr.PhotoNotesGetListAsync(photo.PhotoId);

            IncDecNetworkCallCount(-1);
            if (!pc.HasError)
            {
                SelectedPhotoNotes = pc.Result;


                await _dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.High,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        this.RaisePropertyChanged("SelectedPhotoNotes");
                        if (ChangeState != null) ChangeState("PhotoNotesRetrieved", EventArgs.Empty);
                    })
                );


            }
            else
            {
                _raiseError(pc.ErrorMessage);
            }

        }



        private async void _raiseError(string message)
        {
            if (_dispatcher != null)
            {
                await _dispatcher.RunAsync(
                            Windows.UI.Core.CoreDispatcherPriority.High,
                            new Windows.UI.Core.DispatchedHandler(() =>
                            {
                                if (string.IsNullOrEmpty(message))
                                {
                                    message = "Wow something unexpected occured with Flickr's API :(";
                                }

                                AppService.SendInformationNotification(message, 2);
                            })
                        );
            }
        }

        private async void IncDecNetworkCallCount(int value)
        {
            await _dispatcher?.RunAsync(
                CoreDispatcherPriority.High,
                new DispatchedHandler(() =>
                {
                    //UI.NetworkCallCount = UI.NetworkCallCount + value;
                })
            );

        }

        public async void IncDecDownloadCount(int value)
        {
            await _dispatcher?.RunAsync(
                CoreDispatcherPriority.High,
                new DispatchedHandler(() =>
                {
                    //UI.DownloadCount = UI.DownloadCount + value;
                })
            );
        }

        public void Unload()
        {
            if (FlickrPersonPhotos != null)
            {
                FlickrPersonPhotos.Clear();
                FlickrPersonPhotos = null;
            }

            if (FavouritePhotos != null)
            {
                FavouritePhotos.Clear();
                FavouritePhotos = null;
            }

            if (FlickrPhotoStreamPhotos != null)
            {
                FlickrPhotoStreamPhotos.Clear();
                FlickrPhotoStreamPhotos = null;
            }

            if (SelectedPhotoComments != null)
            {
                SelectedPhotoComments.Clear();
                SelectedPhotoComments = null;
            }

            SelectedPhoto = null;
            SelectedPhotoInfo = null;
            SelectedExifInfo = null;

            if (PublicFavourites != null)
            {
                PublicFavourites.Clear();
                PublicFavourites = null;
            }

            if (PublicPromoted != null)
            {
                PublicPromoted.Clear();
                PublicPromoted = null;
            }

            _dispatcher = null;

            _flickr = null;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }

    public class CustomEventArgs : EventArgs
    {
        public Photo Photo;
    }

    public class CustomPagingEventArgs : EventArgs
    {
        public int PageNo;
        public string UserId;
        public string UserName;
        public string UserBuddyIconUrl;
        public bool FromPagingRequest;
    }
}
