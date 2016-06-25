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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xbox.Music.Platform.BackendClients.ADM;

namespace Microsoft.Xbox.Music.Platform.Client
{
    /// <summary>
    /// Basic Azure Data Market (http://datamarket.azure.com/) authentication cache
    /// </summary>
    public class AzureDataMarketAuthenticationCache : IDisposable
    {
        public class AccessToken
        {
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
        }

        private readonly string clientId;
        private readonly string clientSecret;
        private AccessToken token;

        private readonly AzureDataMarketAuthenticationClient client = new AzureDataMarketAuthenticationClient();

        /// <summary>
        /// Cache an application's authentication token on Azure Data Market
        /// </summary>
        /// <param name="clientId">The application's client ID</param>
        /// <param name="clientSecret">The application's secret</param>
        public AzureDataMarketAuthenticationCache(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        /// <summary>
        /// Get the application's token. Renew it if needed.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AccessToken> CheckAndRenewTokenAsync(CancellationToken cancellationToken)
        {
            if (token == null || token.Expiration < DateTime.UtcNow)
            {
                // This is not thread safe. Unfortunately, portable class library requirements prevent use of
                // asynchronous locking mechanisms. The risk here is authenticating multiple times in parallel
                // which is bad from a performance standpoint but is transparent from a functional standpoint.
                AzureDataMarketAuthenticationResponse authenticationResponse =
                    await client.AuthenticateAsync(clientId, clientSecret, cancellationToken);
                if (authenticationResponse != null)
                {
                    token = new AccessToken
                    {
                        Token = authenticationResponse.AccessToken,
                        Expiration = DateTime.UtcNow.AddSeconds(authenticationResponse.ExpiresIn)
                    };
                }
            }

            return token;
        }
    }
}
