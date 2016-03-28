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


namespace X.Viewer.MediaView
{
    public sealed partial class MediaControls : UserControl
    {

        DispatcherTimer dtChangeValue;
        bool isRunning = false;

        public MediaControls()
        {
            this.InitializeComponent();

            dtChangeValue = new DispatcherTimer();
            dtChangeValue.Interval = TimeSpan.FromSeconds(2);
            dtChangeValue.Tick += DtChangeValue_Tick;
        }

        private void DtChangeValue_Tick(object sender, object e)
        {
            isRunning = true;
            dtChangeValue.Stop();
            
            var me = (MediaElement)DataContext;
            me.Position = TimeSpan.FromMinutes(sldMainMover.Value);
            //me.Play();
            isRunning = false;
        }

        private void sldMainMover_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            if (!isRunning) dtChangeValue.Start();
        }

        private void butPlay_Click(object sender, RoutedEventArgs e)
        {
            var me = (MediaElement)DataContext;
            me.Play();
        }

        private void butPause_Click(object sender, RoutedEventArgs e)
        {
            var me = (MediaElement)DataContext;
            me.Pause();
        }

        private void butStop_Click(object sender, RoutedEventArgs e)
        {
            var me = (MediaElement)DataContext;
            me.Stop();
        }
    }
}
