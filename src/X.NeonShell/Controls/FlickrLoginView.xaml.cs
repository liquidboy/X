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

namespace X.NeonShell.Controls
{
    public sealed partial class FlickrLoginView : UserControl
    {
        public FlickrLoginView()
        {
            this.InitializeComponent();
        }

        public void GoPublicCommand()
        {
            // NavigationService.Current.Navigate(eViews.Dashboard);
        }

        private void butLogin_Click(object sender, RoutedEventArgs e)
        {
            //_fvm.RequestFlickrAuthorization();

        }

        private void butLogout_Click(object sender, RoutedEventArgs e)
        {
            //_fvm.RequestLogout();
        }

    }
}
