using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Browser.ViewModels
{
    public class AddTabVM : ViewModelBase
    {
        public string SearchQuery { get; set; }
        public string UriTitle { get; set; }

        private RelayCommand<string> _searchCommand;
        public RelayCommand<string> SearchCommand { get { return _searchCommand ?? (_searchCommand = new RelayCommand<string>(ExecuteSearchCommand)); } }
        private async void ExecuteSearchCommand(string uri)
        {
            if (uri.Length == 0)
            {
                //Messenger.Default.Send<XBrowser.Messages.NotificationMessage>(new XBrowser.Messages.NotificationMessage("", "Please enter a valid web address!", ConstantsService.SearchIconPath));
                return;
            }

            SearchQuery = CleanUpWebUri(uri);
            this.RaisePropertyChanged("SearchQuery");
        }

        private RelayCommand<object> _createTabCommand;
        public RelayCommand<object> CreateTabCommand { get { return _createTabCommand ?? (_createTabCommand = new RelayCommand<object>(ExecuteCreateTabCommand)); } }
        private async void ExecuteCreateTabCommand(object sender)
        {
            if (SearchQuery.Length == 0)
            {
                //Messenger.Default.Send<XBrowser.Messages.NotificationMessage>(new XBrowser.Messages.NotificationMessage("", "Please enter a valid web address!", ConstantsService.SearchIconPath));
                return;
            }

            Uri uri = new Uri(SearchQuery);

            //Messenger.Default.Send<CreateTabMessage>(new CreateTabMessage(SearchQuery, UriTitle, "http://" + uri.Host + "/favicon.ico?v2"));
            //Messenger.Default.Send<CloseAddTabFlyout>(new CloseAddTabFlyout());

        }


        public AddTabVM()
        {

        }

        private string CleanUpWebUri(string uri)
        {
            string ret = uri;

            if (uri.Contains("http://") || uri.Contains("https://"))
            {
                ret = uri;
            }
            else
            {
                ret = "http://" + uri;
            }

            return ret;
        }
    }
}
