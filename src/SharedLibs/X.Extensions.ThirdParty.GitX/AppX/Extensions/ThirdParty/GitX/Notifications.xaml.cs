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


namespace X.Extensions.ThirdParty.GitX
{
    public sealed partial class Notifications : UserControl
    {
        public Notifications()
        {
            this.InitializeComponent();

            
            tlMain.AddTab("Notifications");
            tlMain.AddTab("Watching");

            tlMain.OnTabChanged += TlMain_OnTabChanged;
            
        }

        private async void TlMain_OnTabChanged(object sender, EventArgs e)
        {
            if (! (sender is X.UI.LiteTab.Tab)) return;


            var tab = sender as X.UI.LiteTab.Tab;

            lbCommon.ItemsSource = null;
            
            switch (tab.Name) {
                case "Notifications":
                    //var data1 = await InstanceLocator.Instance.GitClient.Notification.GetAllForCurrent( new Octokit.NotificationsRequest() { All = true });
                    //lbCommon.ItemsSource = data1.Reverse();
                    //lbCommon.Visibility = Visibility.Visible;
                    //lbCommon.ItemTemplateToUse = 1;
                    //
                    ////var found = data1.Where(x => x.Subject.Title.Contains("Reenable Open")).FirstOrDefault();

                    break;
                case "Watching":
                default:
                    var data2 = await InstanceLocator.Instance.GitClient.Activity.Watching.GetAllForCurrent();
                    lbCommon.ItemsSource = data2.Reverse();
                    lbCommon.Visibility = Visibility.Visible;
                    lbCommon.ItemTemplateToUse = 2;
                    break;

            }
        }

    }
}
