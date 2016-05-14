using System;
using System.Net;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        const int Const_CachLengthInSeconds = 45;
        public void GetCachedResponse<T>(string cachedResponse, Action<T> callback) where T : IFlickrParsable, new()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(new StringReader(cachedResponse), settings);

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

            if (callback != null) callback(t);
        }


        //private void GetResponseEvent<T>(Dictionary<string, string> parameters, EventHandler<FlickrResultArgs<T>> handler) where T : IFlickrParsable, new()
        //{
        //    GetResponseAsync<T>(
        //        parameters,
        //        r =>
        //        {
        //            handler(this, new FlickrResultArgs<T>(r));
        //        });
        //}

        private void CacheCall(string url, string data)
        {
            X.CoreLib.Shared.Services.AppDatabase.Current.SaveCacheCallResponse(url, data, DateTime.UtcNow);
        }

        private string TryGetCacheCall(string url)
        {
            var found = X.CoreLib.Shared.Services.AppDatabase.Current.RetrieveCacheCallResponse(url);

            if (found != null && found.Count == 1)
            {
                if (Math.Abs(found[0].TimeStamp.Subtract(DateTime.UtcNow).TotalSeconds) > Const_CachLengthInSeconds)
                {
                    return string.Empty;
                }
                else
                {
                    return found[0].Data;
                }
            }
            else
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private async Task<FlickrResult<T>> GetResponseAsync<T>(Dictionary<string, string> parameters) where T : IFlickrParsable, new()
        {
            var hashApiSig = CalculateAuthSignature(parameters);

            CheckApiKey();

            FlickrResult<T> result = new FlickrResult<T>();

            var parseResult = new Func<Uri, string, T>((uri, response) =>
            {

                lastResponse = response;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(new StringReader(response.Trim()), settings);

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

                return t;
            });




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
            if (!String.IsNullOrEmpty(sharedSecret))
                url = CalculateUri(parameters, true);
            else
                url = CalculateUri(parameters, false);

            lastRequest = url.AbsoluteUri;

            //get cached result if its there
            var cachedResult = TryGetCacheCall(hashApiSig);
            if (!string.IsNullOrEmpty(cachedResult))
            {
                result.Result = parseResult(url, cachedResult);
                result.HashCall = hashApiSig;
                result.HasError = false;
                return result;
            }


            try
            {
                var r = await FlickrResponder.GetDataResponseAsync(this, hashApiSig, BaseUri.AbsoluteUri, parameters);
                
                if (r.HasError)
                {
                    result.Error = r.Error;
                }
                else
                {

                    //cache results
                    CacheCall(r.HashCall, r.Result);

                    result.Result = parseResult(url, r.Result);
                    result.HashCall = r.HashCall;
                    result.HasError = false;
                    
                }
                
            }
            catch (Exception ex)
            {
                result.Error = ex;

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
                    catch(Exception ex)
                    {
                        result.Error = ex;
                        if (null != callback) callback(result);
                        return;
                    }
                }, null);

            }, null);

            //WebClient client = new WebClient();
            //client.UploadStringCompleted += delegate(object sender, UploadStringCompletedEventArgs e)
            //{
            //    FlickrResult<T> result = new FlickrResult<T>();

            //    if (e.Error != null)
            //    {
            //        result.Error = e.Error;
            //        callback(result);
            //        return;
            //    }

            //    try
            //    {
            //        string responseXml = e.Result;

            //        lastResponse = responseXml;

            //        XmlReaderSettings settings = new XmlReaderSettings();
            //        settings.IgnoreWhitespace = true;
            //        XmlReader reader = XmlReader.Create(new StringReader(responseXml), settings);

            //        if (!reader.ReadToDescendant("rsp"))
            //        {
            //            throw new XmlException("Unable to find response element 'rsp' in Flickr response");
            //        }
            //        while (reader.MoveToNextAttribute())
            //        {
            //            if (reader.LocalName == "stat" && reader.Value == "fail")
            //                throw ExceptionHandler.CreateResponseException(reader);
            //            continue;
            //        }

            //        reader.MoveToElement();
            //        reader.Read();

            //        T t = new T();
            //        ((IFlickrParsable)t).Load(reader);
            //        result.Result = t;
            //        result.HasError = false;

            //    }
            //    catch (Exception ex)
            //    {
            //        result.HasError = true;
            //        result.Error = ex;
            //    }

            //    if (callback != null)
            //    {
            //        callback(result);
            //    }

            //};

            //client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            //client.UploadStringAsync(url, "POST", postContents);
        }
    }
}
