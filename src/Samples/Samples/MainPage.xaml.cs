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


namespace Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            tlMain.AddTab("LiteTab 1", true);
            tlMain.AddTab("LiteTab 2");
            tlMain.AddTab("LiteTab 3");

        }

        private void Input_ValueChanged(object sender, RoutedEventArgs e)
        {

        }

        private void AttemptLogin(object sender, RoutedEventArgs e)
        {

        }
    }
}
