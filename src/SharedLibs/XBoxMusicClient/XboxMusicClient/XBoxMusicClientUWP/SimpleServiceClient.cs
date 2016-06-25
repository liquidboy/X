// Copyright (c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Music.Platform.Client
{
    /// <summary>
    /// Simple REST service client
    /// </summary>
    public class SimpleServiceClient : IDisposable
    {
        private static readonly AssemblyName assemblyName = new AssemblyName(typeof(SimpleServiceClient).AssemblyQualifiedName);
        private static readonly ProductInfoHeaderValue userAgent = new ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString());

        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(60);

        private readonly JsonSerializer jsonSerializer = new JsonSerializer();

        private readonly Lazy<HttpClient> httpClient = new Lazy<HttpClient>(
            () => CreateClient(defaultTimeout),
            LazyThreadSafetyMode.PublicationOnly
            );

        public TimeSpan Timeout
        {
            get { return httpClient.Value.Timeout; }
            set { httpClient.Value.Timeout = value; }
        }

        /// <summary>
        /// Authorization token error types
        /// </summary>
        public enum AuthorizationTokenInvalid
        {
            UnknownError,
            /// <summary>
            /// The authorization token has expired or was revoked. Reauthenticate before calling the service again.
            /// </summary>
            Expired,
        }

        public class SimpleServiceResult<TResult, TErrorResult>
        {
            public TResult Result { get; set; }
            public TErrorResult ErrorResult { get; set; }
            public HttpStatusCode HttpStatusCode { get; set; }
            /// <summary>
            /// If the request was forbidden or unauthorized, this value might contain extra information as to what the issue was.
            /// </summary>
            public AuthorizationTokenInvalid? AuthorizationTokenInvalid { get; set; }
        }

        public virtual void Dispose()
        {
            if (httpClient.IsValueCreated)
            {
                httpClient.Value.Dispose();
            }
        }

        /// <summary>
        /// Issue an HTTP GET request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <returns></returns>
        public async Task<SimpleServiceResult<TResult, TErrorResult>> GetAsync<TResult, TErrorResult>(Uri hostname,
            string relativeUri,
            CancellationToken cancellationToken,
            IEnumerable<KeyValuePair<string, string>> requestParameters = null,
            IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = BuildUri(hostname, relativeUri, requestParameters);

            using (HttpRequestMessage httpRequestMessage = CreateHttpRequest(HttpMethod.Get, uri, null, extraHeaders))
            using (HttpResponseMessage httpResponseMessage = await httpClient.Value.SendAsync(httpRequestMessage, cancellationToken))
            {
                return await ParseResponseAsync<TResult, TErrorResult>(httpResponseMessage);
            }
        }

        public async Task<TResult> GetAsync<TResult>(Uri hostname, string relativeUri,
            CancellationToken cancellationToken,
            IEnumerable<KeyValuePair<string, string>> requestParameters = null,
            IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result =
                await GetAsync<TResult, TResult>(hostname, relativeUri, cancellationToken, requestParameters, extraHeaders);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// Issue an HTTP POST request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <typeparam name="TRequest">The request data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="requestPayload"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <returns></returns>
        public async Task<SimpleServiceResult<TResult, TErrorResult>> PostAsync<TResult, TErrorResult, TRequest>(Uri hostname, string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            IEnumerable<KeyValuePair<string, string>> requestParameters = null,
            IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = BuildUri(hostname, relativeUri, requestParameters);

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (HttpContent content = CreateHttpContent(requestPayload, writer, stream))
            using (HttpRequestMessage requestMessage = CreateHttpRequest(HttpMethod.Post, uri, content, extraHeaders))
            using (HttpResponseMessage result = await httpClient.Value.SendAsync(requestMessage, cancellationToken))
            {
                return await ParseResponseAsync<TResult, TErrorResult>(result);
            }
        }

        public async Task<TResult> PostAsync<TResult, TRequest>(
            Uri hostname, string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            IEnumerable<KeyValuePair<string, string>> requestParameters = null,
            IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result =
                await PostAsync<TResult, TResult, TRequest>(hostname, relativeUri, requestPayload, cancellationToken, requestParameters, extraHeaders);
            return result.Result ?? result.ErrorResult;
        }

        private static Uri BuildUri(Uri hostname, string relativeUri, IEnumerable<KeyValuePair<string, string>> requestParameters)
        {
            string relUri = requestParameters == null
                ? relativeUri
                : requestParameters.Aggregate(relativeUri,
                    (current, param) => current + ((current.Contains("?") ? "&" : "?") + param.Key + "=" + param.Value));
            return new Uri(hostname, relUri);
        }

        private static HttpClient CreateClient(TimeSpan timeout, IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        {
            var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip });
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.UserAgent.Add(userAgent);
            client.Timeout = timeout;

            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return client;
        }

        protected virtual HttpContent CreateHttpContent<TRequest>(TRequest requestPayload, StreamWriter writer,
            MemoryStream stream)
        {
            jsonSerializer.Serialize(writer, requestPayload, typeof(TRequest));
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            HttpContent content = new StreamContent(stream);
            content.Headers.Add("Content-Type", "application/json");
            return content;
        }

        private static HttpRequestMessage CreateHttpRequest(HttpMethod method, Uri uri, HttpContent content,
            IEnumerable<KeyValuePair<string, string>> extraHeaders)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, uri);
            if (content != null)
            {
                message.Content = content;
            }

            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }
            return message;
        }

        private async Task<SimpleServiceResult<TResult, TErrorResult>> ParseResponseAsync<TResult, TErrorResult>(HttpResponseMessage message)
            where TResult : class
            where TErrorResult : class
        {
            // Handle auth token expiration or revocation
            AuthorizationTokenInvalid? validity = ParseAuthorizationTokenValidityErrors(message);

            if (message.Content == null)
            {
                return new SimpleServiceResult<TResult, TErrorResult>
                {
                    HttpStatusCode = message.StatusCode,
                    AuthorizationTokenInvalid = validity,
                };
            }

            using (Stream stream = await message.Content.ReadAsStreamAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new SimpleServiceResult<TResult, TErrorResult>
                    {
                        Result =
                            message.IsSuccessStatusCode
                                ? jsonSerializer.Deserialize(reader, typeof (TResult)) as TResult
                                : null,
                        ErrorResult =
                            message.IsSuccessStatusCode
                                ? null
                                : jsonSerializer.Deserialize(reader, typeof (TErrorResult)) as TErrorResult,
                        HttpStatusCode = message.StatusCode,
                        AuthorizationTokenInvalid = validity,
                    };
                }
            }
        }

        private static readonly Regex AuthenticateResponseDetailRegex = new Regex("error='(?<errorMessage>[^']+)'", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static AuthorizationTokenInvalid? ParseAuthorizationTokenValidityErrors(HttpResponseMessage message)
        {
            AuthorizationTokenInvalid? validity = null;
            if (message.StatusCode == HttpStatusCode.Forbidden || message.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (message.Headers.WwwAuthenticate != null)
                {
                    Match errorDetailMatch = AuthenticateResponseDetailRegex.Match(message.Headers.WwwAuthenticate.ToString());
                    if (errorDetailMatch.Success)
                    {
                        string errorDetail = errorDetailMatch.Groups["errorMessage"].Value;
                        switch (errorDetail)
                        {
                            case "token_expired":
                                // This can happen on scheduled token expiry or service side revocation
                                validity = AuthorizationTokenInvalid.Expired;
                                break;
                            default:
                                validity = AuthorizationTokenInvalid.UnknownError;
                                break;
                        }
                    }
                }
            }
            return validity;
        }
    }
}
