using Octokit;
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
    public sealed partial class ucSample02 : UserControl
    {
        public GitHubClient GitClient { get; private set; }



        public ucSample02()
        {
            this.InitializeComponent();
            
            GitClient = new GitHubClient(new ProductHeaderValue("X"));

            
        }


        private async void PopulateSampleData()
        {
            var found = await GitClient.Repository.GetAllPublic(new PublicRepositoryRequest(0) { Since = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5)).Ticks });

            dynamic data1 = null;

            lbCommon.ItemsSource = data1;
            lbCommon.ItemTemplateToUse = 1;

        }
    }
}
