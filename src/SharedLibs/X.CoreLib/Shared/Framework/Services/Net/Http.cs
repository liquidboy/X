using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SumoNinjaMonkey.Services.Networking
{
    public static class Http
    {
        private static readonly List<HttpResponse> pendingRequests = new List<HttpResponse>();
        public static void AbortAllRequests()
        {
            lock (Http.pendingRequests)
            {
                foreach (HttpResponse current in Http.pendingRequests)
                {
                    current.Abort();
                }
            }
        }
        public static void AbortRequests(Func<HttpResponse, bool> abortPredicate)
        {
            lock (Http.pendingRequests)
            {
                foreach (HttpResponse current in Http.pendingRequests.Where(abortPredicate))
                {
                    current.Abort();
                }
            }
        }
        private static HttpWebRequest CreateRequest(HttpGetRequest req)
        {
            string queryString = Http.GetQueryString(req.Query);
            HttpWebRequest result;
            if (string.IsNullOrEmpty(queryString))
            {
                result = (HttpWebRequest)WebRequest.Create(req.Uri.AbsoluteUri);
            }
            else
            {
                if (req.Uri.AbsoluteUri.Contains("?"))
                {
                    result = (HttpWebRequest)WebRequest.Create(req.Uri.AbsoluteUri + "&" + queryString);
                }
                else
                {
                    result = (HttpWebRequest)WebRequest.Create(req.Uri.AbsoluteUri + "?" + queryString);
                }
            }
            return result;
        }
        public static Task<HttpResponse> GetAsync(string url)
        {
            TaskCompletionSource<HttpResponse> task = new TaskCompletionSource<HttpResponse>();
            Http.Get(url, delegate(HttpResponse result)
            {
                if (result.Successful)
                {
                    task.SetResult(result);
                    return;
                }
                if (result.Canceled)
                {
                    task.SetCanceled();
                    return;
                }
                task.SetException(result.Exception);
            });
            return task.Task;
        }
        public static Task<HttpResponse> GetAsync(HttpGetRequest request)
        {
            TaskCompletionSource<HttpResponse> task = new TaskCompletionSource<HttpResponse>();
            Http.Get(request, delegate(HttpResponse result)
            {
                if (result.Successful)
                {
                    task.SetResult(result);
                    return;
                }
                if (result.Canceled)
                {
                    task.SetCanceled();
                    return;
                }
                task.SetException(result.Exception);
            });
            return task.Task;
        }
        public static Task<HttpResponse> PostAsync(string url)
        {
            TaskCompletionSource<HttpResponse> task = new TaskCompletionSource<HttpResponse>();
            Http.Post(url, delegate(HttpResponse result)
            {
                if (result.Successful)
                {
                    task.SetResult(result);
                    return;
                }
                if (result.Canceled)
                {
                    task.SetCanceled();
                    return;
                }
                task.SetException(result.Exception);
            });
            return task.Task;
        }
        public static Task<HttpResponse> PostAsync(HttpPostRequest request)
        {
            TaskCompletionSource<HttpResponse> task = new TaskCompletionSource<HttpResponse>();
            Http.Post(request, delegate(HttpResponse result)
            {
                if (result.Successful)
                {
                    task.SetResult(result);
                    return;
                }
                if (result.Canceled)
                {
                    task.SetCanceled();
                    return;
                }
                task.SetException(result.Exception);
            });
            return task.Task;
        }
        public static HttpResponse Get(string uri, Action<HttpResponse> action)
        {
            return Http.Get(new HttpGetRequest(uri), action);
        }
        public static HttpResponse Get(HttpGetRequest req, Action<HttpResponse> action)
        {
            HttpResponse response = new HttpResponse(req);
            try
            {
                if (!req.UseCache)
                {
                    req.Query["__dcachetime"] = DateTime.Now.Ticks.ToString();
                }
                HttpWebRequest request = Http.CreateRequest(req);
                if (req.Credentials != null)
                {
                    request.Credentials = req.Credentials;
                }
                response.WebRequest = request;
                if (req.Cookies.Count > 0)
                {
                    request.CookieContainer = new CookieContainer();
                    foreach (Cookie current in req.Cookies)
                    {
                        request.CookieContainer.Add(request.RequestUri, current);
                    }
                }
                request.Method = "GET";
                if (req.ContentType != null)
                {
                    request.ContentType = req.ContentType;
                }
                if (req.Header.Count > 0)
                {
                    foreach (KeyValuePair<string, string> current2 in req.Header)
                    {
                        request.Headers[current2.Key] = current2.Value;
                    }
                }
                response.CreateTimeoutTimer(request);
                request.BeginGetResponse(delegate(IAsyncResult r)
                {
                    Http.ProcessResponse(r, request, response, action);
                }, request);
            }
            catch (Exception exception)
            {
                response.Exception = exception;
                if (action != null)
                {
                    action(response);
                }
            }
            lock (Http.pendingRequests)
            {
                Http.pendingRequests.Add(response);
            }
            return response;
        }
        public static HttpResponse Post(string uri, Action<HttpResponse> action)
        {
            return Http.Post(new HttpPostRequest(uri), action);
        }
        public static HttpResponse Post(HttpPostRequest req, Action<HttpResponse> action)
        {
            HttpResponse response = new HttpResponse(req);
            try
            {
                string boundary = "";
                HttpWebRequest request = Http.CreateRequest(req);
                if (req.Credentials != null)
                {
                    request.Credentials = req.Credentials;
                }
                response.WebRequest = request;
                if (req.Files.Count == 0)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;
                }
                if (req.Cookies.Count > 0)
                {
                    request.CookieContainer = new CookieContainer();
                    foreach (Cookie current in req.Cookies)
                    {
                        request.CookieContainer.Add(request.RequestUri, current);
                    }
                }
                request.Method = "POST";
                if (req.ContentType != null)
                {
                    request.ContentType = req.ContentType;
                }
                if (req.Header.Count > 0)
                {
                    foreach (KeyValuePair<string, string> current2 in req.Header)
                    {
                        request.Headers[current2.Key] = current2.Value;
                    }
                }
                response.CreateTimeoutTimer(request);
                request.BeginGetRequestStream(delegate(IAsyncResult ar1)
                {
                    try
                    {
                        using (Stream stream = request.EndGetRequestStream(ar1))
                        {
                            if (req.Files.Count > 0)
                            {
                                Http.WritePostData(stream, boundary, req);
                            }
                            else
                            {
                                Http.WritePostData(stream, req);
                            }
                        }
                        request.BeginGetResponse(delegate(IAsyncResult r)
                        {
                            Http.ProcessResponse(r, request, response, action);
                        }, request);
                    }
                    catch (Exception exception2)
                    {
                        response.Exception = exception2;
                        if (action != null)
                        {
                            action(response);
                        }
                    }
                }, request);
            }
            catch (Exception exception)
            {
                response.Exception = exception;
                if (action != null)
                {
                    action(response);
                }
            }
            lock (Http.pendingRequests)
            {
                Http.pendingRequests.Add(response);
            }
            return response;
        }
        private static void WritePostData(Stream stream, HttpPostRequest request)
        {
            byte[] array = request.RawData ?? request.Encoding.GetBytes(Http.GetQueryString(request.Data));
            stream.Write(array, 0, array.Length);
        }
        private static void WritePostData(Stream stream, string boundary, HttpPostRequest request)
        {
            byte[] bytes = request.Encoding.GetBytes("\r\n--" + boundary + "\r\n");
            string format = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
            if (request.RawData != null)
            {
                throw new Exception("RawData not allowed if uploading files");
            }
            foreach (KeyValuePair<string, string> current in request.Data)
            {
                stream.Write(bytes, 0, bytes.Length);
                string s = string.Format(format, new object[]
				{
					current.Key,
					current.Value
				});
                byte[] bytes2 = request.Encoding.GetBytes(s);
                stream.Write(bytes2, 0, bytes2.Length);
            }
            foreach (HttpPostFile current2 in request.Files)
            {
                stream.Write(bytes, 0, bytes.Length);
                string s2 = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", new object[]
				{
					current2.Name,
					current2.Filename,
					current2.ContentType ?? "application/octet-stream"
				});
                byte[] bytes3 = request.Encoding.GetBytes(s2);
                stream.Write(bytes3, 0, bytes3.Length);
                Stream stream2 = current2.Stream;
                if (stream2 == null)
                {
                    IAsyncOperation<StorageFile> fileFromPathAsync = StorageFile.GetFileFromPathAsync(current2.Path);
                    Task<StorageFile> task = fileFromPathAsync.AsTask<StorageFile>();
                    task.RunSynchronously();
                    IAsyncOperation<IRandomAccessStreamWithContentType> asyncOperation = fileFromPathAsync.GetResults().OpenReadAsync();
                    Task<IRandomAccessStreamWithContentType> task2 = asyncOperation.AsTask<IRandomAccessStreamWithContentType>();
                    task2.RunSynchronously();
                    stream2 = asyncOperation.GetResults().AsStreamForRead();
                }
                try
                {
                    byte[] array = new byte[1024];
                    int count;
                    while ((count = stream2.Read(array, 0, array.Length)) != 0)
                    {
                        stream.Write(array, 0, count);
                    }
                }
                finally
                {
                    if (current2.CloseStream)
                    {
                        stream2.Dispose();
                    }
                }
            }
            bytes = request.Encoding.GetBytes("\r\n--" + boundary + "--\r\n");
            stream.Write(bytes, 0, bytes.Length);
        }
        private static void ProcessResponse(IAsyncResult asyncResult, WebRequest request, HttpResponse resp, Action<HttpResponse> action)
        {
            lock (Http.pendingRequests)
            {
                if (Http.pendingRequests.Contains(resp))
                {
                    Http.pendingRequests.Remove(resp);
                }
            }
            try
            {
                WebResponse webResponse = request.EndGetResponse(asyncResult);
                resp.IsConnected = true;
                WebResponse webResponse2 = webResponse;
                using (webResponse)
                {
                    if (resp.Request.ResponseAsStream)
                    {
                        resp.ResponseStream = webResponse.GetResponseStream();
                    }
                    else
                    {
                        resp.RawResponse = webResponse.GetResponseStream().ReadToEnd();
                    }
                    if (webResponse2.Headers.AllKeys.Contains("Set-Cookie"))
                    {
                        string text = webResponse2.Headers["Set-Cookie"];
                        int num = text.IndexOf(';');
                        if (num != -1)
                        {
                            foreach (KeyValuePair<string, string> current in HttpUtilityExtensions.ParseQueryString(text.Substring(0, num)))
                            {
                                resp.Cookies.Add(new Cookie(current.Key, current.Value));
                            }
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                WebException ex = ex2 as WebException;
                if (ex != null)
                {
                    HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        resp.HttpStatusCode = httpWebResponse.StatusCode;
                    }
                }
                if (resp.ResponseStream != null)
                {
                    resp.ResponseStream.Dispose();
                    resp.ResponseStream = null;
                }
                resp.Exception = ex2;
                if (action != null)
                {
                    action(resp);
                }
                return;
            }
            if (action != null)
            {
                action(resp);
            }
        }
        private static string GetQueryString(Dictionary<string, string> query)
        {
            string text = "";
            foreach (KeyValuePair<string, string> current in query)
            {
                string text2 = text;
                text = string.Concat(new string[]
				{
					text2,
					Uri.EscapeDataString(current.Key),
					"=",
					(current.Value == null) ? "" : Uri.EscapeDataString(current.Value),
					"&"
				});
            }
            return text.Trim("&".ToCharArray()); 
        }
    }
}
