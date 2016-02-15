using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Samples
{
    public sealed partial class Chrome : UserControl
    {
        public Chrome()
        {
            this.InitializeComponent();
        }

        public void InitChrome()
        {
            App.Current.DebugSettings.IsTextPerformanceVisualizationEnabled = false;
            App.Current.DebugSettings.IsBindingTracingEnabled = false;
            App.Current.DebugSettings.IsOverdrawHeatMapEnabled = false;
            App.Current.DebugSettings.EnableFrameRateCounter = false;


            var applicationView = ApplicationView.GetForCurrentView();
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
    }
}
