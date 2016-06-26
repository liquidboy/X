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

namespace X.Extension.ThirdParty.Facebook.VM
{
    public class SplashVM : ViewModelBase
    {
        string _appId = "2356103186";
        string _appIdSecret = "366a1024a3327074d46153bacb818697";
        private string _accessToken = "";
        private DateTime _tokenExpiry;

        private RelayCommand<string> _requestLogin;

        public RelayCommand<string> RequestLogin { get { return _requestLogin ?? (_requestLogin = new RelayCommand<string>((arg) => { AttemptLogin(); })); } }

        FacebookClient _client;
        //https://blogs.msdn.microsoft.com/wsdevsol/2015/02/12/integrating-facebook-authentication-in-universal-windows-apps/



        private async void AttemptLogin() {
            if (_client != null) return;
            _client = new FacebookClient();
            _client.AppId = _appId;
            _client.AppSecret = _appIdSecret;

            var scope = "public_profile, email";

            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var fb = new FacebookClient();
            Uri loginUrl = fb.GetLoginUrl(new
            {
                client_id = _appId,
                redirect_uri = redirectUri,
                response_type = "token",
                scope = scope
            });

            Uri startUri = loginUrl;
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            await ParseAuthenticationResult(result);


        }

        private async void MakeCall() {
            _client = new FacebookClient(_accessToken);
            dynamic parameters = new ExpandoObject();
            parameters.message = "FacebookString";
            dynamic result = await _client.PostTaskAsync("me/feed", parameters);
            var id = result.id;


 
        }

        public async Task ParseAuthenticationResult(WebAuthenticationResult result)
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
