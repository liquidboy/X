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

namespace Microsoft.Xbox.Music.Platform.Contract.AuthenticationDataModel
{
    public interface IXToken
    {
        /// <summary>
        /// This is what you need to use as value for the HTTP Authorization header when identifying users.
        /// </summary>
        string AuthorizationHeaderValue { get; set; }

        /// <summary>
        /// The token is invalid after this date.
        /// </summary>
        DateTime NotAfter { get; set; }

        /// <summary>
        /// The token was created at this date.
        /// </summary>
        DateTime IssueInstant { get; set; }

        /// <summary>
        /// Refresh the token.
        /// This is intended to be used mainly to react to authentication errors due to token expiry or revocation.
        /// It is recommended to run this proactively only if <see cref="NotAfter"/> &lt; <see cref="DateTime.UtcNow"/> .
        /// It is recommended not to throw exceptions on authentication errors when implementing this method.
        /// </summary>
        /// <returns>True if the token was refreshed succesfully.</returns>
        Task<bool> RefreshAsync(CancellationToken cancellationToken);
    }
}
