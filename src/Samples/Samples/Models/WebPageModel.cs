using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Web.Http;

namespace Samples.Models
{
    public class WebPageModel : ViewModelBase, IWebPageModel
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Index1 { get; set; }

        public bool HasFocus { get; set; }
        public string DisplayTitle { get; set; }
        public string FaviconUri { get; set; }
        public bool ShowPadlock { get; set; }

        public string UriQueryString { get; set; }

        public string UriPart1 { get; set; }

        public string UriPart2 { get; set; }

        public string UriPart3 { get; set; }

        public string UriPart4 { get; set; }

        public string PrimaryFontFamily { get; set; }

        public string PrimaryBackgroundColor { get; set; }



        public string PrimaryForegroundColor { get; set; }


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
                RaisePropChangeOnUIThread("UriPart1");


                this.ShowPadlock = tempUri.Scheme == "https" ? true : false;
                RaisePropChangeOnUIThread("ShowPadlock");

                //hrm
                string ua = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X)" + "AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25";
                HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Get, tempUri);
                hrm.Headers.Add("User-Agent", ua);
                //webView1.NavigateWithHttpRequestMessage(hrm);

                if (!string.IsNullOrEmpty(Uri) && string.IsNullOrEmpty(OriginalUri)) OriginalUri = Uri;
            }
        }

        public string OriginalUri { get; set; }

        public async void RaisePropChangeOnUIThread(string propName)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => { this.RaisePropertyChanged(propName); });
        }


    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public interface IWebPageModel 
    {


        bool HasFocus { get; set; }
        string DisplayTitle { get; set; }
        string FaviconUri { get; set; }
        bool ShowPadlock { get; set; }


        string Uri { get; set; }

    }
}
