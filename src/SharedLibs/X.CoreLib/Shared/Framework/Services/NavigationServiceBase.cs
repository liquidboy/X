using System;
//using FavouriteMX.Shared.Views;
using SumoNinjaMonkey.Framework.Services;
using Windows.UI.Xaml.Controls;
using FavouriteMX.Shared.Views;
namespace FavouriteMX.Shared.Services
{
    public class NavigationServiceBase
    {
        private static int _frontFrame = 0;
        //public static Frame _mainFrame = null;
        public ContentControl _contentFrame1 = null;
        public ContentControl _contentFrame2 = null;
        public static NavigationServiceBase Instance = new NavigationServiceBase();
        

        public NavigationServiceBase()
        {
        }


        public static void Init(Frame mainFrame)
        {

            //NavigationServiceBase._mainFrame = mainFrame;
            //_mainFrame = mainFrame;
        }

        public void Init(ContentControl contentFrame1, ContentControl contentFrame2)
        {

            _contentFrame1 = contentFrame1;
            _contentFrame2 = contentFrame2;
            
            _contentFrame1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _contentFrame2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        
        private void FlipFrame()
        {
            
            if(_frontFrame == 1)
            {
                _contentFrame2.Visibility = Windows.UI.Xaml.Visibility.Visible;
                _contentFrame1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                _frontFrame = 2;
            }
            else
            {
                _contentFrame2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                _contentFrame1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                _frontFrame = 1;
            }
        }

        public void LoadFrame(object view)
        {
            if (_frontFrame == 1)
            {
                _contentFrame2.Content = view;
            }else
            {
                _contentFrame1.Content = view;
            }

            FlipFrame();
            UnloadBackFrame();
        }

        
        private void UnloadBackFrame()
        {
            if (_frontFrame == 1)
            {
                _contentFrame2.Content = null;
            }
            else
            {
                _contentFrame1.Content = null;
            }
        }


        public void NavigateBase(string viewName, object parameter = null)
        {
            //if (!string.IsNullOrEmpty(viewName)))
            //{
            //    _mainFrame.Navigate(typeof(HomeView), parameter);
            //}
        }


        public void GoBack()
        {
            //ShareManager.Instance.Clear();


            //if (_mainFrame != null && _mainFrame.CanGoBack)
            //{
            //    LoggingService.LogInformation("navigating back", "NavigationServiceBase.GoBack");
            //    _mainFrame.GoBack();
            //    return;
            //}



        }
    }
}
