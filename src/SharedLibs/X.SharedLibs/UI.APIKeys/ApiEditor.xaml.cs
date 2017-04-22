using FlickrNet;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.Services.Data;

namespace X.UI.APIKeys
{
    public sealed partial class ApiEditor : UserControl
    {
        public event EventHandler SaveComplete;

        public APIKeyDataModel APIKey { get; set; }

        public ApiEditor()
        {
            this.InitializeComponent();
        }

        public bool IsLoggedIn
        {
            get { return (bool)GetValue(IsLoggedInProperty); }
            set { SetValue(IsLoggedInProperty, value); }
        }

        public static readonly DependencyProperty IsLoggedInProperty = DependencyProperty.Register("IsLoggedIn", typeof(bool), typeof(ApiEditor), new PropertyMetadata(false));

        private void butSave_Click(object sender, RoutedEventArgs e)
        {
         
            APIKey = new APIKeyDataModel()
            {
                APIKey = FlickrClientID.Text,
                APISecret = FlickrClientSecret.Text,
                APICallbackUrl = FlickrCallbackUrl.Text,
                Type = tbAPIType.Text,
                APIName = tbAPIName.Text,
                DeveloperUri = tbDeveloperUri.Text
            };

            StorageService.Instance.Storage.Insert(APIKey);

            SaveComplete?.Invoke(null, EventArgs.Empty);


        }
    }
}
