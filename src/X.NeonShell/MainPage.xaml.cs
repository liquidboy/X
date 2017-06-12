using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.NeonShell.Features;
using X.NeonShell.Features.HamburgerNavigation;
using X.NeonShell.ViewModels;
using X.NeonShell.Views;

namespace X.NeonShell
{

    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        
        FlickrViewModel _fvm;
        
        public MainPage()
        {
            this.InitializeComponent();

            // This is a static public property that allows downstream pages (loaded in a Frame) to get a handle 
            // to the MainPage instance in order to call methods & access properties that are in this class.
            Current = this;

            InitVM();
            InitChrome();
            InitNavigation();
            InitView();
            
        }


        private void InitNavigation() {
            ContentFrame.Navigated += (s, args) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ContentFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
                
                ctlHamburgerNavigationView.SetSelectedMenuItem(ContentFrame.SourcePageType.Name);

            };

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

        }
        
        private void InitVM() {
            if (_fvm == null)
            {
                _fvm = new FlickrViewModel(Dispatcher);
                this.DataContext = _fvm;
            }
        }

        private void InitChrome()
        {
            ctlHeader.InitChrome(App.Current, ApplicationView.GetForCurrentView());
        }

        private void InitView() {
            CheckIfFlickrLoggedIn();
            
        }



        public void CheckIfFlickrLoggedIn()
        {
            if (_fvm.IsFlickrLoginDetailsCached())
            {
                ContentFrame.Navigate(typeof(YourDashboardView));
            }
            else
            {
                ContentFrame.Navigate(typeof(PublicDashboardView));
            }
        }


        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                e.Handled = true;
                ContentFrame.GoBack();
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void NavigationChanged(object sender, NavigationChangedArgs e)
        {
            switch (e.FriendlyText)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(PublicDashboardView));
                    break;
                case "Your Account":
                    ContentFrame.Navigate(typeof(YourAccountView));
                    break;
                case "Your Photos":
                    ContentFrame.Navigate(typeof(YourDashboardView));
                    break;
            }

        }
    }
}
