using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace X.Extension.ThirdParty.Groove.VM
{
    public class SplashVM : ViewModelBase
    {
        private string _appId = "";
        private string _appIdSecret = "";
        private string _accessToken = "";

        private RelayCommand<string> _requestLogin;

        public RelayCommand<string> RequestLogin { get { return _requestLogin ?? (_requestLogin = new RelayCommand<string>((arg) => { AttemptLogin(); })); } }

        

        private void AttemptLogin() {

        }

        private async void MakeCall() {
          
        }


    }
}
