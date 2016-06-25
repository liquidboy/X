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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xbox.Music.Platform.Contract.DataModel;

namespace Microsoft.Xbox.Music.Platform.Client
{
    public static class XboxMusicClientExtensions
    {
        /// <summary>
        /// Lookup an item and get details about it.
        /// </summary>
        /// <param name="client">An IXboxMusicClient instance.</param>
        /// <param name="itemId">Id to look up, prefixed by a namespace: {namespace.id}.</param>
        /// <param name="source">The content source: Catalog, Collection or both</param>
        /// <param name="language">ISO 2 letter code.</param>
        /// <param name="country">ISO 2 letter code.</param>
        /// <param name="extras">Enumeration of extra details.</param>
        /// <returns> Content response with details about one or more items.</returns>
        public static Task<ContentResponse> LookupAsync(this IXboxMusicClient client, string itemId, ContentSource? source = null, string language = null,
            string country = null, ExtraDetails extras = ExtraDetails.None)
        {
            return client.LookupAsync(new List<string> { itemId }, source, language, country, extras);
        }

        /// <summary>
        /// Request the continuation of an incomplete list of content from the service. The relative URL (i.e. the ids list) must be the same as in the original request.
        /// </summary>
        /// <param name="client">An IXboxMusicClient instance.</param>
        /// <param name="itemId">Id to look up, prefixed by a namespace: {namespace.id}.</param>
        /// <param name="continuationToken">A Continuation Token provided in an earlier service response.</param>
        /// <returns> Content response with details about one or more items.</returns>
        public static Task<ContentResponse> LookupContinuationAsync(this IXboxMusicClient client, string itemId, string continuationToken)
        {
            return client.LookupContinuationAsync(new List<string> { itemId }, continuationToken);
        }
    }
}
