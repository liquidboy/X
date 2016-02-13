using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace X.UI.RichInput
{
    class InputModel : INotifyPropertyChanged
    {

        public object Data { get; set; }


        private Color _FocusColor;
        public Color FocusColor { get { return _FocusColor; } set { _FocusColor = value; NotifyPropertyChanged(); } }


        private Color _FocusHoverColor;
        public Color FocusHoverColor { get { return _FocusHoverColor; } set { _FocusHoverColor = value; NotifyPropertyChanged(); } }


        private Color _FocusForegroundColor;
        public Color FocusForegroundColor { get { return _FocusForegroundColor; } set { _FocusForegroundColor = value; NotifyPropertyChanged(); } }




        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
