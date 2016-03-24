using GalaSoft.MvvmLight;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class WebPageDataModel : IWebPageDataModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Index1 { get; set; }

        public bool HasFocus { get; set; }
        public string DisplayTitle { get; set; }
        public string FaviconUri { get; set; }
        public bool ShowPadlock { get; set; }
        

        private string _uri;
        public string Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                _uri = value;
            }

        }
        
    }
}
