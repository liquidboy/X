using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.NeonShell.ViewModels;
using X.NeonShell.Views;

namespace X.NeonShell
{

    public sealed partial class MainPage : Page
    {
        FlickrViewModel _fvm;


        public MainPage()
        {
            this.InitializeComponent();
            InitVM();
            InitChrome();
            InitView();
        }

        private void InitVM() {
            if (_fvm == null)
            {
                _fvm = new FlickrViewModel(Dispatcher);
                this.DataContext = _fvm;
            }
        }

        public void CheckIfFlickrLoggedIn()
        {
            if (_fvm.IsFlickrLoginDetailsCached())
            {
                ContentFrame.Navigate(typeof(FlickrHomeView));
            }
            else
            {
                ContentFrame.Navigate(typeof(FlickrLoginView));
            }
        }

        private void InitChrome()
        {
            ctlHeader.InitChrome(App.Current, ApplicationView.GetForCurrentView());
        }

        private void InitView() {
            CheckIfFlickrLoggedIn();
            
        }
    }
}
