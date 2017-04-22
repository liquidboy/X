using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Get the tag list for a given photo.
        /// </summary>
        /// <param name="photoId">The id of the photo to return tags for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Collection<PhotoInfoTag>>> TagsGetListPhotoAsync(string photoId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getListPhoto");
            parameters.Add("api_key", apiKey);
            parameters.Add("photo_id", photoId);

            var r = await GetResponseAsync<PhotoInfo>(parameters);
            
            FlickrResult<Collection<PhotoInfoTag>> result = new FlickrResult<Collection<PhotoInfoTag>>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = r.Result.Tags;
            }
            return result;
            
        }

        /// <summary>
        /// Get the tag list for a given user (or the currently logged in user).
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetListUserAsync()
        {
            return await TagsGetListUserAsync(null);
        }

        /// <summary>
        /// Get the tag list for a given user (or the currently logged in user).
        /// </summary>
        /// <param name="userId">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetListUserAsync(string userId )
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getListUser");
            if (userId != null && userId.Length > 0) parameters.Add("user_id", userId);

            return await GetResponseAsync<TagCollection>(parameters);
        }

        /// <summary>
        /// Get the popular tags for a given user (or the currently logged in user).
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetListUserPopularAsync()
        {
            CheckRequiresAuthentication();

            return await TagsGetListUserPopularAsync(null, 0);
        }

        /// <summary>
        /// Get the popular tags for a given user (or the currently logged in user).
        /// </summary>
        /// <param name="count">Number of popular tags to return. defaults to 10 when this argument is not present.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetListUserPopularAsync(int count )
        {
            CheckRequiresAuthentication();

            return await TagsGetListUserPopularAsync(null, count);
        }

        /// <summary>
        /// Get the popular tags for a given user (or the currently logged in user).
        /// </summary>
        /// <param name="userId">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetListUserPopularAsync(string userId )
        {
            return await TagsGetListUserPopularAsync(userId, 0);
        }

        /// <summary>
        /// Get the popular tags for a given user (or the currently logged in user).
        /// </summary>
        /// <param name="userId">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <param name="count">Number of popular tags to return. defaults to 10 when this argument is not present.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetListUserPopularAsync(string userId, int count )
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getListUserPopular");
            if (userId != null) parameters.Add("user_id", userId);
            if (count > 0) parameters.Add("count", count.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<TagCollection>(parameters);
        }

        /// <summary>
        /// Gets a list of 'cleaned' tags and the raw values for those tags.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<RawTagCollection>> TagsGetListUserRawAsync()
        {
            return await TagsGetListUserRawAsync(null);
        }

        /// <summary>
        /// Gets a list of 'cleaned' tags and the raw values for a specific tag.
        /// </summary>
        /// <param name="tag">The tag to return the raw version of.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<RawTagCollection>> TagsGetListUserRawAsync(string tag)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getListUserRaw");
            if (tag != null && tag.Length > 0) parameters.Add("tag", tag);

            return await GetResponseAsync<RawTagCollection>(parameters);
        }

        /// <summary>
        /// Returns a list of tags 'related' to the given tag, based on clustered usage analysis.
        /// </summary>
        /// <param name="tag">The tag to fetch related tags for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<TagCollection>> TagsGetRelatedAsync(string tag )
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getRelated");
            parameters.Add("api_key", apiKey);
            parameters.Add("tag", tag);

            return await GetResponseAsync<TagCollection>(parameters);
        }

        /// <summary>
        /// Gives you a list of tag clusters for the given tag.
        /// </summary>
        /// <param name="tag">The tag to fetch clusters for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ClusterCollection>> TagsGetClustersAsync(string tag)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getClusters");
            parameters.Add("tag", tag);

            return await GetResponseAsync<ClusterCollection>(parameters);
        }

        /// <summary>
        /// Returns the first 24 photos for a given tag cluster.
        /// </summary>
        /// <param name="cluster">The <see cref="Cluster"/> instance to return the photos for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> TagsGetClusterPhotosAsync(Cluster cluster)
        {
            return await TagsGetClusterPhotosAsync(cluster.SourceTag, cluster.ClusterId, PhotoSearchExtras.None);
        }

        /// <summary>
        /// Returns the first 24 photos for a given tag cluster.
        /// </summary>
        /// <param name="cluster">The <see cref="Cluster"/> instance to return the photos for.</param>
        /// <param name="extras">Extra information to return with each photo.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> TagsGetClusterPhotosAsync(Cluster cluster, PhotoSearchExtras extras)
        {
            return await TagsGetClusterPhotosAsync(cluster.SourceTag, cluster.ClusterId, extras);
        }

        /// <summary>
        /// Returns the first 24 photos for a given tag cluster.
        /// </summary>
        /// <param name="tag">The tag whose cluster photos you want to return.</param>
        /// <param name="clusterId">The cluster id for the cluster you want to return the photos. This is the first three subtags of the tag cluster appended with hyphens ('-').</param>
        /// <param name="extras">Extra information to return with each photo.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> TagsGetClusterPhotosAsync(string tag, string clusterId, PhotoSearchExtras extras)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getClusterPhotos");
            parameters.Add("tag", tag);
            parameters.Add("cluster_id", clusterId);
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Returns a list of hot tags for the given period.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<HotTagCollection>> TagsGetHotListAsync()
        {
            return await TagsGetHotListAsync(null, 0);
        }

        /// <summary>
        /// Returns a list of hot tags for the given period.
        /// </summary>
        /// <param name="period">The period for which to fetch hot tags. Valid values are day and week (defaults to day).</param>
        /// <param name="count">The number of tags to return. Defaults to 20. Maximum allowed value is 200.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<HotTagCollection>> TagsGetHotListAsync(string period, int count)
        {
            if (!String.IsNullOrEmpty(period) && period != "day" && period != "week")
            {
                throw new ArgumentException("Period must be either 'day' or 'week'.", "period");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.tags.getHotList");
            if (!String.IsNullOrEmpty(period)) parameters.Add("period", period);
            if (count > 0) parameters.Add("count", count.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<HotTagCollection>(parameters);
        }
    }
}
