using System;
using System.Collections.Generic;
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
using X.UI.LiteTab;

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
            var selected = (Tab)sender;
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
    

    }

    public class vmSamples : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    

}
