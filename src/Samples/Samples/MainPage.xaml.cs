using GalaSoft.MvvmLight.Command;
using Samples.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Samples
{
    public sealed partial class MainPage : Page
    {
        vmSamples vm = new vmSamples();

        public MainPage()
        {
            this.InitializeComponent();

            header.InitChrome(App.Current, ApplicationView.GetForCurrentView());

            this.DataContext = vm;

            tlMain.AddTab("Samples 1", true);
            tlMain.AddTab("Samples 2");
            tlMain.AddTab("Samples 3");

            //butEnableDisableChromeResizeFix.DataContext = header;

            SetTheme("Orange");


            SetTabContent("Samples 1");

        }

     

        public void SetTheme(string theme)
        {

            if (theme == "Purple")
            {
                vm.Accent1 = Colors.DarkSlateBlue;
                vm.Accent1Brush = new SolidColorBrush(Colors.DarkSlateBlue);
                vm.Accent1Contrast = Colors.White;
                vm.Accent1ContrastBrush = new SolidColorBrush(Colors.White);
                vm.Accent2 = Colors.BlueViolet;
                vm.Accent2Brush = new SolidColorBrush(Colors.BlueViolet);
                vm.Accent3 = Colors.DarkSlateBlue;
                vm.Accent3Brush = new SolidColorBrush(Colors.DarkSlateBlue);
            }
            else if (theme == "Orange")
            {
                vm.Accent1 = Colors.OrangeRed;
                vm.Accent1Brush = new SolidColorBrush(Colors.OrangeRed);
                vm.Accent1Contrast = Colors.White;
                vm.Accent1ContrastBrush = new SolidColorBrush(Colors.White);
                vm.Accent2 = Colors.DarkOrange;
                vm.Accent2Brush = new SolidColorBrush(Colors.DarkOrange);
                vm.Accent3 = Colors.OrangeRed;
                vm.Accent3Brush = new SolidColorBrush(Colors.OrangeRed);
            }
            else if (theme == "Black")
            {
                vm.Accent1 = Colors.Black;
                vm.Accent1Brush = new SolidColorBrush(Colors.Black);
                vm.Accent1Contrast = Colors.White;
                vm.Accent1ContrastBrush = new SolidColorBrush(Colors.White);
                vm.Accent2 = Colors.Gray;
                vm.Accent2Brush = new SolidColorBrush(Colors.Gray);
                vm.Accent3 = Colors.Black;
                vm.Accent3Brush = new SolidColorBrush(Colors.Black);
            }
            else if (theme == "Blue")
            {
                vm.Accent1 = new Color() { R = 4, G = 69, B = 124, A = 255 };
                vm.Accent1Brush = new SolidColorBrush(vm.Accent1);
                vm.Accent1Contrast = Colors.White;
                vm.Accent1ContrastBrush = new SolidColorBrush(Colors.White);
                vm.Accent2 = new Color() { R = 0, G = 139, B = 255, A = 255 };
                vm.Accent2Brush = new SolidColorBrush(vm.Accent2);
                vm.Accent3 = vm.Accent1;
                vm.Accent3Brush = new SolidColorBrush(vm.Accent1);

             
            }
        }

        private void SetTabContent(string name) {

            ccTabContent.Content = null;
            
            if (name == "Samples 1") {
                var nt = new ucSample01();
                ccTabContent.Content = nt;
            }
            else if (name == "Samples 2")
            {
                
                var nt = new ucSample02();
                ccTabContent.Content = nt;
                nt.LoadSample();
            }
            else if (name == "Samples 3")
            {
                var nt = new ucSample03();
                ccTabContent.Content = nt;
                nt.LoadSample();
            }
        }

        private void tlMain_OnTabChanged(object sender, EventArgs e)
        {
            var selected = (X.UI.LiteTab.Tab)sender;
            SetTabContent(selected.Name);
        }

        private void rcbThemes_ValueChanged(object sender, RoutedEventArgs e)
        {
            var selectedItem = rcbThemes.Items[((ComboBox)sender).SelectedIndex];

            if (selectedItem is ComboBoxItem)
            {
                var theme = (string)((ComboBoxItem)selectedItem).Content;

                SetTheme(theme);


            }
        }

        private void butEnableDisableChromeResizeFix_Click(object sender, RoutedEventArgs e)
        {
            header.EnableResizeFix = !header.EnableResizeFix;
        }

        private void tlMainTabs_TabPointerEntered(object sender, PointerRoutedEventArgs e)
        {

        }

        private void tlMainTabs_TabPointerExited(object sender, PointerRoutedEventArgs e)
        {

        }

        

    }

    public class vmSamples : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }


        private Color _Accent1;
        private Brush _Accent1Brush;
        private Color _Accent1Contrast;
        private Brush _Accent1ContrastBrush;
        private Color _Accent2;
        private Brush _Accent2Brush;
        private Color _Accent3;
        private Brush _Accent3Brush;

        public Color Accent3 { get { return _Accent3; } set { _Accent3 = value; NotifyPropertyChanged(); } }
        public Brush Accent3Brush { get { return _Accent3Brush; } set { _Accent3Brush = value; NotifyPropertyChanged(); } }
        public Color Accent2 { get { return _Accent2; } set { _Accent2 = value; NotifyPropertyChanged(); } }
        public Brush Accent2Brush { get { return _Accent2Brush; } set { _Accent2Brush = value; NotifyPropertyChanged(); } }
        public Color Accent1 { get { return _Accent1; } set { _Accent1 = value; NotifyPropertyChanged(); } }
        public Brush Accent1Brush { get { return _Accent1Brush; } set { _Accent1Brush = value; NotifyPropertyChanged(); } }
        public Color Accent1Contrast { get { return _Accent1Contrast; } set { _Accent1Contrast = value; NotifyPropertyChanged(); } }
        public Brush Accent1ContrastBrush { get { return _Accent1ContrastBrush; } set { _Accent1ContrastBrush = value; NotifyPropertyChanged(); } }

        private RelayCommand<object> _tabChangedCommand;
        public RelayCommand<object> TabChangedCommand { get { return _tabChangedCommand ?? (_tabChangedCommand = new RelayCommand<object>(ExecuteTabChangedCommand)); } }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteTabChangedCommand(object obj)
        {

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
                        previousTab.RaisePropChangeOnUIThread("RightBorderColor");
                    }
                    tab.RightBorderColor = "black";
                }
                else
                {
                    tab.HasFocus = false;
                    tab.Foreground = "Black";
                    tab.RightBorderColor = "#FFB8B8B8";
                }
                tab.RaisePropChangeOnUIThread("HasFocus");
                tab.RaisePropChangeOnUIThread("Foreground");
                tab.RaisePropChangeOnUIThread("RightBorderColor");

                previousTab = tab;
            }

            this.SelectedTab = obj as TabViewModel;
            this.NotifyPropertyChanged("SelectedTab");

        }

        public vmSamples()
        {
            Tabs = new ObservableCollection<TabViewModel>();
            RetrieveDummyTabs();
        }


        private void RetrieveDummyTabs()
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
