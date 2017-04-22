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
        /// Used to fid a flickr users details by specifying their email address.
        /// </summary>
        /// <param name="emailAddress">The email address to search on.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        /// <exception cref="FlickrApiException">A FlickrApiException is raised if the email address is not found.</exception>
        public async Task<FlickrResult<FoundUser>> PeopleFindByEmailAsync(string emailAddress)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.findByEmail");
            parameters.Add("api_key", apiKey);
            parameters.Add("find_email", emailAddress);

            return await GetResponseAsync<FoundUser>(parameters);
        }

        /// <summary>
        /// Returns a <see cref="FoundUser"/> object matching the screen name.
        /// </summary>
        /// <param name="userName">The screen name or username of the user.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        /// <exception cref="FlickrApiException">A FlickrApiException is raised if the email address is not found.</exception>
        public async Task<FlickrResult<FoundUser>> PeopleFindByUserNameAsync(string userName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.findByUsername");
            parameters.Add("api_key", apiKey);
            parameters.Add("username", userName);

            return await GetResponseAsync<FoundUser>(parameters);
        }

        /// <summary>
        /// Gets the <see cref="Person"/> object for the given user id.
        /// </summary>
        /// <param name="userId">The user id to find.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Person>> PeopleGetInfoAsync(string userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.getInfo");
            parameters.Add("api_key", apiKey);
            parameters.Add("user_id", userId);

            return await GetResponseAsync<Person>(parameters);
        }

        /// <summary>
        /// Gets the upload status of the authenticated user.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<UserStatus>> PeopleGetUploadStatusAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.getUploadStatus");

            return await GetResponseAsync<UserStatus>(parameters);
        }

        /// <summary>
        /// Get a list of public groups for a user.
        /// </summary>
        /// <param name="userId">The user id to get groups for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PublicGroupInfoCollection>> PeopleGetPublicGroupsAsync(string userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.getPublicGroups");
            parameters.Add("api_key", apiKey);
            parameters.Add("user_id", userId);

            return await GetResponseAsync<PublicGroupInfoCollection>(parameters);
        }

        /// <summary>
        /// Gets a users public photos. Excludes private photos.
        /// </summary>
        /// <param name="userId">The user id of the user.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPublicPhotosAsync(string userId)
        {
            return await PeopleGetPublicPhotosAsync(userId, 0, 0, SafetyLevel.None, PhotoSearchExtras.None);
        }

        /// <summary>
        /// Gets a users public photos. Excludes private photos.
        /// </summary>
        /// <param name="userId">The user id of the user.</param>
        /// <param name="page">The page to return. Defaults to page 1.</param>
        /// <param name="perPage">The number of photos to return per page. Default is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPublicPhotosAsync(string userId, int page, int perPage)
        {
            return await PeopleGetPublicPhotosAsync(userId, page, perPage, SafetyLevel.None, PhotoSearchExtras.None);
        }

        /// <summary>
        /// Gets a users public photos. Excludes private photos.
        /// </summary>
        /// <param name="userId">The user id of the user.</param>
        /// <param name="page">The page to return. Defaults to page 1.</param>
        /// <param name="perPage">The number of photos to return per page. Default is 100.</param>
        /// <param name="extras">Which (if any) extra information to return. The default is none.</param>
        /// <param name="safetyLevel">The safety level of the returned photos. 
        /// Unauthenticated calls can only return Safe photos.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPublicPhotosAsync(string userId, int page, int perPage, SafetyLevel safetyLevel, PhotoSearchExtras extras)
        {
            if (!IsAuthenticated && safetyLevel > SafetyLevel.Safe)
                throw new ArgumentException("Safety level may only be 'Safe' for unauthenticated calls", "safetyLevel");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.getPublicPhotos");
            parameters.Add("api_key", apiKey);
            parameters.Add("user_id", userId);
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (safetyLevel != SafetyLevel.None) parameters.Add("safety_level", safetyLevel.ToString("D"));
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Return photos from the calling user's photostream. This method must be authenticated;
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync()
        {
            return await PeopleGetPhotosAsync(null, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Return photos from the calling user's photostream. This method must be authenticated;
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(int page, int perPage)
        {
            return await PeopleGetPhotosAsync(null, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Return photos from the calling user's photostream. This method must be authenticated;
        /// </summary>
        /// <param name="extras">A list of extra information to fetch for each returned record.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(PhotoSearchExtras extras)
        {
            return await PeopleGetPhotosAsync(null, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, extras, 0, 0);
        }

        /// <summary>
        /// Return photos from the calling user's photostream. This method must be authenticated;
        /// </summary>
        /// <param name="extras">A list of extra information to fetch for each returned record.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(PhotoSearchExtras extras, int page, int perPage)
        {
            return await PeopleGetPhotosAsync(null, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, extras, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// </summary>
        /// <param name="userId">The NSID of the user who's photos to return. A value of "me" will return the calling user's photos.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(string userId)
        {
            return await PeopleGetPhotosAsync(userId, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// </summary>
        /// <param name="userId">The NSID of the user who's photos to return. A value of "me" will return the calling user's photos.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(string userId, int page, int perPage)
        {
            return await PeopleGetPhotosAsync(userId, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// </summary>
        /// <param name="userId">The NSID of the user who's photos to return. A value of "me" will return the calling user's photos.</param>
        /// <param name="extras">A list of extra information to fetch for each returned record.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(string userId, PhotoSearchExtras extras)
        {
            return await PeopleGetPhotosAsync(userId, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, extras, 0, 0);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// </summary>
        /// <param name="userId">The NSID of the user who's photos to return. A value of "me" will return the calling user's photos.</param>
        /// <param name="extras">A list of extra information to fetch for each returned record.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(string userId, PhotoSearchExtras extras, int page, int perPage)
        {
            return await PeopleGetPhotosAsync(userId, SafetyLevel.None, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, ContentTypeSearch.None, PrivacyFilter.None, extras, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// </summary>
        /// <param name="userId">The NSID of the user who's photos to return. A value of "me" will return the calling user's photos.</param>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned.</param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned.</param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. </param>
        /// <param name="contentType">Content Type setting</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="extras">A list of extra information to fetch for each returned record.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PeopleGetPhotosAsync(string userId, SafetyLevel safeSearch, DateTime minUploadDate, DateTime maxUploadDate, DateTime minTakenDate, DateTime maxTakenDate, ContentTypeSearch contentType, PrivacyFilter privacyFilter, PhotoSearchExtras extras, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.people.getPhotos");
            parameters.Add("user_id", userId ?? "me");
            if (safeSearch != SafetyLevel.None) parameters.Add("safe_search", safeSearch.ToString("d"));
            if (minUploadDate != DateTime.MinValue) parameters.Add("min_upload_date", UtilityMethods.DateToUnixTimestamp(minUploadDate));
            if (maxUploadDate != DateTime.MinValue) parameters.Add("max_upload_date", UtilityMethods.DateToUnixTimestamp(maxUploadDate));
            if (minTakenDate != DateTime.MinValue) parameters.Add("min_taken_date", UtilityMethods.DateToMySql(minTakenDate));
            if (maxTakenDate != DateTime.MinValue) parameters.Add("max_taken_date", UtilityMethods.DateToMySql(maxTakenDate));

            if (contentType != ContentTypeSearch.None) parameters.Add("content_type", contentType.ToString("d"));
            if (privacyFilter != PrivacyFilter.None) parameters.Add("privacy_filter", privacyFilter.ToString("d"));
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));

            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Gets the photos containing the authenticated user. Requires that the AuthToken has been set.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PeoplePhotoCollection>> PeopleGetPhotosOfAsync()
        {
            CheckRequiresAuthentication();

            return await PeopleGetPhotosOfAsync("me", PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets the photos containing the specified user.
        /// </summary>
        /// <param name="userId">The user ID to get photos of.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PeoplePhotoCollection>> PeopleGetPhotosOfAsync(string userId)
        {
            return await PeopleGetPhotosOfAsync(userId, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets the photos containing the specified user.
        /// </summary>
        /// <param name="userId">The user ID to get photos of.</param>
        /// <param name="extras">A list of extras to return for each photo.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PeoplePhotoCollection>> PeopleGetPhotosOfAsync(string userId, PhotoSearchExtras extras)
        {
            return await PeopleGetPhotosOfAsync(userId, extras, 0, 0);
        }

        /// <summary>
        /// Gets the photos containing the specified user.
        /// </summary>
        /// <param name="userId">The user ID to get photos of.</param>
        /// <param name="perPage">The number of photos to return per page.</param>
        /// <param name="page">The page of photos to return. Default is 1.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PeoplePhotoCollection>> PeopleGetPhotosOfAsync(string userId, int page, int perPage)
        {
            return await PeopleGetPhotosOfAsync(userId, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Gets the photos containing the specified user.
        /// </summary>
        /// <param name="userId">The user ID to get photos of.</param>
        /// <param name="extras">A list of extras to return for each photo.</param>
        /// <param name="perPage">The number of photos to return per page.</param>
        /// <param name="page">The page of photos to return. Default is 1.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PeoplePhotoCollection>> PeopleGetPhotosOfAsync(string userId, PhotoSearchExtras extras, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.people.getPhotosOf");
            parameters.Add("user_id", userId);
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PeoplePhotoCollection>(parameters);
        }

    }
}
