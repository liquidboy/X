using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Services.Networking
{
    public class HttpGetRequest : IHttpRequest
    {
        public int ConnectionTimeout
        {
            get;
            set;
        }
        public bool ResponseAsStream
        {
            get;
            set;
        }
        public Uri Uri
        {
            get;
            private set;
        }
        public Dictionary<string, string> Query
        {
            get;
            private set;
        }
        public List<Cookie> Cookies
        {
            get;
            private set;
        }
        public Dictionary<string, string> Header
        {
            get;
            private set;
        }
        public bool UseCache
        {
            get;
            set;
        }
        public Encoding Encoding
        {
            get;
            set;
        }
        public string ContentType
        {
            get;
            set;
        }
        public object Tag
        {
            get;
            set;
        }
        public ICredentials Credentials
        {
            get;
            set;
        }
        public HttpGetRequest(string uri)
            : this(new Uri(uri, UriKind.RelativeOrAbsolute))
        {
        }
        public HttpGetRequest(string uri, Dictionary<string, string> query)
            : this(new Uri(uri, UriKind.RelativeOrAbsolute), query)
        {
        }
        public HttpGetRequest(Uri uri)
            : this(uri, new Dictionary<string, string>())
        {
        }
        public HttpGetRequest(Uri uri, Dictionary<string, string> query)
        {
            this.Uri = uri;
            this.Query = (query ?? new Dictionary<string, string>());
            this.Header = new Dictionary<string, string>();
            this.Cookies = new List<Cookie>();
            this.UseCache = true;
            this.Encoding = Encoding.UTF8;
            this.ConnectionTimeout = 0;
            this.ResponseAsStream = false;
        }
    }
}
