using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
//using System.Net.Http;  deprecated in store apps  https://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh781239.aspx
using Windows.Web.Http;

namespace FlickrNet
{
    public static partial class TwitterResponder
    {
        /// <summary>
        /// Gets a data response for the given base url and parameters, 
        /// either using OAuth or not depending on which parameters were passed in.
        /// </summary>
        /// <param name="flickr">The current instance of the <see cref="Flickr"/> class.</param>
        /// <param name="baseUrl">The base url to be called.</param>
        /// <param name="parameters">A dictionary of parameters.</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async static Task<FlickrResult<string>> GetDataResponseAsync(Twitter flickr, string method, string baseUrl, Dictionary<string, string> parameters)
        {
            bool oAuth = parameters.ContainsKey("oauth_consumer_key");

            if (oAuth)
                return await GetDataResponseOAuthAsync(flickr, method, baseUrl, parameters);
            else
                return await GetDataResponseNormalAsync(flickr, baseUrl, parameters);
        }

        private async static Task<FlickrResult<string>> GetDataResponseNormalAsync(Twitter flickr, string baseUrl, Dictionary<string, string> parameters)
        {
            string method = flickr.CurrentService == SupportedService.Zooomr ? "GET" : "POST";

            string data = String.Empty;

            foreach (var k in parameters)
            {
                data += k.Key + "=" + Uri.EscapeDataString(k.Value) + "&";
            }

            if (method == "GET" && data.Length > 2000) method = "POST";

            if (method == "GET")
                return await DownloadDataAsync(method, baseUrl + "?" + data, null, null, null);
            else
                return await DownloadDataAsync(method, baseUrl, data, PostContentType, null);
        }

        private async static Task<FlickrResult<string>> GetDataResponseOAuthAsync(Twitter flickr, string method, string baseUrl, Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("api_key")) parameters.Remove("api_key");

            if (parameters.ContainsKey("api_sig")) parameters.Remove("api_sig");

            if (!String.IsNullOrEmpty(flickr.OAuthAccessToken) && !parameters.ContainsKey("oauth_token"))
            {
                parameters.Add("oauth_token", flickr.OAuthAccessToken);
            }
            if (!String.IsNullOrEmpty(flickr.OAuthAccessTokenSecret) && !parameters.ContainsKey("oauth_signature"))
            {
                string sig = flickr.OAuthCalculateSignatureForCalls(method, baseUrl, parameters, flickr.OAuthAccessTokenSecret);
                parameters.Add("oauth_signature", sig);
            }

            string data = OAuthCalculatePostData(parameters);

            string authHeader = OAuthCalculateAuthHeader(parameters);


            baseUrl = baseUrl.Split("?".ToCharArray())[0];

            if (method == "GET") return await DownloadDataAsync(method, baseUrl, data, GetContentType, authHeader);
            else return await DownloadDataAsync(method, baseUrl, data, PostContentType, authHeader);

        }

        private async static Task<FlickrResult<string>> DownloadDataAsync(string method, string baseUrl, string data, string contentType, string authHeader)
        {
            int retryCounter = 0;

            retry:

            FlickrResult<string> ret = new FlickrResult<string>();

            string useUrl = baseUrl;
            if (method == "GET" && data.Length > 0) useUrl += "?" + data;

            HttpClient client = new HttpClient();
            //todo: what is the equivalent of this call in windows.web.http
            //client.MaxResponseContentBufferSize = 10 * 1024 * 1024;
            


            client.DefaultRequestHeaders.Add("HTTP_USER_AGENT", Flickr.UserAgent);

            //if (!String.IsNullOrEmpty(contentType) && method != "GET") 
            //    client.DefaultRequestHeaders.Add("Content-Type", contentType);

            if (!String.IsNullOrEmpty(authHeader))
                client.DefaultRequestHeaders.Add("Authorization", authHeader);

            try
            {


                HttpResponseMessage httpRM = new HttpResponseMessage();

                if (method == "POST")
                {
                    var content = new HttpStringContent(data);
                    httpRM = await client.PostAsync(new Uri(useUrl), content);
                }
                else if (method == "GET")
                {
                    httpRM = await client.GetAsync(new Uri(useUrl));
                }

                if (httpRM.IsSuccessStatusCode)
                {
                    ret.Result = await httpRM.Content.ReadAsStringAsync();
                    ret.HasError = false;
                }
                else
                {
                    retryCounter++;
                    if (retryCounter == 2)
                    {
                        retryCounter = 0;
                        ret.HasError = true;
                        ret.ErrorCode = (int)httpRM.StatusCode;
                        ret.ErrorMessage = httpRM.StatusCode.ToString();
                    }
                    else
                    {
                        goto retry;
                    }
                }

            }
            catch (Exception ex)
            {
                retryCounter++;
                if (retryCounter == 2)
                {
                    retryCounter = 0;
                    ret.HasError = true;
                    ret.ErrorCode = -999;
                    ret.ErrorMessage = ex.Message;
                    ret.Error = ex;
                }
                else
                {
                    goto retry;
                }
            }

            return ret;

        }

    }
}

