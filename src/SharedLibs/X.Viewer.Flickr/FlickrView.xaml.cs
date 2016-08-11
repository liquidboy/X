using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Viewer.Flickr
{
    public sealed partial class FlickrView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object _photo;
        public object Photo { get { return _photo; } set { _photo = value; RaisePropertyChanged("Photo");  } }

        private object _user;
        public object User { get { return _user; } set { _user = value; RaisePropertyChanged("User"); } }

        public FlickrView()
        {
            this.InitializeComponent();
        }

        private void RaisePropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
