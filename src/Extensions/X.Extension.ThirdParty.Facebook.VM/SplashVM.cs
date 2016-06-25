using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace X.Extension.ThirdParty.Facebook.VM
{
    public class SplashVM : ViewModelBase
    {
        private string _appId = "";
        private string _appIdSecret = "";
        private string _accessToken = "";

        private RelayCommand<string> _requestLogin;

        public RelayCommand<string> RequestLogin { get { return _requestLogin ?? (_requestLogin = new RelayCommand<string>((arg) => { AttemptLogin(); })); } }

        FacebookClient _client;

        private void AttemptLogin() {
            if (_client != null) return;
            _client.AppId = _appId;
            _client.AppSecret = _appIdSecret;

            _client.ParseOAuthCallbackUrl(_client.GetLoginUrl(""));
        }

        private async void MakeCall() {
            _client = new FacebookClient(_accessToken);
            dynamic parameters = new ExpandoObject();
            parameters.message = "FacebookString";
            dynamic result = await _client.PostTaskAsync("me/feed", parameters);
            var id = result.id;
        }


    }
}
