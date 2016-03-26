using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using X.Browser.Messages;
using X.Services.Data;

namespace X.Browser.ViewModels
{
    partial class BrowserVM
    {
        public ObservableCollection<TabViewModel> Tabs { get; set; }

        TabViewModel _selectedTab;
        public TabViewModel SelectedTab {
            get { return _selectedTab; }
            set {

                //unselect previous one
                if (_selectedTab != null && _selectedTab != value) {
                    _selectedTab.HasFocus = false;
                    _selectedTab.ExternalRaisePropertyChanged("HasFocus");

                    _selectedTab.Foreground = "Black";
                    _selectedTab.ExternalRaisePropertyChanged("Foreground");

                    _selectedTab.RightBorderColor = "#FFB8B8B8";
                    _selectedTab.ExternalRaisePropertyChanged("RightBorderColor");
                }

                //select the new one
                _selectedTab = value;
                if (_selectedTab != null) {
                    _selectedTab.HasFocus = true;
                    _selectedTab.ExternalRaisePropertyChanged("HasFocus");

                    _selectedTab.Foreground = "White";
                    _selectedTab.ExternalRaisePropertyChanged("Foreground");

                    _selectedTab.RightBorderColor = "Black";
                    _selectedTab.ExternalRaisePropertyChanged("RightBorderColor");
                }


               
                ExposedNotifyPropertyChanged("SelectedTab"); }
        }

        public bool IsShowingAddTab { get; set; }
        public bool IsShowingMoreTab { get; set; }

        private RelayCommand<object> _tabChangedCommand;

        private RelayCommand<object> _tabAddCommand;

        private RelayCommand<object> _tabMoreCommand;

        public RelayCommand<object> TabChangedCommand { get { return _tabChangedCommand ?? (_tabChangedCommand = new RelayCommand<object>(ExecuteTabChangedCommand)); } }

        private void ExecuteTabChangedCommand(object obj)
        {
            if (this.IsShowingAddTab)
            {
                Messenger.Default.Send(new CloseAddTabFlyout());
            }

            this.IsShowingAddTab = false;
            this.NotifyPropertyChanged("IsShowingAddTab");

            this.IsShowingMoreTab = false;
            this.NotifyPropertyChanged("IsShowingMoreTab");


            TabViewModel previousTab = null;

            foreach (var tab in this.Tabs)
            {
                if (((TabViewModel)obj).DisplayTitle == tab.DisplayTitle)
                {
                    tab.HasFocus = true;
                    tab.Foreground = "White";
                    if (previousTab != null)
                    {
                        previousTab.RightBorderColor = "";
                        previousTab.ExternalRaisePropertyChanged("RightBorderColor");
                    }
                    tab.RightBorderColor = "black";
                }
                else
                {
                    tab.HasFocus = false;
                    tab.Foreground = "Black";
                    tab.RightBorderColor = "#FFB8B8B8";
                }
                tab.ExternalRaisePropertyChanged("HasFocus");
                tab.ExternalRaisePropertyChanged("Foreground");
                tab.ExternalRaisePropertyChanged("RightBorderColor");

                previousTab = tab;
            }

            this.SelectedTab = obj as TabViewModel;
            this.NotifyPropertyChanged("SelectedTab");

            Messenger.Default.Send(new ShowOnebox());
        }

        public RelayCommand<object> TabAddCommand { get { return _tabAddCommand ?? (_tabAddCommand = new RelayCommand<object>(ExecuteTabAddCommand)); } }
        private void ExecuteTabAddCommand(object obj)
        {
            this.IsShowingAddTab = true;
            this.NotifyPropertyChanged("IsShowingAddTab");

            this.IsShowingMoreTab = false;
            this.NotifyPropertyChanged("IsShowingMoreTab");

            HideOneBox();

            Messenger.Default.Send(new SetAddTabSearchBoxFocus());

        }

        public RelayCommand<object> TabMoreCommand { get { return _tabMoreCommand ?? (_tabMoreCommand = new RelayCommand<object>(ExecuteTabMoreCommand)); } }
        private void ExecuteTabMoreCommand(object obj)
        {
            if (this.IsShowingAddTab)
            {
                Messenger.Default.Send(new CloseAddTabFlyout());
            }

            this.IsShowingAddTab = false;
            this.NotifyPropertyChanged("IsShowingAddTab");

            this.IsShowingMoreTab = true;
            this.NotifyPropertyChanged("IsShowingMoreTab");

            HideOneBox();

            foreach (var tab in Tabs) { tab.ExternalRaisePropertyChanged("LastRefreshedDate");  }

            Messenger.Default.Send(new SetMoreTabSearchBoxFocus());
        }


