using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.NeonShell.Services;
using X.NeonShell.ViewModels;

namespace X.NeonShell.Views
{
    public sealed partial class FlickrLoginView : Page
    {

        public FlickrLoginView()
        {
            this.InitializeComponent();
        }
        
        public FlickrViewModel GetFVM()
        {
            return (FlickrViewModel)this.DataContext;
        }
        
        private void butLogin_Click(object sender, RoutedEventArgs e)
        {
            GetFVM().RequestFlickrAuthorization();

        }

        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
