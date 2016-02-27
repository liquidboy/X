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
        /// Correct the places hierarchy for all the photos for a user at a given latitude, longitude and accuracy.
        /// </summary>
        /// <remarks>
        /// Batch corrections are processed in a delayed queue so it may take a few minutes before the changes are reflected in a user's photos.
        /// </remarks>
        /// <param name="latitude">The latitude of the photos to be update whose valid range is -90 to 90. Anything more than 6 decimal places will be truncated.</param>
        /// <param name="longitude">The longitude of the photos to be updated whose valid range is -180 to 180. Anything more than 6 decimal places will be truncated.</param>
        /// <param name="accuracy">Recorded accuracy level of the photos to be updated. World level is 1, Country is ~3, Region ~6, City ~11, Street ~16. Current range is 1-16. Defaults to 16 if not specified.</param>
        /// <param name="placeId">A Flickr Places ID. (While optional, you must pass either a valid Places ID or a WOE ID.)</param>
        /// <param name="woeId">A Where On Earth (WOE) ID. (While optional, you must pass either a valid Places ID or a WOE ID.)</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoBatchCorrectLocationAsync(double latitude, double longitude, GeoAccuracy accuracy, string placeId, string woeId)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.batchCorrectLocation");
            parameters.Add("lat", latitude.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("lon", longitude.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("accuracy", accuracy.ToString("D"));
            parameters.Add("place_id", placeId);
            parameters.Add("woe_id", woeId);

            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Correct the places hierarchy for a given photo.
        /// </summary>
        /// <param name="photoId">The ID of the photo whose WOE location is being corrected.</param>
        /// <param name="placeId">A Flickr Places ID. (While optional, you must pass either a valid Places ID or a WOE ID.)</param>
        /// <param name="woeId">A Where On Earth (WOE) ID. (While optional, you must pass either a valid Places ID or a WOE ID.)</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoCorrectLocationAsync(string photoId, string placeId, string woeId)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.correctLocation");
            parameters.Add("photo_id", photoId);
            parameters.Add("place_id", placeId);
            parameters.Add("woe_id", woeId);

            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Returns the location data for a give photo.
        /// </summary>
        /// <param name="photoId">The ID of the photo to return the location information for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PlaceInfo>> PhotosGeoGetLocationAsync(string photoId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.getLocation");
            parameters.Add("photo_id", photoId);

            var r = await GetResponseAsync<PhotoInfo>(
                parameters);

            FlickrResult<PlaceInfo> result = new FlickrResult<PlaceInfo>();
            result.HasError = r.HasError;
            if (result.HasError)
            {
                if (result.ErrorCode == 2)
                {
                    result.HasError = false;
                    result.Result = null;
                    result.Error = null;
                }
                else
                {
                    result.Error = r.Error;
                }
            }
            else
            {
                result.Result = r.Result.Location;
            }
            return result;
                
        }

        /// <summary>
        /// Indicate the state of a photo's geotagginess beyond latitude and longitude.
        /// </summary>
        /// <remarks>
        /// Note : photos passed to this method must already be geotagged (using the flickr.photos.geo.setLocation method).
        /// </remarks>
        /// <param name="photoId">The id of the photo to set context data for.</param>
        /// <param name="context">ontext is a numeric value representing the photo's geotagginess beyond latitude and longitude. For example, you may wish to indicate that a photo was taken "indoors" or "outdoors". </param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoSetContextAsync(string photoId, GeoContext context)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.photos.geo.setContext");
            parameters.Add("photo_id", photoId);
            parameters.Add("context", context.ToString("D"));

            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Sets the geo location for a photo.
        /// </summary>
        /// <param name="photoId">The ID of the photo to set to location for.</param>
        /// <param name="latitude">The latitude of the geo location. A double number ranging from -180.00 to 180.00. Digits beyond 6 decimal places will be truncated.</param>
        /// <param name="longitude">The longitude of the geo location. A double number ranging from -180.00 to 180.00. Digits beyond 6 decimal places will be truncated.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoSetLocationAsync(string photoId, double latitude, double longitude)
        {
            return await PhotosGeoSetLocationAsync(photoId, latitude, longitude, GeoAccuracy.None);
        }

        /// <summary>
        /// Sets the geo location for a photo.
        /// </summary>
        /// <param name="photoId">The ID of the photo to set to location for.</param>
        /// <param name="latitude">The latitude of the geo location. A double number ranging from -180.00 to 180.00. Digits beyond 6 decimal places will be truncated.</param>
        /// <param name="longitude">The longitude of the geo location. A double number ranging from -180.00 to 180.00. Digits beyond 6 decimal places will be truncated.</param>
        /// <param name="accuracy">The accuracy of the photos geo location.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoSetLocationAsync(string photoId, double latitude, double longitude, GeoAccuracy accuracy)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.setLocation");
            parameters.Add("photo_id", photoId);
            parameters.Add("lat", latitude.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("lon", longitude.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (accuracy != GeoAccuracy.None)
                parameters.Add("accuracy", accuracy.ToString("D"));

            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Return a list of photos for a user at a specific latitude, longitude and accuracy.
        /// </summary>
        /// <param name="latitude">The latitude whose valid range is -90 to 90. Anything more than 6 decimal places will be truncated.</param>
        /// <param name="longitude">The longitude whose valid range is -180 to 180. Anything more than 6 decimal places will be truncated.</param>
        /// <param name="accuracy">Recorded accuracy level of the location information. World level is 1, Country is ~3, Region ~6, City ~11, Street ~16. Current range is 1-16. Defaults to 16 if not specified.</param>
        /// <param name="extras"></param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PhotosGeoPhotosForLocationAsync(double latitude, double longitude, GeoAccuracy accuracy, PhotoSearchExtras extras, int perPage, int page)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.photos.geo.photosForLocation");
            parameters.Add("lat", latitude.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("lon", longitude.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("accuracy", accuracy.ToString("D"));
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PhotoCollection>(parameters);

        }

        /// <summary>
        /// Removes Location information.
        /// </summary>
        /// <param name="photoId">The photo ID of the photo to remove information from.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoRemoveLocationAsync(string photoId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.removeLocation");
            parameters.Add("photo_id", photoId);

            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Gets a list of photos that do not contain geo location information.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PhotosGetWithoutGeoDataAsync()
        {
            PartialSearchOptions options = new PartialSearchOptions();
            return await PhotosGetWithoutGeoDataAsync(options);
        }

        /// <summary>
        /// Gets a list of photos that do not contain geo location information.
        /// </summary>
        /// <param name="options">A limited set of options are supported.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PhotosGetWithoutGeoDataAsync(PartialSearchOptions options)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.getWithoutGeoData");
            UtilityMethods.PartialOptionsIntoArray(options, parameters);

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Gets a list of photos that contain geo location information.
        /// </summary>
        /// <remarks>
        /// Note, this method doesn't actually return the location information with the photos, 
        /// unless you specify the <see cref="PhotoSearchExtras.Geo"/> option in the <c>extras</c> parameter.
        /// </remarks>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PhotosGetWithGeoDataAsync()
        {
            PartialSearchOptions options = new PartialSearchOptions();
            return await PhotosGetWithGeoDataAsync(options);
        }

        /// <summary>
        /// Gets a list of photos that contain geo location information.
        /// </summary>
        /// <remarks>
        /// Note, this method doesn't actually return the location information with the photos, 
        /// unless you specify the <see cref="PhotoSearchExtras.Geo"/> option in the <c>extras</c> parameter.
        /// </remarks>
        /// <param name="options">The options to filter/sort the results by.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> PhotosGetWithGeoDataAsync(PartialSearchOptions options)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.getWithGeoData");
            UtilityMethods.PartialOptionsIntoArray(options, parameters);

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Get permissions for a photo.
        /// </summary>
        /// <param name="photoId">The id of the photo to get permissions for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GeoPermissions>> PhotosGeoGetPermsAsync(string photoId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.getPerms");
            parameters.Add("photo_id", photoId);

            return await GetResponseAsync<GeoPermissions>(parameters);
        }

        /// <summary>
        /// Set the permission for who can see geotagged photos on Flickr.
        /// </summary>
        /// <param name="photoId">The ID of the photo permissions to update.</param>
        /// <param name="isPublic"></param>
        /// <param name="isContact"></param>
        /// <param name="isFamily"></param>
        /// <param name="isFriend"></param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosGeoSetPermsAsync(string photoId, bool isPublic, bool isContact, bool isFamily, bool isFriend)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.geo.setPerms");
            parameters.Add("photo_id", photoId);
            parameters.Add("is_public", isPublic ? "1" : "0");
            parameters.Add("is_contact", isContact ? "1" : "0");
            parameters.Add("is_friend", isFriend ? "1" : "0");
            parameters.Add("is_family", isFamily ? "1" : "0");

            return await GetResponseAsync<NoResponse>(parameters);
        }

    }
}
