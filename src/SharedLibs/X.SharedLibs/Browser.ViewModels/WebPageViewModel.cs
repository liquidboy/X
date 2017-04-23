using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Services.Data;

namespace X.Browser
{
    public class WebPageViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Index1 { get; set; }
        public bool HasFocus { get; set; }
        public string DisplayTitle { get; set; }
        public string FaviconUri { get; set; }
        public bool ShowPadlock { get; set; }
        public string UriQueryString { get; set; }
        public string UriPart1 { get; set; }
        public string UriPart2 { get; set; }
        public string UriPart3 { get; set; }
        public string UriPart4 { get; set; }
        public string PrimaryFontFamily { get; set; }
        public string PrimaryBackgroundColor { get; set; }
        public string PrimaryForegroundColor { get; set; }
        public ObservableCollection<NameValue> QueryNames { get; set; }
        public string OriginalUri { get; set; }


        bool _IsPinned;
        public bool IsPinned { get { return _IsPinned; } set { _IsPinned = value; RaisePropertyChanged(); } }

        DateTime _LastRefreshedDate;
        public DateTime LastRefreshedDate {
            get { return _LastRefreshedDate; }
            set {
                _LastRefreshedDate = value;
                RaisePropertyChanged();

                if (Id > 0 && _LastRefreshedDate != DateTime.MinValue) X.Services.Data.StorageService.Instance.Storage.UpdateFieldById<WebPageDataModel>(Id, "LastRefreshedDate", LastRefreshedDate.ToUniversalTime());
            } }

        private string _uri;
        public string Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                _uri = value;

                var temp = _uri.Split(".".ToCharArray());

                Uri tempUri = new Uri(Uri);

                this.UriQueryString = tempUri.Query;
                var parts = this.UriQueryString.Split("&".ToCharArray());
                this.QueryNames = new ObservableCollection<NameValue>();
                foreach (var qnp in parts)
                {
                    if (qnp.Length > 1)
                    {
                        var qnpParts = qnp.Split("=".ToCharArray());
                        var qn = qnpParts[0];
                        this.QueryNames.Add(new NameValue() { Name = qn.Replace("?", ""), Value = qnpParts[1] });
                    }
                }


                this.UriPart2 = tempUri.Host;
                this.UriPart3 = tempUri.Authority;
                this.UriPart4 = tempUri.Port.ToString();
                this.UriPart1 = tempUri.Scheme;
                RaisePropertyChanged("UriPart1");


                this.ShowPadlock = tempUri.Scheme == "https" ? true : false;
                RaisePropertyChanged("ShowPadlock");

                //hrm
                string ua = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X)" + "AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25";
                Windows.Web.Http.HttpRequestMessage hrm = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, tempUri);
                hrm.Headers.Add("User-Agent", ua);
                //webView1.NavigateWithHttpRequestMessage(hrm);

                if (!string.IsNullOrEmpty(Uri) && string.IsNullOrEmpty(OriginalUri)) OriginalUri = Uri;

                Uid = FlickrNet.UtilityMethods.MD5Hash(OriginalUri);

            }

        }


        private RelayCommand<object> _togglePinCommand;

        public RelayCommand<object> TogglePinCommand { get { return _togglePinCommand ?? (_togglePinCommand = new RelayCommand<object>(ExecuteTogglePinCommand)); } }


        private void ExecuteTogglePinCommand(object obj)
        {

            var vm = (ViewModels.BrowserVM)obj;
            var uriHash = FlickrNet.UtilityMethods.MD5Hash(OriginalUri); //   e.Uri);
            

            IsPinned = !IsPinned;


            if (IsPinned)
            {
                X.Services.Tile.Service.UpdateSecondaryTile(uriHash, DisplayTitle, FaviconUri,
                    "ms-appdata:///local/tile/" + uriHash + "-150x150.png",
                    "ms-appdata:///local/tile/" + uriHash + "-310x150.png",
                    "ms-appdata:///local/tile/" + uriHash + "-310x310.png");
            }
            else {
                X.Services.Tile.Service.DeleteSecondaryTile(uriHash);
            }

            if (Id > 0) X.Services.Data.StorageService.Instance.Storage.UpdateFieldById<WebPageDataModel>(Id, "IsPinned", IsPinned ? 1 : 0);

            //locked stuff
            if (IsPinned)
            {
                var tabFoundInTabs = vm.Tabs.Where(x => x.Id == Id).First();
                if (tabFoundInTabs != null)
                {
                    vm.Tabs.Remove(tabFoundInTabs);
                    vm.LockedTabs.Add(tabFoundInTabs);
                }
            }
            else {
                var tabFoundInLocked = vm.LockedTabs.Where(x => x.Id == Id).First();
                if (tabFoundInLocked != null)
                {
                    vm.Tabs.Add(tabFoundInLocked);
                    vm.LockedTabs.Remove(tabFoundInLocked);
                }

            }


        }







        public void ExternalRaisePropertyChanged(string propName) { RaisePropertyChanged(propName); }


    }
}
