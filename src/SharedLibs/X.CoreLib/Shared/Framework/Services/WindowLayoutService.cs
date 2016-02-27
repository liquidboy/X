
using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;


namespace FavouriteMX.Shared.Services
{
    public class WindowLayoutService
    {


        private static WindowLayoutService Instance = new WindowLayoutService();

        public static event EventHandler OnWindowLayoutRaised;

        //public static ApplicationViewState CurrentViewState { get; set; }

        private WindowLayoutService()
        {


        }

        public static void Init()
        {
            Window.Current.SizeChanged += WindowSizeChanged;  

        }

        public static void Unload()
        {
            Window.Current.SizeChanged -= WindowSizeChanged;  
        }


        private static void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {

            //CurrentViewState = ApplicationView.Value;

            //if (CurrentViewState == ApplicationViewState.Filled)
            //{
            //    System.Diagnostics.Debug.WriteLine("viewState is Filled");
            //}
            //else if (CurrentViewState == ApplicationViewState.FullScreenLandscape)
            //{
            //    System.Diagnostics.Debug.WriteLine("viewState is FullScreenLandscape");
            //}
            //else if (CurrentViewState == ApplicationViewState.Snapped)
            //{
            //    System.Diagnostics.Debug.WriteLine("viewState is Snapped");
            //}
            //else if (CurrentViewState == ApplicationViewState.FullScreenPortrait)
            //{
            //    System.Diagnostics.Debug.WriteLine("viewState is FullScreenPortrait");
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("viewState is something unexpected");
            //}


            if (OnWindowLayoutRaised != null) OnWindowLayoutRaised(null, new WindowLayoutEventArgs() { 
                //ViewState = CurrentViewState, 
                Size = e.Size });

        }

    }


    public class WindowLayoutEventArgs : EventArgs
    {
        //public ApplicationViewState ViewState { get; set; }
        public Size Size { get; set; }
    }

}
