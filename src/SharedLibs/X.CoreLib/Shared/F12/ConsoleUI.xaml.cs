using SumoNinjaMonkey.Framework.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace CoreLib.Shared.F12
{
    public sealed partial class ConsoleUI : UserControl
    {
        public ConsoleUI()
        {
            this.InitializeComponent();
        }

        private void butRefresh_Click(object sender, RoutedEventArgs e)
        {

            RefreshMessagesFromStore();
        }


        private void RefreshMessagesFromStore() {
            StringBuilder sb = new StringBuilder();

            foreach(var msg in LoggingService.LoggingMessages.OrderByDescending(x=>x.DateStamp))
            {
                sb.AppendFormat("{0} : {1}  {2}", msg.Message, msg.FriendlyMessage, msg.Exception);
                sb.AppendLine();
            }

            tbFeedback.Text = sb.ToString();
        }

        private async void butClear_Click(object sender, RoutedEventArgs e)
        {
            await LoggingService.Clear();
            RefreshMessagesFromStore();
        }

        private void butDockAlernate_Click(object sender, RoutedEventArgs e)
        {
            if(F12Service.CurrentDockPosition == eDockPosition.bottom)
                F12Service.DockPanelTo( eDockPosition.right, 400);
            else
                F12Service.DockPanelTo(eDockPosition.bottom, 200);
        }
    }
}
