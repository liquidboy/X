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
using X.Services.Data;

namespace X.UI.APIKeys
{
    public sealed partial class Settings : UserControl
    {
        public int ApiKeyCount { get; set; }

        public Settings()
        {
            this.InitializeComponent();

            RefreshData();
        }

        private void RefreshData() {
            ApiKeyCount = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>().Count();
        }

        private void butClearKeys_Click(object sender, RoutedEventArgs e)
        {
            StorageService.Instance.Storage.Truncate<APIKeyDataModel>();
            RefreshData();
        }
    }
}
