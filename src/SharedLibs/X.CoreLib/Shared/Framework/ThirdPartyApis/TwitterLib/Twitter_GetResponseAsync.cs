using System;
using System.Net;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Twitter
    {


        private async Task<FlickrResult<T>> GetResponseAsync<T>(Dictionary<string, string> parameters, string method) where T : IFlickrParsable, new()
        {
            CheckApiKey();

            parameters["api_key"] = ApiKey;

            if (!String.IsNullOrEmpty(OAuthAccessToken) || String.IsNullOrEmpty(AuthToken))
            {
                OAuthGetBasicParameters(parameters);
                parameters.Remove("api_key");
                if (!String.IsNullOrEmpty(OAuthAccessToken)) parameters["oauth_token"] = OAuthAccessToken;
            }
            else
            {
                parameters["auth_token"] = AuthToken;
            }

            Uri url;
            //if (!String.IsNullOrEmpty(sharedSecret))
            //    url = CalculateUri(parameters, true);
            //else
            url = CalculateUri(parameters, false);

            lastRequest = url.AbsoluteUri;



            FlickrResult<string> r = await TwitterResponder.GetDataResponseAsync(this, method, url.AbsoluteUri, parameters);

            FlickrResult<T> result = new FlickrResult<T>();

            if (!r.HasError)
            {
                lastResponse = r.Result;
                using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Result)))
                {
                    try
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                        T t = (T)serializer.ReadObject(ms);
                        result.Result = t;
                        result.HasError = false;
                    }
                    catch (Exception ex)
                    {
                        result.HasError = true;
                        result.Error = ex;
                    }
                }
            }
            else
            {
                result.HasError = true;
                result.ErrorCode = r.ErrorCode;
                result.ErrorMessage = r.ErrorMessage;
                result.Error = r.Error;
            }


            return result;
        }

        private void DoGetResponseAsync<T>(Uri url, Action<FlickrResult<T>> callback) where T : IFlickrParsable, new()
        {
            string postContents = String.Empty;

            if (url.AbsoluteUri.Length > 2000)
            {
                postContents = url.Query.Substring(1);
                url = new Uri(url, String.Empty);
            }

            FlickrResult<T> result = new FlickrResult<T>();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.BeginGetRequestStream(requestAsyncResult =>
            {
                using (Stream s = request.EndGetRequestStream(requestAsyncResult))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(postContents);

                    }

                }

                request.BeginGetResponse(responseAsyncResult =>
                {
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(responseAsyncResult);
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = sr.ReadToEnd();

                            lastResponse = responseXml;

                            XmlReaderSettings settings = new XmlReaderSettings();
                            settings.IgnoreWhitespace = true;

                            XmlReader reader = XmlReader.Create(new StringReader(responseXml), settings);

                            if (!reader.ReadToDescendant("rsp"))
                            {
                                throw new XmlException("Unable to find response element 'rsp' in Flickr response");
                            }
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.LocalName == "stat" && reader.Value == "fail")
                                    throw ExceptionHandler.CreateResponseException(reader);
                                continue;
                            }

                            reader.MoveToElement();
                            reader.Read();

                            T t = new T();
                            ((IFlickrParsable)t).Load(reader);
                            result.Result = t;
                            result.HasError = false;

                        }

                        if (null != callback) callback(result);

                    }
                    catch (Exception ex)
                    {
                        result.Error = ex;
                        if (null != callback) callback(result);
                        return;
                    }
                }, null);

            }, null);

        }
    }
}

