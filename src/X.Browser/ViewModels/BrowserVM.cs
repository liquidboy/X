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
    class BrowserVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TabViewModel> Tabs { get; set; }




        public TabViewModel SelectedTab { get; set; }
        public bool IsShowingAddTab { get; set; }
        public bool IsShowingMoreTab { get; set; }

        private Color _Accent1;
        private Brush _Accent1Brush;
        private Color _Accent1Contrast;
        private Brush _Accent1ContrastBrush;
        private Color _Accent2;
        private Brush _Accent2Brush;
        private Color _Accent3;
        private Brush _Accent3Brush;
        private Color _Accent4;
        private Brush _Accent4Brush;

        public Color Accent3 { get { return _Accent3; } set { _Accent3 = value; NotifyPropertyChanged(); } }
        public Brush Accent3Brush { get { return _Accent3Brush; } set { _Accent3Brush = value; NotifyPropertyChanged(); } }
        public Color Accent2 { get { return _Accent2; } set { _Accent2 = value; NotifyPropertyChanged(); } }
        public Brush Accent2Brush { get { return _Accent2Brush; } set { _Accent2Brush = value; NotifyPropertyChanged(); } }
        public Color Accent1 { get { return _Accent1; } set { _Accent1 = value; NotifyPropertyChanged(); } }
        public Brush Accent1Brush { get { return _Accent1Brush; } set { _Accent1Brush = value; NotifyPropertyChanged(); } }
        public Color Accent1Contrast { get { return _Accent1Contrast; } set { _Accent1Contrast = value; NotifyPropertyChanged(); } }
        public Brush Accent1ContrastBrush { get { return _Accent1ContrastBrush; } set { _Accent1ContrastBrush = value; NotifyPropertyChanged(); } }
        public Color Accent4 { get { return _Accent4; } set { _Accent4 = value; NotifyPropertyChanged(); } }
        public Brush Accent4Brush { get { return _Accent4Brush; } set { _Accent4Brush = value; NotifyPropertyChanged(); } }


        private RelayCommand<object> _tabChangedCommand;
        private RelayCommand<object> _tabAddCommand;
        private RelayCommand<object> _tabMoreCommand;

        public RelayCommand<object> TabChangedCommand { get { return _tabChangedCommand ?? (_tabChangedCommand = new RelayCommand<object>(ExecuteTabChangedCommand)); } }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ExposedNotifyPropertyChanged(string propertyName)
        {
            this.NotifyPropertyChanged(propertyName);
        }

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

            Messenger.Default.Send(new SetMoreTabSearchBoxFocus());
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

        public BrowserVM()
        {
            Tabs = new ObservableCollection<TabViewModel>();
            LoadTheme();
            LoadTabs();
        }

        private void LoadTabs() {

            var data = App.StorageSvc.Storage.RetrieveList<WebPageDataModel>();
            if (data.Count == 0)
            {
                LoadDefaultTabs();
                DeleteTabs();
                SaveTabs(Tabs);
            }
            else {
                bool hasSetFirstItem = false;
                foreach (var d in data)
                {
                    var tempTab = new TabViewModel() { DisplayTitle = d.DisplayTitle, FaviconUri = d.FaviconUri, HasFocus = !hasSetFirstItem, Uri = d.Uri, TabChangedCommand = this.TabChangedCommand };
                    tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
                    tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
                    tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
                    if (!hasSetFirstItem) tempTab.Foreground = "White";
                    tempTab.RightBorderColor = hasSetFirstItem ? "#FFB8B8B8" : "Black";

                    Tabs.Add(tempTab);

                    if (!hasSetFirstItem) this.SelectedTab = tempTab;

                    hasSetFirstItem = true;
                }

            }

        }

        public void SaveTabs(ObservableCollection<TabViewModel> TabsToSave)
        {

            foreach (var t in TabsToSave)
            {
                var wpm = new WebPageDataModel() { DisplayTitle = t.DisplayTitle, Uid = Guid.NewGuid().ToString(), Uri = t.Uri, FaviconUri = t.FaviconUri, HasFocus = t.HasFocus };

                App.StorageSvc.Storage.Insert(wpm);

            }
        }

        public void DeleteTabs() {
            App.StorageSvc.Storage.TruncateAll();
        }


        private void LoadDefaultTabs()
        {

            var tempTab = new TabViewModel() { DisplayTitle = "Microsoft", FaviconUri = "http://www.microsoft.com/favicon.ico?v2", HasFocus = true, Uri = "http://www.microsoft.com?test=1", TabChangedCommand = this.TabChangedCommand };
            tempTab.PrimaryFontFamily = DetermineFontFamily(tempTab.DisplayTitle);
            tempTab.PrimaryBackgroundColor = DeterminePrimaryBackgroundColor(tempTab.DisplayTitle);
            tempTab.PrimaryForegroundColor = DeterminePrimaryForegroundColor(tempTab.DisplayTitle);
            tempTab.Foreground = "White";
            tempTab.RightBorderColor = "Black";
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

        private void LoadTheme() {
            Accent1 = new Color() { R = 204, G = 204, B = 204, A = 255 };
            Accent1Brush = new SolidColorBrush(Accent1);
            Accent1Contrast = Colors.Black;
            Accent1ContrastBrush = new SolidColorBrush(Colors.Black);
            Accent2 = new Color() { R = 133, G = 133, B = 133, A = 255 };
            Accent2Brush = new SolidColorBrush(Accent2);
            Accent3 = Accent1;
            Accent3Brush = new SolidColorBrush(Accent1);
            Accent4 = new Color() { R = 230, G = 230, B = 230, A = 255 };
            Accent4Brush = new SolidColorBrush(Accent4);
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
    }
}
