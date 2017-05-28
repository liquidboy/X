using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml.Controls;
using X.CoreLib.Shared.Services;
using X.NeonShell.Controls;
using X.NeonShell.ViewModels;

namespace X.NeonShell.Services
{
    interface INavigationServiceParticipant
    {
        void UnloadByNavigationService();
        void HiddenByNavigationService();
        void OnLoadFromNavigationService(FlickrViewModel obj);
        void RefreshFromNavigationService();

        //void ProcessMessage(string action, object data, EventArgs eventargs);
        Task ProcessMessage(string action, object data, EventArgs eventargs);
    }

    public enum eViews
    {
        None,
        SplashScreen,
        FlickrLogin,
        Dashboard,
        ExplorerMX,
        NoConnection,
        TwitterLogin,
        OneDriveLogin,
        LiveStream
    }

    public class NavigationService : NavigationServiceBase
    {
        public eViews CurrentView;
        private FlickrViewModel _fvm = null;
        private Dictionary<eViews, INavigationServiceParticipant> _viewsCache = new Dictionary<eViews, INavigationServiceParticipant>();


        private static NavigationService _service = null;
        public static NavigationService Current
        {
            get
            {
                NavigationService result;

                if (NavigationService._service == null)
                {
                    NavigationService._service = new NavigationService();
                }
                result = NavigationService._service;

                return result;
            }
        }





        public NavigationService()
        {

        }

        public void Init(ContentControl contentFrame1, ContentControl contentFrame2, ref FlickrViewModel fvm)
        {
            Init(contentFrame1, contentFrame2);
            _fvm = fvm;

            _fvm.ChangeState += (object sender, EventArgs e) => {
                _viewsCache[CurrentView].ProcessMessage((string)sender, null, e);
            };


        }


        public void ProcessMessage(string action, object data, EventArgs eventargs)
        {

            _viewsCache[CurrentView].ProcessMessage(action, data, eventargs);
        }



        //public static async Task<bool> RequestAuthorization2Complete(WebAuthenticationStatus ResponseStatus, string ResponseData, uint ResponseErrorDetail)
        //{
        //    return await _fvm.RequestAuthorization2Complete(ResponseStatus, ResponseData, ResponseErrorDetail);
        //}



        public void Navigate(eViews viewName, object parameter = null)
        {
            //LoggingService.LogInformation(viewName.ToString(), "NavigationService.Navigate");

            if (CurrentView == viewName) return;

            //clear previous view
            if (_viewsCache.ContainsKey(CurrentView))
            {
                var viewCurrent = _viewsCache[CurrentView];
                //if (viewCurrent is IDisposable) ((IDisposable)viewCurrent).Dispose();
                viewCurrent.UnloadByNavigationService();
                viewCurrent = null;
                //_viewsCache[CurrentView] = null;
                //_viewsCache.Remove(CurrentView);
            }

            CurrentView = viewName;


            INavigationServiceParticipant view = null;

            if (!_viewsCache.ContainsKey(viewName))
            {
                switch (viewName)
                {
                    //case eViews.SplashScreen: view = new SplashScreenView(); break;
                    //case eViews.FlickrLogin: view = new FlickrLoginView(); break;
                    //case eViews.Dashboard: view = new DashboardView(); break;
                    //case eViews.ExplorerMX: view = new ExplorerMXView(); break;
                    //case eViews.NoConnection: view = new NoConnectionView(); break;
                    //case eViews.TwitterLogin: view = new TwitterLoginView(); break;
                    //case eViews.OneDriveLogin: view = new OneDriveLoginView(); break;
                    //case eViews.LiveStream: view = new LiveStreamView(); break;
                }

                if (view != null) _viewsCache.Add(viewName, view);
            }
            else
            {
                view = _viewsCache[viewName];
            }

            LoadFrame(view);
            view.OnLoadFromNavigationService(_fvm);


            //Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Action = "ShowTopMagicLayer", Identifier = "MasterPageView" });

        }

        public eViews temporaryNextView(eViews viewName)
        {
            switch (viewName)
            {
                case eViews.SplashScreen: return eViews.TwitterLogin; break;  //was flickrlogin
                case eViews.FlickrLogin: return eViews.ExplorerMX; break;
                case eViews.ExplorerMX: return eViews.Dashboard; break;
                case eViews.Dashboard: return eViews.NoConnection; break;
                case eViews.NoConnection: return eViews.SplashScreen; break;
                case eViews.TwitterLogin: return eViews.ExplorerMX; break;
                case eViews.OneDriveLogin: return eViews.Dashboard; break;
                case eViews.LiveStream: return eViews.Dashboard; break;
            }

            return eViews.SplashScreen;
        }


        public static void NavigateOnUI(eViews viewName, object parameter = null)
        {
            Windows.UI.Core.DispatchedHandler invokedHandler = new Windows.UI.Core.DispatchedHandler(() =>
            {
                // LoggingService.LogInformation("navigating to " + viewName, "NavigationService.NavigateOnUI");

                Current.Navigate(viewName, parameter);
            });

            //if (_mainFrame != null) await _mainFrame.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, invokedHandler);
            //else if (_contentFrame != null) await _contentFrame.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, invokedHandler);
        }


        public void NavigateBasedOnWindowsLayoutChange(WindowLayoutEventArgs args)
        {

        }

        public void NavigateBasedOnNetworkConnectivity(bool isConnected)
        {
            if (!isConnected)
            {
                Current.Navigate(eViews.NoConnection);
            }
        }

        public async Task<bool> RequestFlickrAuthorizationComplete(WebAuthenticationStatus ResponseStatus, string ResponseData, uint ResponseErrorDetail)
        {
            return await _fvm.RequestFlickrAuthorizationComplete(ResponseStatus, ResponseData, ResponseErrorDetail);


        }
    }
}
