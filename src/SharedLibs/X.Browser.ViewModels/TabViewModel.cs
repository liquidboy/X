using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Browser
{
    public class TabViewModel : WebPageModel
    {
        public RelayCommand<object> TabChangedCommand { get; set; }
        
        public string Foreground { get; set; }

        public string RightBorderColor { get; set; }

        public string ThumbUri { get; set; }

        public bool IsTabGroup { get; set; }
        public ObservableCollection<TabViewModel> ChildTabs { get; set; }


        public TabViewModel()
        {
            ChildTabs = new ObservableCollection<TabViewModel>();
        }

       
    }
}
