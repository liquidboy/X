using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace X.UI.WebView
{
    public sealed class WebAuthView : Control
    {
        Windows.UI.Xaml.Controls.WebView _wvMain;//x
        Grid _root;


        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(WebAuthView), new PropertyMetadata(null));


        public WebAuthView()
        {
            this.DefaultStyleKey = typeof(WebAuthView);

            Loaded += WebAuthView_Loaded;
            Unloaded += WebAuthView_Unloaded;
        }

        private void WebAuthView_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void WebAuthView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {
            if (_root == null)
            {
                _root = GetTemplateChild("root") as Grid;
                _root.DataContext = this;
            }

            if (_wvMain == null)
            {
                _wvMain = GetTemplateChild("wvMain") as Windows.UI.Xaml.Controls.WebView;
                _wvMain.NavigationCompleted += _wvMain_NavigationCompleted;
                _wvMain.NavigationStarting += _wvMain_NavigationStarting;
                _wvMain.NavigationFailed += _wvMain_NavigationFailed;
            }

            base.OnApplyTemplate();
        }

        private void _wvMain_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            
        }

        private void _wvMain_NavigationStarting(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationStartingEventArgs args)
        {
            
        }

        private void _wvMain_NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            
        }
    }
}
