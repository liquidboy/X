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
    public sealed partial class Repositories : UserControl
    {
        public Repositories()
        {
            this.InitializeComponent();

            tlMain.AddTab("All");
            tlMain.AddTab("Public");
            tlMain.AddTab("Private");
            tlMain.AddTab("Sources");
            tlMain.AddTab("Forks");

            tlMain.OnTabChanged += TlMain_OnTabChanged;
        }

        private async void TlMain_OnTabChanged(object sender, EventArgs e)
        {
            if (!(sender is X.UI.LiteTab.Tab)) return;

            lbRepos.ItemsSource = null;
            lbRepos.Visibility = Visibility.Collapsed;


            var tab = sender as X.UI.LiteTab.Tab;

            switch (tab.Name)
            {
                case "Public": break;
                case "Private": break;
                case "Sources": break;
                case "Forks": break;
                case "All":
                default:
                    lbRepos.Visibility = Visibility.Visible;
                    lbRepos.ItemsSource = await InstanceLocator.Instance.GitClient.Repository.GetAllForCurrent();
                    break;

            }
        }





    }
}
