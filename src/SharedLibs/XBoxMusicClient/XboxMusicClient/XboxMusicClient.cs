// Copyright (c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated  documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to
// do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software. 
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xbox.Music.Platform.Contract.AuthenticationDataModel;
using Microsoft.Xbox.Music.Platform.Contract.DataModel;
using Microsoft.Xbox.Music.Platform.Contract.DataModel.CollectionEdit;

namespace Microsoft.Xbox.Music.Platform.Client
{
    internal class XboxMusicClient : SimpleServiceClient, IXboxMusicClient
    {
        private readonly Uri hostname = new Uri("https://music.xboxlive.com");

        private readonly AzureDataMarketAuthenticationCache azureDataMarketAuthenticationCache;

        private readonly IXToken xToken;
        public IXToken XToken { get { return xToken; } }
    
        internal XboxMusicClient(string clientId, string clientSecret)
            : this(new AzureDataMarketAuthenticationCache(clientId, clientSecret))
        {
        }

        private XboxMusicClient(AzureDataMarketAuthenticationCache azureDataMarketAuthenticationCache, IXToken xToken = null)
        {
            this.azureDataMarketAuthenticationCache = azureDataMarketAuthenticationCache;
            this.xToken = xToken;
        }

        public override void Dispose()
        {
            azureDataMarketAuthenticationCache.Dispose();
            base.Dispose();
        }

        public IXboxMusicClient CreateUserAuthenticatedClient(IXToken xToken)
        {
            if (xToken == null)
            {
                throw new ArgumentNullException("xToken", "User authentication should be provided.");
            }
            return new XboxMusicClient(azureDataMarketAuthenticationCache, xToken);
        }

        #region Search
        private async Task<ContentResponse> SearchApiAsync(Namespace mediaNamespace, string query = null, ContentSource? source = null,
            SearchFilter filter = SearchFilter.Default, string language = null, string country = null,
            int? maxItems = null, string continuationToken = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(continuationToken, language, country, source);
            if (!String.IsNullOrEmpty(query))
                requestParameters.Add("q", Uri.EscapeDataString(query));
            if (filter != SearchFilter.Default)
                requestParameters.Add("filters", filter.ToString().Replace(", ", "+"));
            if (maxItems.HasValue)
                requestParameters.Add("maxItems", maxItems.ToString());
            return await GetWithRetryOnExpiredTokenAsync<ContentResponse>("/1/content/" + mediaNamespace + "/search",
                new CancellationToken(false), requestParameters);
        }

        public Task<ContentResponse> SearchAsync(Namespace mediaNamespace, string query, ContentSource? source = null, SearchFilter filter = SearchFilter.Default,
            string language = null, string country = null, int? maxItems = null)
        {
            return SearchApiAsync(mediaNamespace, query, source, filter, language, country, maxItems);
        }

        public Task<ContentResponse> SearchContinuationAsync(Namespace mediaNamespace, string continuationToken)
        {
            return SearchApiAsync(mediaNamespace, continuationToken: continuationToken);
        }
        #endregion

        #region Lookup
        private async Task<ContentResponse> LookupApiAsync(IEnumerable<string> itemIds, ContentSource? source = null,
            string language = null, string country = null, ExtraDetails extras = ExtraDetails.None,
            string continuationToken = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(continuationToken, language, country, source);
            if (extras != ExtraDetails.None)
            {
                string extra = extras.ToString().Replace(", ", "+");
                requestParameters.Add("extras", extra);
            }
            string ids = itemIds.Aggregate("",
                (current, id) => current + (!String.IsNullOrEmpty(current) ? "+" : "") + id);
            return await GetWithRetryOnExpiredTokenAsync<ContentResponse>("/1/content/" + ids + "/lookup", new CancellationToken(false),
                requestParameters);
        }

        public Task<ContentResponse> LookupAsync(List<string> itemIds, ContentSource? source = null, string language = null,
            string country = null, ExtraDetails extras = ExtraDetails.None)
        {
            return LookupApiAsync(itemIds, source, language, country, extras);
        }

        public Task<ContentResponse> LookupContinuationAsync(List<string> itemIds, string continuationToken)
        {
            return LookupApiAsync(itemIds, continuationToken: continuationToken);
        }
        #endregion

