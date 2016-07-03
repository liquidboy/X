using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.UI.RichInput;

namespace X.Extensions.ThirdParty.GitX
{
    public sealed partial class Login : UserControl
    {

        public event EventHandler OnLoggedIn;

        public Login()
        {
            this.InitializeComponent();
        }

        private async void AttemptLogin(object sender, RoutedEventArgs e)
        {
            try {
                InstanceLocator.Instance.GitClient.Credentials = new Credentials(ipUserid.Value, ipPassword.Value);
                var auth = await InstanceLocator.Instance.GitClient.Authorization.GetAll();
                //var found = await client.Repository.GetAllPublic();
                lbAuths.ItemsSource = auth;
                //var ffff = found.Count();

                if (OnLoggedIn != null) OnLoggedIn(null, EventArgs.Empty);
            }
            catch //(Exception ex) 
            { }
            
        }
        
        private void ValidateLoginButton() {
            //if (ipUserid.Value.Length > 0 && ipPassword.Value.Length > 0) butLogin.IsEnabled = true;
            //else butLogin.IsEnabled = false;
        }

        private void Input_ValueChanged(object sender, RoutedEventArgs e)
        {
            ValidateLoginButton();
        }
    }
}
