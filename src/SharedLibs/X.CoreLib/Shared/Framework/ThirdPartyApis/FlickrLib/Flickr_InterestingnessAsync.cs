using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets a list of photos from the most recent interstingness list.
        /// </summary>
        /// <param name="perPage">Number of photos per page.</param>
        /// <param name="page">The page number to return.</param>
        /// <param name="extras"><see cref="PhotoSearchExtras"/> enumeration.</param>
        /// <returns><see cref="PhotoCollection"/> instance containing list of photos.</returns>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> InterestingnessGetListAsync(PhotoSearchExtras extras, int page, int perPage)
        {
            return await InterestingnessGetListAsync(DateTime.MinValue, extras, page, perPage);
        }

        /// <summary>
        /// Gets a list of photos from the interstingness list for the specified date.
        /// </summary>
        /// <param name="date">The date to return the interestingness list for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> InterestingnessGetListAsync(DateTime date)
        {
            return await InterestingnessGetListAsync(date, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets a list of photos from the most recent interstingness list.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> InterestingnessGetListAsync()
        {
            return await InterestingnessGetListAsync(DateTime.MinValue, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets a list of photos from the most recent interstingness list.
        /// </summary>
        /// <param name="date">The date to return the interestingness photos for.</param>
        /// <param name="extras">The extra parameters to return along with the search results.
        /// See <see cref="PhotoSearchOptions"/> for more details.</param>
        /// <param name="perPage">The number of results to return per page.</param>
        /// <param name="page">The page of the results to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> InterestingnessGetListAsync(DateTime date, PhotoSearchExtras extras, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.interestingness.getList");

            if (date > DateTime.MinValue) parameters.Add("date", date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (extras != PhotoSearchExtras.None)
                parameters.Add("extras", UtilityMethods.ExtrasToString(extras));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }
    }
}
