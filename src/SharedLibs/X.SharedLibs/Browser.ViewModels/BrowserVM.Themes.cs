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
    public partial class BrowserVM
    {


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



        private void LoadTheme()
        {
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

    }
}
