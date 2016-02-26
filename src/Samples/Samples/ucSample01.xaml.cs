
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
    public sealed partial class ucSample01 : UserControl
    {

        DispatcherTimer dt;

        public ucSample01()
        {
            this.InitializeComponent();

            dt = new DispatcherTimer();
            dt.Tick += Dt_Tick;
            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Start();

            this.Unloaded += UcSample01_Unloaded;
        }

        private void UcSample01_Unloaded(object sender, RoutedEventArgs e)
        {

            this.Unloaded -= UcSample01_Unloaded;
            dt.Stop();
            dt.Tick -= Dt_Tick;
        }

        private async void Dt_Tick(object sender, object e)
        {

            var current = ipProgress.Value1 + 20;
            current = current > ipProgress.Maximum1 ? 0 : current;
            //ipProgress.Value1 = current;
            //ipProgress.Invalidate();

            ipProgress.SetValue(X.UI.RichInput.Input.Value1Property, current);

        }

        private void Input_ValueChanged(object sender, RoutedEventArgs e)
        {

        }

        private void AttemptLogin(object sender, RoutedEventArgs e)
        {
            //var current = ipProgress.Value1 + 50;
            //current = current > ipProgress.Maximum1 ? 0 : current;

            //ipProgress.SetValue(X.UI.RichInput.Input.Value1Property, current);

        }
    }
}