        #region Browse
        private async Task<ContentResponse> BrowseApiAsync(Namespace mediaNamespace, ContentSource source, ItemType type,
            string genre = null, OrderBy? orderBy = null, int? maxItems = null, int? page = null,
            string language = null, string country = null, string continuationToken = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(continuationToken, language, country);
            if (genre != null)
                requestParameters.Add("genre", genre);
            if (orderBy.HasValue)
                requestParameters.Add("orderby", orderBy.ToString());
            if (maxItems.HasValue)
                requestParameters.Add("maxitems", maxItems.ToString());
            if (page.HasValue)
                requestParameters.Add("page", page.ToString());
            return await GetWithRetryOnExpiredTokenAsync<ContentResponse>(
                "/1/content/" + mediaNamespace + "/" + source + "/" + type + "/browse",
                new CancellationToken(false), requestParameters);
        }

        public Task<ContentResponse> BrowseAsync(Namespace mediaNamespace, ContentSource source, ItemType type,
            string genre = null, OrderBy? orderBy = null, int? maxItems = null, int? page = null, string language = null,
            string country = null)
        {
            return BrowseApiAsync(mediaNamespace, source, type, genre, orderBy, maxItems, page, language, country);
        }

        public Task<ContentResponse> BrowseContinuationAsync(Namespace mediaNamespace, ContentSource source, ItemType type,
            string continuationToken)
        {
            return BrowseApiAsync(mediaNamespace, source, type, continuationToken: continuationToken);
        }

        private async Task<ContentResponse> SubBrowseApiAsync(string id, ContentSource source, BrowseItemType browseType, ExtraDetails extra, OrderBy? orderBy = null, int? maxItems = null, int? page = null, string language = null, string country = null, string continuationToken = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(continuationToken, language, country);
            if (orderBy.HasValue)
                requestParameters.Add("orderby", orderBy.ToString());
            if (maxItems.HasValue)
                requestParameters.Add("maxitems", maxItems.ToString());
            if (page.HasValue)
                requestParameters.Add("page", page.ToString());
            return await GetWithRetryOnExpiredTokenAsync<ContentResponse>(
                "/1/content/" + id + "/" + source + "/" + browseType + "/" + extra + "/browse",
                new CancellationToken(false), requestParameters);
        }

        public Task<ContentResponse> SubBrowseAsync(string id, ContentSource source, BrowseItemType browseType, ExtraDetails extra, OrderBy? orderBy = null, int? maxItems = null, int? page = null, string language = null, string country = null)
        {
            return SubBrowseApiAsync(id, source, browseType, extra, orderBy, maxItems, page, language, country);
        }

        public Task<ContentResponse> SubBrowseContinuationAsync(string id, ContentSource source, BrowseItemType browseType, ExtraDetails extra, string continuationToken)
        {
            return SubBrowseApiAsync(id, source, browseType, extra, continuationToken: continuationToken);
        }
        #endregion

        #region Discovery
        private async Task<ContentResponse> DiscoverAsync(Namespace mediaNamespace, string type,
            string country = null, string language = null, string genre = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(language: language, country: country);
            if (!String.IsNullOrEmpty(genre))
                requestParameters.Add("genre", genre);
            return await GetWithRetryOnExpiredTokenAsync<ContentResponse>("/1/content/" + mediaNamespace + "/" + type,
                new CancellationToken(false), requestParameters);
        }
        public Task<ContentResponse> SpotlightAsync(Namespace mediaNamespace, string language = null,
            string country = null)
        {
            return DiscoverAsync(mediaNamespace, "spotlight", country: country, language: language);
        }

        public Task<ContentResponse> NewReleasesAsync(Namespace mediaNamespace, string genre = null,
            string language = null, string country = null)
        {
            return DiscoverAsync(mediaNamespace, "newreleases", country, language, genre);
        }

        public async Task<ContentResponse> BrowseGenresAsync(Namespace mediaNamespace,
            string language = null, string country = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(language: language, country: country);
            return await GetWithRetryOnExpiredTokenAsync<ContentResponse>("/1/content/" + mediaNamespace + "/catalog/genres",
                new CancellationToken(false), requestParameters);
        }
        #endregion

        #region Collection
        public async Task<TrackActionResponse> CollectionOperationAsync(Namespace mediaNamespace, TrackActionType operation, TrackActionRequest trackActionRequest)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync();
            return await PostWithRetryOnExpiredTokenAsync<TrackActionResponse, TrackActionRequest>("/1/content/" + mediaNamespace + "/collection/" + operation,
                trackActionRequest, new CancellationToken(false), requestParameters);
        }

