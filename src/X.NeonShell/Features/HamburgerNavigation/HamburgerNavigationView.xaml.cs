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
using X.NeonShell.Extensions;

namespace X.NeonShell.Features.HamburgerNavigation
{
    public sealed partial class HamburgerNavigationView : UserControl
    {
        public HamburgerNavigationView()
        {
            this.InitializeComponent();
        }

        private void NavRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            OnNavigationChanged?.Invoke(null, new NavigationChangedArgs() { Data = (string)rb.Tag, FriendlyText = (string)rb.Content });
        }


        
        public void SetSelectedMenuItem(string contentName) {

            foreach (var rb in this.FindVisualChildList<RadioButton>())
            {
                if (contentName == ConvertRadioButtonToView((string)rb.Content))
                {
                    rb.IsChecked = true;
                    return;
                }
            }
            
        }

        private string ConvertRadioButtonToView(string content) {
            var ctn = "";
            switch (content)
            {
                case "Home": ctn = "PublicDashboardView"; break;
                case "Your Account": ctn = "YourAccountView"; break;
                case "Your Photos": ctn = "YourDashboardView"; break;
            }
            return ctn;
        }
        
        public event EventHandler<NavigationChangedArgs> OnNavigationChanged;
    }

    public class NavigationChangedArgs : EventArgs {
        public string Data;
        public string FriendlyText;
    }
}
