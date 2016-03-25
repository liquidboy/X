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
    partial class BrowserVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ExposedNotifyPropertyChanged(string propertyName)
        {
            this.NotifyPropertyChanged(propertyName);
        }
        
     

        public BrowserVM()
        {
            LoadTheme();
            LoadTabs();
        }


        
    }
}
