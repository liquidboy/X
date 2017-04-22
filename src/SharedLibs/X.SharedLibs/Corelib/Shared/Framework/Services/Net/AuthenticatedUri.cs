using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Services.Networking
{
    public class AuthenticatedUri : Uri
    {
        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public ICredentials Credentials
        {
            get
            {
                if (this.UserName != null)
                {
                    return new NetworkCredential(this.UserName, this.Password);
                }
                return null;
            }
        }
        public AuthenticatedUri(string uriString, string username, string password)
            : base(uriString)
        {
            this.UserName = username;
            this.Password = password;
        }
        public AuthenticatedUri(string uriString, UriKind uriKind, string username, string password)
            : base(uriString, uriKind)
        {
            this.UserName = username;
            this.Password = password;
        }
        public AuthenticatedUri(Uri baseUri, string relativeUri, string username, string password)
            : base(baseUri, relativeUri)
        {
            this.UserName = username;
            this.Password = password;
        }
        public AuthenticatedUri(Uri baseUri, Uri relativeUri, string username, string password)
            : base(baseUri, relativeUri)
        {
            this.UserName = username;
            this.Password = password;
        }
    }
}
