using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace X.UI.Chrome
{
    public sealed class Header : Control
    {
        public Header()
        {
            this.DefaultStyleKey = typeof(Header);

            this.Loaded += Header_Loaded;
            this.Unloaded += Header_Unloaded;
        }

        private void Header_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Header_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnApplyTemplate()
        {



            base.OnApplyTemplate();
        }

        public void InitChrome(Application app, ApplicationView applicationView)
        {
            app.DebugSettings.IsTextPerformanceVisualizationEnabled = false;
            app.DebugSettings.IsBindingTracingEnabled = false;
            app.DebugSettings.IsOverdrawHeatMapEnabled = false;
            app.DebugSettings.EnableFrameRateCounter = false;


            var titleBar = applicationView.TitleBar;

            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ForegroundColor = Colors.Black;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            //Window.Current.SetTitleBar(recSmallTitle);
        }





        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }






        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Header), new PropertyMetadata(string.Empty, OnPropertyChanged));


        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as Header;
            if (d == null)
                return;

            //if (instance._root != null)
            //{
            //    instance.Invalidate();
            //}
        }

    }
}
