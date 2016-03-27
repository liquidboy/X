using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace X.Browser.ViewModels
{
    public partial class BrowserVM : INotifyPropertyChanged
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
        
     

        public BrowserVM(string defaultTabUid = null, Uri defaultUri = null)
        {
            LoadTheme();
            LoadTabs(defaultTabUid, defaultUri);
        }


        
    }
}
