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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xbox.Music.Platform.BackendClients.ADM;

namespace Microsoft.Xbox.Music.Platform.Client
{
    /// <summary>
    /// Basic Azure Data Market (http://datamarket.azure.com/) authentication client
    /// </summary>
    public class AzureDataMarketAuthenticationClient : SimpleServiceClient
    {
        private readonly Uri hostname = new Uri("https://datamarket.accesscontrol.windows.net");

        /// <summary>
        /// Authenticate an application on Azure Data Market
        /// </summary>
        /// <param name="clientId">The application's client ID</param>
        /// <param name="clientSecret">The application's secret</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<AzureDataMarketAuthenticationResponse> AuthenticateAsync(string clientId, string clientSecret, CancellationToken cancellationToken)
        {
            Dictionary<string, string> request = new Dictionary<string, string>()
            {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"scope", "http://music.xboxlive.com/"},
                {"grant_type", "client_credentials"}
            };
            return PostAsync<AzureDataMarketAuthenticationResponse, Dictionary<string, string>>(hostname, "/v2/OAuth2-13", request, cancellationToken);
        }

        protected override HttpContent CreateHttpContent<TRequest>(TRequest requestPayload, StreamWriter writer, MemoryStream stream)
        {
            // We need the url-encoded data for Azure authentication
            return new FormUrlEncodedContent(requestPayload as Dictionary<string, string>);
        }
    }
}
