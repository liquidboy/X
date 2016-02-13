using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

            this.DataContext = vm;

            tlMain.AddTab("Samples 1", true);
            tlMain.AddTab("Samples 2");
            tlMain.AddTab("Samples 3");


            SetTheme("Orange");

        }


        public void SetTheme(string theme) {

            if (theme == "Purple") {
                vm.Accent1 = Colors.Purple;
                vm.Accent1Brush = new SolidColorBrush(Colors.Purple);
                vm.Accent1Contrast = Colors.White;
                vm.Accent1ContrastBrush = new SolidColorBrush(Colors.White);
                vm.Accent2 = Colors.MediumPurple;
                vm.Accent2Brush = new SolidColorBrush(Colors.MediumPurple);
            }
            else if (theme == "Orange")
            {
                vm.Accent1 = Colors.Orange;
                vm.Accent1Brush = new SolidColorBrush(Colors.Orange);
                vm.Accent1Contrast = Colors.White;
                vm.Accent1ContrastBrush = new SolidColorBrush(Colors.White);
                vm.Accent2 = Colors.DarkOrange;
                vm.Accent2Brush = new SolidColorBrush(Colors.DarkOrange);
            }
        }



        private void tlMain_OnTabChanged(object sender, EventArgs e)
        {
            ctl1.Visibility = Visibility.Collapsed;
            ctl2.Visibility = Visibility.Collapsed;
            ctl3.Visibility = Visibility.Collapsed;

            var selected = (Tab)sender;
            if (selected.Name == "Samples 1") { ctl1.Visibility = Visibility.Visible; }
            else if (selected.Name == "Samples 2") { ctl2.Visibility = Visibility.Visible; ctl2.LoadSample(); }
            else if (selected.Name == "Samples 3") { ctl3.Visibility = Visibility.Visible; ctl3.LoadSample(); }
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