        private void LoadTabs(string defaultTabUid, Uri defaultUri)
        {
            Tabs = new ObservableCollection<TabViewModel>();

            var data = App.StorageSvc.Storage.RetrieveList<WebPageDataModel>();
            if (data.Count == 0)
            {
                LoadDefaultTabs();
                DeleteTabs();
                SaveTabs(Tabs);
            }
            else {

                foreach (var d in data)
                {
                    var tempTab = new TabViewModel() { DisplayTitle = d.DisplayTitle, FaviconUri = d.FaviconUri, HasFocus = false, Uri = d.Uri, TabChangedCommand = this.TabChangedCommand, LastRefreshedDate = d.LastRefreshedDate, IsPinned = d.IsPinned };
                    tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
                    tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
                    tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
                    tempTab.Foreground = "Black";
                    tempTab.RightBorderColor = "#FFB8B8B8";

                    Tabs.Add(tempTab);
                    

                }

                if (!string.IsNullOrEmpty( defaultTabUid))
                {
                    var found = Tabs.Where(x => x.Uid == defaultTabUid).First();
                    if (found != null ) this.SelectedTab = found;
                    else this.SelectedTab = Tabs[0];
                }
                else this.SelectedTab = Tabs[0];

            }

            ExposedNotifyPropertyChanged("SelectedTab");
        }

        private void SaveTabs(ObservableCollection<TabViewModel> TabsToSave)
        {
            foreach (var t in TabsToSave)
            {
                var wpm = new WebPageDataModel() { DisplayTitle = t.DisplayTitle, Uid = Guid.NewGuid().ToString(), Uri = t.Uri, FaviconUri = t.FaviconUri, HasFocus = t.HasFocus, LastRefreshedDate = t.LastRefreshedDate };

                App.StorageSvc.Storage.Insert(wpm);

            }
        }

        private void DeleteTabs()
        {
            App.StorageSvc.Storage.TruncateAll();
        }

        private void LoadDefaultTabs()
        {

            var tempTab = new TabViewModel() { DisplayTitle = "Microsoft", FaviconUri = "http://www.microsoft.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.microsoft.com?test=1", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);
            this.SelectedTab = tempTab;


            tempTab = new TabViewModel() { DisplayTitle = "Twitter", FaviconUri = "http://www.twitter.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.twitter.com:80?a=1&b=2", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

            tempTab = new TabViewModel() { DisplayTitle = "airbnb", FaviconUri = "http://www.airbnb.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.airbnb.com", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

            tempTab = new TabViewModel() { DisplayTitle = "Flickr", FaviconUri = "http://www.flickr.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.flickr.com", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

            tempTab = new TabViewModel() { DisplayTitle = "Bing", FaviconUri = "http://www.bing.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.bing.com", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

            tempTab = new TabViewModel() { DisplayTitle = "Azure", FaviconUri = "http://www.windowsazure.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.windowsazure.com", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

            tempTab = new TabViewModel() { DisplayTitle = "Apple", FaviconUri = "http://www.apple.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.apple.com", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

            tempTab = new TabViewModel() { DisplayTitle = "Google", FaviconUri = "http://www.google.com/favicon.ico?v2", HasFocus = false, Uri = "http://www.google.com", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.RightBorderColor = "#FFB8B8B8";
            Tabs.Add(tempTab);

        }

        private string DetermineFontFamily(string title)
        {
            switch (title)
            {
                case "Microsoft": return "Segoe UI";
                case "Apple": return "Calibri";
                case "Google": return "Arial";
                case "Uber": return "Felix Titling";
                default: return "Segoe UI";
            }
        }

        private string DeterminePrimaryBackgroundColor(string title)
        {
            switch (title)
            {
                //case "Microsoft": return "White";
                //case "Apple": return "White";
                case "Group": return "#FF007CF9";
                default: return "Black";
            }
        }

        private string DeterminePrimaryForegroundColor(string title)
        {
            switch (title)
            {
                //case "Microsoft": return "Black";
                //case "Apple": return "Black";
                case "Group": return "White";
                default: return "White";
            }
        }

        private void HideOneBox()
        {

            foreach (var tab in this.Tabs)
            {
                tab.HasFocus = false;
                tab.ExternalRaisePropertyChanged("HasFocus");

                tab.Foreground = "Black";
                tab.RightBorderColor = "#FFB8B8B8";
                tab.ExternalRaisePropertyChanged("Foreground");
                tab.ExternalRaisePropertyChanged("RightBorderColor");
            }

            this.SelectedTab = null;
            this.NotifyPropertyChanged("SelectedTab");

            Messenger.Default.Send(new HideOnebox());
        }
    }
}
