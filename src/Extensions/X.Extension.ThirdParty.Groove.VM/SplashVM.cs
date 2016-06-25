using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Microsoft.Xbox.Music.Platform.Client;
using Microsoft.Xbox.Music.Platform.Contract.DataModel;
using Windows.UI.Xaml;
using X.Services.Data;

namespace X.Extension.ThirdParty.Groove.VM
{
    public class SplashVM : ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public string GroupingType { get; set; } = "Groove";
        
        APIKeyDataModel apiKey;
        IXboxMusicClient _client;

        private List<Album> _Albums;
        public List<Album> Albums { get { return _Albums; } set { _Albums = value; RaisePropertyChanged(); } }

        private RelayCommand<string> _requestLogin;
        public RelayCommand<string> RequestLogin { get { return _requestLogin ?? (_requestLogin = new RelayCommand<string>( async (arg) => { await AttemptLoginAsync(); })); } }

        Visibility _IsLoginVisible;
        public Visibility IsLoginVisible { get { return _IsLoginVisible; } set { _IsLoginVisible = value; RaisePropertyChanged(); } }

        Visibility _IsAPIEditorVisible;
        public Visibility IsAPIEditorVisible { get { return _IsAPIEditorVisible; } set { _IsAPIEditorVisible = value; RaisePropertyChanged(); } }



        public SplashVM()
        {
            IsLoginVisible = Visibility.Collapsed;
            IsAPIEditorVisible = Visibility.Visible;
            GetAPIData();
            PopulatePassportData();
        }

        private void GetAPIData()
        {
            var apis = StorageService.Instance.Storage.RetrieveList<APIKeyDataModel>();
            if (apis != null && apis.Count > 0) apiKey = apis.Where(x => x.Type == GroupingType).FirstOrDefault();
        }

        private async void PopulatePassportData()
        {
            //var data = StorageService.Instance.Storage.RetrieveList<PassportDataModel>().Where(x=>x.PassType == GroupingType).ToList();
            //if (data != null && data.Count > 0)
            if (apiKey!= null && !string.IsNullOrEmpty(apiKey.APIKey))
            {
                IsLoginVisible = Visibility.Visible;
                IsAPIEditorVisible = Visibility.Collapsed;
            }
            else
            {
                IsLoginVisible = Visibility.Collapsed;
                IsAPIEditorVisible = Visibility.Visible;
            }
        }

        private async Task AttemptLoginAsync() {
            if (_client != null) return;
            _client = XboxMusicClientFactory.CreateXboxMusicClient(apiKey.APIKey, apiKey.APISecret);
            await MakeCallAsync("Sia", Namespace.music, 20);
            IsLoginVisible = Visibility.Collapsed;
        }

        private async Task MakeCallAsync(string qry, Namespace nspc = Namespace.music, int maxItems = 5) {
            if (_client == null) return;
            string country = null;
            ContentResponse searchResponse = await _client.SearchAsync(nspc, qry, source: ContentSource.Catalog, filter: SearchFilter.Albums, maxItems: maxItems, country: country);
            var count = searchResponse.Albums.TotalItemCount;
            Albums = searchResponse.Albums.Items;
        }


    }
}
