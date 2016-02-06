using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.UI.LiteTab
{
    public class Tab : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        private bool _IsSelected;
        public bool IsSelected {
            get { return _IsSelected; }
            set {
                _IsSelected = value;
                if (PropertyChanged != null)
                    PropertyChanged(null, new PropertyChangedEventArgs( nameof(IsSelected)) );
            }
        }

        
    }
}
