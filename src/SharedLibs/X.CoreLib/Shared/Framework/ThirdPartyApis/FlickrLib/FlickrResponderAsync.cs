using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlickrNet
{
    public static partial class FlickrResponder
    {
        /// <summary>
        /// Gets a data response for the given base url and parameters, 
        /// either using OAuth or not depending on which parameters were passed in.
        /// </summary>
        /// <param name="flickr">The current instance of the <see cref="Flickr"/> class.</param>
        /// <param name="hashCall"></param>
        /// <param name="baseUrl">The base url to be called.</param>
        /// <param name="parameters">A dictionary of parameters.</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static async Task<FlickrResult<string>> GetDataResponseAsync(Flickr flickr, string hashCall, string baseUrl, Dictionary<string, string> parameters)
        {
            bool oAuth = parameters.ContainsKey("oauth_consumer_key");

            if (oAuth)
                return await GetDataResponseOAuthAsync(flickr, hashCall, baseUrl, parameters);
            else
                return await GetDataResponseNormalAsync(flickr, hashCall, baseUrl, parameters);
        }

        private static async Task<FlickrResult<string>> GetDataResponseNormalAsync(Flickr flickr, string hashCall, string baseUrl, Dictionary<string, string> parameters) 
        {
            string method = flickr.CurrentService == SupportedService.Zooomr ? "GET" : "POST";

            string data = String.Empty;

            foreach (var k in parameters)
            {
                data += k.Key + "=" + Uri.EscapeDataString(k.Value) + "&";
            }

            if (method == "GET" && data.Length > 2000) method = "POST";

            if (method == "GET")
                return await DownloadDataAsync(method,hashCall, baseUrl + "?" + data, null, null, null);
            else
                return await DownloadDataAsync(method,hashCall, baseUrl, data, PostContentType, null);
        }

        private static async Task<FlickrResult<string>> GetDataResponseOAuthAsync(Flickr flickr, string hashCall, string baseUrl, Dictionary<string, string> parameters)
        {
            string method = "POST";

            // Remove api key if it exists.
            if (parameters.ContainsKey("api_key")) parameters.Remove("api_key");
            if (parameters.ContainsKey("api_sig")) parameters.Remove("api_sig");

            // If OAuth Access Token is set then add token and generate signature.
            if (!String.IsNullOrEmpty(flickr.OAuthAccessToken) && !parameters.ContainsKey("oauth_token"))
            {
                parameters.Add("oauth_token", flickr.OAuthAccessToken);
            }
            if (!String.IsNullOrEmpty(flickr.OAuthAccessTokenSecret) && !parameters.ContainsKey("oauth_signature"))
            {
                string sig = flickr.OAuthCalculateSignature(method, baseUrl, parameters, flickr.OAuthAccessTokenSecret);
                parameters.Add("oauth_signature", sig);
            }

            // Calculate post data, content header and auth header
            string data = OAuthCalculatePostData(parameters);
            string authHeader = OAuthCalculateAuthHeader(parameters);

            // Download data.
            try
            {
                return  await DownloadDataAsync(method, hashCall, baseUrl, data, PostContentType, authHeader);
            }
            catch (WebException ex)
            {
                //if (ex.Status != WebExceptionStatus.ProtocolError) throw;
                Debug.WriteLine("ERR - [" + baseUrl + "] " + ex.Message);
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response == null) throw;

                if (response.StatusCode != HttpStatusCode.BadRequest && response.StatusCode != HttpStatusCode.Unauthorized) throw;

                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = responseReader.ReadToEnd();
                    

                    throw new OAuthException(responseData, ex);
                }
            }

            return null;
        }

        private async static Task<FlickrResult<string>> DownloadDataAsync(string method,string hashCall, string baseUrl, string data, string contentType, string authHeader)
        {
            Debug.WriteLine("DownloadDataAsync [START] - " + baseUrl + " ====== ");
            Debug.WriteLine(data);


            FlickrResult<string> result = new FlickrResult<string>();
            result.HashCall = hashCall;


            HttpWebRequest client = (HttpWebRequest)HttpWebRequest.CreateHttp(baseUrl);
            //HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(baseUrl);


            client.Headers["HTTP_USER_AGENT"] = Flickr.UserAgent;
            if (!String.IsNullOrEmpty(contentType)) client.ContentType = contentType;//client.Headers["Content-Type"] = contentType;
            if (!String.IsNullOrEmpty(authHeader)) client.Headers["Authorization"] = authHeader;
            client.Method = method;
            //client.ContentType = contentType;
            
            if (method == "POST")
            {
                #region old1
                //IAsyncResult requestResult = client.BeginGetRequestStream(requestAsyncResult =>
                //{

                //    using (Stream s = client.EndGetRequestStream(requestAsyncResult))
                //    {
                //        using (StreamWriter sw = new StreamWriter(s))
                //        {
                //            sw.Write(data);
                //        }
                //    }



                //    IAsyncResult responseResult = client.BeginGetResponse(responseAsyncResult =>
                //    {
                //        Debug.WriteLine("DownloadDataAsync [FINISH] - " + baseUrl);
                //        Debug.WriteLine(data);

                //        FlickrResult<string> result = new FlickrResult<string>();
                //        result.HashCall = hashCall;
                //        try
                //        {
                //            using(HttpWebResponse response = (HttpWebResponse)client.EndGetResponse(responseAsyncResult))
                //            {
                //                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                //                {
                //                    string responseXml = sr.ReadToEnd();

                //                    result.Result = responseXml;
                //                    callback(result);
                //                }
                //            }

                //        }
                //        catch (Exception ex)
                //        {

                //            result.Error = ex;
                //            callback(result);
                //            return;
                //        }
                //    }, null);


                //},null);
                #endregion

                var requestStream = await client.GetRequestStreamAsync();
                using (StreamWriter sw = new StreamWriter(requestStream)) sw.Write(data);

                var response = await client.GetResponseAsync();
                using (StreamReader sr = new StreamReader(response.GetResponseStream())) result.Result = sr.ReadToEnd();

            }
            else
            {
                #region old1
                //IAsyncResult responseResult = client.BeginGetResponse(responseAsyncResult =>
                //{
                //    FlickrResult<string> result = new FlickrResult<string>();
                //    result.HashCall = hashCall;
                //    try
                //    {
                //        using (HttpWebResponse response = (HttpWebResponse)client.EndGetResponse(responseAsyncResult))
                //        {
                //            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                //            {
                //                string responseXml = sr.ReadToEnd();

                //                result.Result = responseXml;
                //                callback(result);
                //            }
                //        }

                //    }
                //    catch (Exception ex)
                //    {

                //        result.Error = ex;
                //        callback(result);
                //        return;
                //    }
                //}, null);
                #endregion

                var response = await client.GetResponseAsync();
                using (StreamReader sr = new StreamReader(response.GetResponseStream())) result.Result = sr.ReadToEnd();
                
            }
            
            return result;
        }

    }
}
