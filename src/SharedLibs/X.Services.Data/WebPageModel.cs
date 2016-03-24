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
    public class WebPageModel : ViewModelBase, IWebPageModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Index1 { get; set; }

        public bool HasFocus { get; set; }
        public string DisplayTitle { get; set; }
        public string FaviconUri { get; set; }
        public bool ShowPadlock { get; set; }

        [IgnoreAttribute]
        public string UriQueryString { get; set; }
        [IgnoreAttribute]
        public string UriPart1 { get; set; }
        [IgnoreAttribute]
        public string UriPart2 { get; set; }
        [IgnoreAttribute]
        public string UriPart3 { get; set; }
        [IgnoreAttribute]
        public string UriPart4 { get; set; }
        [IgnoreAttribute]
        public string PrimaryFontFamily { get; set; }

        [IgnoreAttribute]
        public string PrimaryBackgroundColor { get; set; }


        [IgnoreAttribute]
        public string PrimaryForegroundColor { get; set; }

        [IgnoreAttribute]
        public ObservableCollection<NameValue> QueryNames { get; set; }

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

                var temp = _uri.Split(".".ToCharArray());

                Uri tempUri = new Uri(Uri);

                this.UriQueryString = tempUri.Query;
                var parts = this.UriQueryString.Split("&".ToCharArray());
                this.QueryNames = new ObservableCollection<NameValue>();
                foreach (var qnp in parts)
                {
                    if (qnp.Length > 0)
                    {
                        var qnpParts = qnp.Split("=".ToCharArray());
                        var qn = qnpParts[0];
                        this.QueryNames.Add(new NameValue() { Name = qn.Replace("?", ""), Value = qnpParts[1] });
                    }
                }


                this.UriPart2 = tempUri.Host;
                this.UriPart3 = tempUri.Authority;
                this.UriPart4 = tempUri.Port.ToString();
                this.UriPart1 = tempUri.Scheme;
                RaisePropertyChanged("UriPart1");


                this.ShowPadlock = tempUri.Scheme == "https" ? true : false;
                RaisePropertyChanged("ShowPadlock");

                //hrm
                string ua = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X)" + "AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25";
                Windows.Web.Http.HttpRequestMessage hrm = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, tempUri);
                hrm.Headers.Add("User-Agent", ua);
                //webView1.NavigateWithHttpRequestMessage(hrm);

                if (!string.IsNullOrEmpty(Uri) && string.IsNullOrEmpty(OriginalUri)) OriginalUri = Uri;
            }

        }

        [IgnoreAttribute]
        public string OriginalUri { get; set; }

        public void ExternalRaisePropertyChanged(string propName) { RaisePropertyChanged(propName); }


    }
}
