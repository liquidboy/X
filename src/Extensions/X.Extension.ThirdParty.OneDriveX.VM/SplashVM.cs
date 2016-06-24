using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extension.ThirdParty.OneDriveX.VM
{
    public class SplashVM : ViewModelBase
    {
        private const string _appIdMSA = "861dc4f2-ab87-4677-bc91-794c2034cc10";
        private readonly string[] _scopesMSA = new []{ "onedrive.readwrite","offline_access" };

        private const string _clientIDAAD = "";
        private const string _clientSecretAAD = "";
        private const string _returnUrlAAD = "";
        private const string _oneDriveApiEndpointAAD = "";
        private const string _serviceResourceIdAAD = "";
        private const string _serviceUrlAAD = "";

        private IOneDriveClient oneDriveClient;

        private RelayCommand<string> _requestOnedriveLogin;

        public RelayCommand<string> RequestOnedriveLogin { get { return _requestOnedriveLogin ?? (_requestOnedriveLogin = new RelayCommand<string>((arg) => { MSAAuthenticate(); })); } }

        private async Task MSAAuthenticate() {
            if (oneDriveClient != null) return;
            oneDriveClient = await OneDriveClientExtensions.GetAuthenticatedClientUsingWebAuthenticationBroker(_appIdMSA, _scopesMSA);
            //oneDriveClient = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(_scopesMSA);
            await oneDriveClient?.AuthenticateAsync();
        }

        private async void AADAuthenticate()
        {
            if (oneDriveClient != null) return;
            oneDriveClient = BusinessClientExtensions.GetClient(
                new AppConfig()
                {
                    ActiveDirectoryAppId = _clientIDAAD,
                    ActiveDirectoryClientSecret = _clientSecretAAD,
                    ActiveDirectoryReturnUrl = _returnUrlAAD,
                    ActiveDirectoryServiceEndpointUrl = _oneDriveApiEndpointAAD,
                    ActiveDirectoryServiceResource = _serviceResourceIdAAD,
                    ActiveDirectoryAuthenticationServiceUrl = _serviceUrlAAD
                });
            await oneDriveClient?.AuthenticateAsync();
        }

        private async void SignOut() {
            await oneDriveClient?.SignOutAsync();
        }

    }
}
