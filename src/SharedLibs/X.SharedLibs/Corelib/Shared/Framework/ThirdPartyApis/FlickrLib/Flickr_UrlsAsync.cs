using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Returns the url to a group's page.
        /// </summary>
        /// <param name="groupId">The NSID of the group to fetch the url for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> UrlsGetGroupAsync(string groupId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.urls.getGroup");
            parameters.Add("group_id", groupId);

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            
            FlickrResult<string> result = new FlickrResult<string>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = r.Result.GetAttributeValue("*", "url");
            }
            return result;
            
        }

        /// <summary>
        /// Returns the url to a user's photos.
        /// </summary>
        /// <returns>An instance of the <see cref="Uri"/> class containing the URL for the users photos.</returns>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> UrlsGetUserPhotosAsync()
        {
            CheckRequiresAuthentication();

            return await UrlsGetUserPhotosAsync(null);
        }

        /// <summary>
        /// Returns the url to a user's photos.
        /// </summary>
        /// <param name="userId">The NSID of the user to fetch the url for. If omitted, the calling user is assumed.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> UrlsGetUserPhotosAsync(string userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.urls.getUserPhotos");
            if (userId != null && userId.Length > 0) parameters.Add("user_id", userId);

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            FlickrResult<string> result = new FlickrResult<string>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = r.Result.GetAttributeValue("*", "url");
            }
            return result;
                
        }

        /// <summary>
        /// Returns the url to a user's profile.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> UrlsGetUserProfileAsync()
        {
            CheckRequiresAuthentication();

            return await UrlsGetUserProfileAsync(null);
        }

        /// <summary>
        /// Returns the url to a user's profile.
        /// </summary>
        /// <param name="userId">The NSID of the user to fetch the url for. If omitted, the calling user is assumed.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> UrlsGetUserProfileAsync(string userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.urls.getUserProfile");
            if (userId != null && userId.Length > 0) parameters.Add("user_id", userId);

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            FlickrResult<string> result = new FlickrResult<string>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = r.Result.GetAttributeValue("*", "url");
            }
            return result;
  
        }

        /// <summary>
        /// Returns gallery info, by url.
        /// </summary>
        /// <param name="url">The gallery's URL.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Gallery>> UrlsLookupGalleryAsync(string url)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.urls.lookupGallery");
            parameters.Add("api_key", apiKey);
            parameters.Add("url", url);

            return await GetResponseAsync<Gallery>(parameters);
        }

        /// <summary>
        /// Returns a group NSID, given the url to a group's page or photo pool.
        /// </summary>
        /// <param name="urlToFind">The url to the group's page or photo pool.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> UrlsLookupGroupAsync(string urlToFind)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.urls.lookupGroup");
            parameters.Add("api_key", apiKey);
            parameters.Add("url", urlToFind);

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            FlickrResult<string> result = new FlickrResult<string>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = r.Result.GetAttributeValue("*", "id");
            }
            return result;
                
        }

        /// <summary>
        /// Returns a user NSID, given the url to a user's photos or profile.
        /// </summary>
        /// <param name="urlToFind">Thr url to the user's profile or photos page.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<FoundUser>> UrlsLookupUserAsync(string urlToFind)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.urls.lookupUser");
            parameters.Add("api_key", apiKey);
            parameters.Add("url", urlToFind);

            return await GetResponseAsync<FoundUser>(parameters);
        }
    }
}
