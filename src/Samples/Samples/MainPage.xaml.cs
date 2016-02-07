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
using X.UI.LiteTab;

namespace Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            tlMain.AddTab("Samples 1", true);
            tlMain.AddTab("Samples 2");
            tlMain.AddTab("Samples 3");

        }



        private void tlMain_OnTabChanged(object sender, EventArgs e)
        {
            ctl1.Visibility = Visibility.Collapsed;
            ctl2.Visibility = Visibility.Collapsed;
            ctl3.Visibility = Visibility.Collapsed;

            var selected = (Tab)sender;
            if (selected.Name == "Samples 1") { ctl1.Visibility = Visibility.Visible; }
            else if (selected.Name == "Samples 2") { ctl2.Visibility = Visibility.Visible; }
            else if (selected.Name == "Samples 3") { ctl3.Visibility = Visibility.Visible; }
        }


    }


    
}
