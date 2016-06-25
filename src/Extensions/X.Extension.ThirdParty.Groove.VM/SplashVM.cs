using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Microsoft.Xbox.Music.Platform.Client;

namespace X.Extension.ThirdParty.Groove.VM
{
    public class SplashVM : ViewModelBase
    {
        private string _appId = "";
        private string _appIdSecret = "";
        private string _accessToken = "";

        private RelayCommand<string> _requestLogin;

        IXboxMusicClient _client;

        public RelayCommand<string> RequestLogin { get { return _requestLogin ?? (_requestLogin = new RelayCommand<string>((arg) => { AttemptLogin(); })); } }

        

        private void AttemptLogin() {
            if (_client != null) return;
            _client = XboxMusicClientFactory.CreateXboxMusicClient(_appId, _appIdSecret);
        }

        private async void MakeCall() {
          
        }


    }
}
