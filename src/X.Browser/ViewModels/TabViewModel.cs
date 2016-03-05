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

        //public List<TabHistoryItem> History = new List<TabHistoryItem>();

        public string Foreground { get; set; }

        public string RightBorderColor { get; set; }

        public string ThumbUri { get; set; }

        public bool IsTabGroup { get; set; }
        public ObservableCollection<TabViewModel> ChildTabs { get; set; }


        public TabViewModel()
        {
            ChildTabs = new ObservableCollection<TabViewModel>();
        }

        //public void RecordForHistory(Uri uri)
        //{
        //    //if current uri == where we want to go then no change
        //    if (this.Uri == uri.AbsoluteUri) return;

        //    //does it already exist in history
        //    var found = this.History.Where(x => x.Uri == uri.AbsoluteUri);
        //    if (found != null && found.Count() > 0) return;


        //    //doesnt exist in history so add it
        //    var thi = new TabHistoryItem();
        //    thi.Uri = uri.AbsoluteUri;
        //    thi.DisplayTitle = this.DisplayTitle;
        //    thi.FaviconUri = this.FaviconUri;
        //    this.History.Add(thi);
        //}
    }
}