        public async Task<PlaylistActionResponse> PlaylistOperationAsync(Namespace mediaNamespace, PlaylistActionType operation,
            PlaylistAction playlistAction)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync();
            return await PostWithRetryOnExpiredTokenAsync<PlaylistActionResponse, PlaylistAction>("/1/content/" + mediaNamespace + "/collection/playlists/" + operation,
                playlistAction, new CancellationToken(false), requestParameters);
        }
        #endregion

        #region Location
        private async Task<StreamResponse> LocationAsync(string id, string clientInstanceId,
            string type, string country = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(country: country);
            if (!String.IsNullOrEmpty(clientInstanceId))
                requestParameters.Add("clientInstanceId", clientInstanceId);
            return await GetWithRetryOnExpiredTokenAsync<StreamResponse>("/1/content/" + id + "/" + type, new CancellationToken(false),
                requestParameters);
        }

        public Task<StreamResponse> StreamAsync(string id, string clientInstanceId)
        {
            return LocationAsync(id, clientInstanceId, "stream");
        }

        public Task<StreamResponse> PreviewAsync(string id, string clientInstanceId, string country = null)
        {
            return LocationAsync(id, clientInstanceId, "preview", country);
        }
        #endregion

        #region User Profile
        public async Task<UserProfileResponse> GetUserProfileAsync(Namespace mediaNamespace, string language = null, string country = null)
        {
            Dictionary<string, string> requestParameters = await FormatRequestParametersAsync(language: language, country: country);
            return await GetWithRetryOnExpiredTokenAsync<UserProfileResponse>("/1/user/" + mediaNamespace + "/profile",
                new CancellationToken(false), requestParameters);
        }
        #endregion

        private Task<TResult> PostWithRetryOnExpiredTokenAsync<TResult, TRequest>(
            string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            IEnumerable<KeyValuePair<string, string>> requestParameters)
            where TResult : class
        {
            return RetryOnExpiredTokenAsync(cancellationToken,
                async ct => await PostAsync<TResult, TResult, TRequest>(hostname, relativeUri, requestPayload, ct, requestParameters, FormatRequestHeaders()));
        }

        private Task<TResult> GetWithRetryOnExpiredTokenAsync<TResult>(string relativeUri,
            CancellationToken cancellationToken,
            IEnumerable<KeyValuePair<string, string>> requestParameters)
            where TResult : class
        {
            return RetryOnExpiredTokenAsync(cancellationToken, 
                async ct => await GetAsync<TResult, TResult>(hostname, relativeUri, ct, requestParameters, FormatRequestHeaders()));
        }

        private async Task<TResult> RetryOnExpiredTokenAsync<TResult>(CancellationToken cancellationToken, Func<CancellationToken, Task<SimpleServiceResult<TResult, TResult>>> action)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result = await action(cancellationToken);
            if ((result.HttpStatusCode == HttpStatusCode.Unauthorized || result.HttpStatusCode == HttpStatusCode.Forbidden) &&
                result.AuthorizationTokenInvalid != null &&
                result.AuthorizationTokenInvalid.Value == AuthorizationTokenInvalid.Expired)
            {
                if (xToken != null && await xToken.RefreshAsync(cancellationToken))
                {
                    result = await action(cancellationToken);
                }
            }

            return result.Result ?? result.ErrorResult;
        }

        private async Task<Dictionary<string, string>> FormatRequestParametersAsync(string continuationToken = null,
            string language = null, string country = null, ContentSource? source = null)
        {
            AzureDataMarketAuthenticationCache.AccessToken token = await azureDataMarketAuthenticationCache.CheckAndRenewTokenAsync(new CancellationToken(false));
            Dictionary<string, string> requestParameters = new Dictionary<string, string>
            {
                {"accessToken", Uri.EscapeDataString("Bearer " + token.Token)}
            };
            if (!String.IsNullOrEmpty(continuationToken))
                requestParameters.Add("continuationToken", continuationToken);
            if (!String.IsNullOrEmpty(language))
                requestParameters.Add("language", language);
            if (!String.IsNullOrEmpty(country))
                requestParameters.Add("country", country);
            if (source.HasValue)
            {
                string sources = source.ToString().Replace(", ", "+");
                requestParameters.Add("source", sources);
            }
            return requestParameters;
        }

        private Dictionary<string, string> FormatRequestHeaders()
        {
            return XToken != null && !String.IsNullOrEmpty(XToken.AuthorizationHeaderValue)
                ? new Dictionary<string, string> { { "Authorization", XToken.AuthorizationHeaderValue } }
                : null;
        }
    }
}
