using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Services.Networking
{
    public class HttpPostRequest : HttpGetRequest
    {
        public byte[] RawData
        {
            get;
            set;
        }
        public Dictionary<string, string> Data
        {
            get;
            private set;
        }
        public List<HttpPostFile> Files
        {
            get;
            private set;
        }
        public HttpPostRequest(Uri uri)
            : base(uri)
        {
            this.Data = new Dictionary<string, string>();
            this.Files = new List<HttpPostFile>();
        }
        public HttpPostRequest(string uri)
            : this(new Uri(uri, UriKind.RelativeOrAbsolute))
        {
        }
    }
}
