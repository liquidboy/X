using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Returns a list of recent activity on photos belonging to the calling user.
        /// </summary>
        /// <remarks>
        /// <b>Do not poll this method more than once an hour.</b>
        /// </remarks>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ActivityItemCollection>> ActivityUserPhotosAsync()
        {
            return await ActivityUserPhotosAsync(null, 0, 0);
        }

        /// <summary>
        /// Returns a list of recent activity on photos belonging to the calling user.
        /// </summary>
        /// <remarks>
        /// <b>Do not poll this method more than once an hour.</b>
        /// </remarks>
        /// <param name="page">The page numver of the activity to return.</param>
        /// <param name="perPage">The number of activities to return per page.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ActivityItemCollection>> ActivityUserPhotosAsync(int page, int perPage)
        {
            return await ActivityUserPhotosAsync(null, page, perPage);
        }

        /// <summary>
        /// Returns a list of recent activity on photos belonging to the calling user.
        /// </summary>
        /// <remarks>
        /// <b>Do not poll this method more than once an hour.</b>
        /// </remarks>
        /// <param name="timePeriod">The number of days or hours you want to get activity for.</param>
        /// <param name="timeType">'d' for days, 'h' for hours.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ActivityItemCollection>> ActivityUserPhotosAsync(int timePeriod, string timeType)
        {
            return await ActivityUserPhotosAsync(timePeriod, timeType, 0, 0);
        }

        /// <summary>
        /// Returns a list of recent activity on photos belonging to the calling user.
        /// </summary>
        /// <remarks>
        /// <b>Do not poll this method more than once an hour.</b>
        /// </remarks>
        /// <param name="timePeriod">The number of days or hours you want to get activity for.</param>
        /// <param name="timeType">'d' for days, 'h' for hours.</param>
        /// <param name="page">The page numver of the activity to return.</param>
        /// <param name="perPage">The number of activities to return per page.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ActivityItemCollection>> ActivityUserPhotosAsync(int timePeriod, string timeType, int page, int perPage)
        {
            if (timePeriod == 0)
                throw new ArgumentOutOfRangeException("timePeriod", "Time Period should be greater than 0");

            if (timeType == null)
                throw new ArgumentNullException("timeType");

            if (timeType != "d" && timeType != "h")
                throw new ArgumentOutOfRangeException("timeType", "Time type must be 'd' or 'h'");

            return await ActivityUserPhotosAsync(timePeriod + timeType, page, perPage);
        }

        private async Task<FlickrResult<ActivityItemCollection>> ActivityUserPhotosAsync(string timeframe, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.activity.userPhotos");
            if (timeframe != null && timeframe.Length > 0) parameters.Add("timeframe", timeframe);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<ActivityItemCollection>(parameters);
        }

        /// <summary>
        /// Returns a list of recent activity on photos commented on by the calling user.
        /// </summary>
        /// <remarks>
        /// <b>Do not poll this method more than once an hour.</b>
        /// </remarks>
        /// <param name="page">The page of the activity to return.</param>
        /// <param name="perPage">The number of activities to return per page.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ActivityItemCollection>> ActivityUserCommentsAsync(int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.activity.userComments");
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<ActivityItemCollection>(parameters);
        }


    }
}
