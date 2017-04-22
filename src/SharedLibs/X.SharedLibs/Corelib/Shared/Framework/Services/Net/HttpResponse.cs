using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Services.Networking
{
    public class HttpResponse
    {
        private Exception exception;
        private HttpStatusCode code = HttpStatusCode.OK;
        public IHttpRequest Request
        {
            get;
            internal set;
        }
        internal HttpWebRequest WebRequest
        {
            get;
            set;
        }
        public bool IsConnected
        {
            get;
            internal set;
        }
        public bool IsPending
        {
            get
            {
                return !this.HasException && !this.Canceled && !this.Successful;
            }
        }
        public Exception Exception
        {
            get
            {
                return this.exception;
            }
            set
            {
                if (this.exception != null && this.exception is TimeoutException)
                {
                    return;
                }
                if (value is WebException && ((WebException)value).Status == WebExceptionStatus.RequestCanceled)
                {
                    this.exception = null;
                    this.Canceled = true;
                    return;
                }
                this.exception = value;
                this.Canceled = false;
            }
        }
        public bool Processed
        {
            get
            {
                return this.Canceled || this.Successful || this.HasException;
            }
        }
        public bool Canceled
        {
            get;
            private set;
        }
        public bool Successful
        {
            get
            {
                return !this.Canceled && this.Exception == null && (this.Response != null || this.ResponseStream != null);
            }
        }
        public bool HasException
        {
            get
            {
                return this.Exception != null;
            }
        }
        public string Response
        {
            get
            {
                if (this.RawResponse != null)
                {
                    return this.Request.Encoding.GetString(this.RawResponse, 0, this.RawResponse.Length);
                }
                return null;
            }
        }
        public byte[] RawResponse
        {
            get;
            internal set;
        }
        public Stream ResponseStream
        {
            get;
            internal set;
        }
        public List<Cookie> Cookies
        {
            get;
            private set;
        }
        public HttpStatusCode HttpStatusCode
        {
            get
            {
                return this.code;
            }
            internal set
            {
                this.code = value;
            }
        }
        public HttpResponse(Exception exception)
        {
            this.exception = exception;
        }
        public HttpResponse(IHttpRequest request)
        {
            this.Request = request;
            this.Cookies = new List<Cookie>();
        }
        public void Abort()
        {
            if (this.Request != null && !this.Successful)
            {
                this.WebRequest.Abort();
            }
        }
        internal void CreateTimeoutTimer(HttpWebRequest request)
        {
            if (this.Request.ConnectionTimeout > 0)
            {
                request.ContinueTimeout = this.Request.ConnectionTimeout;
            }
        }
    }
}
