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
        /// Get a list of current 'Pandas' supported by Flickr.
        /// </summary>
        /// <returns>An array of panda names.</returns>
        public async Task<FlickrResult<string[]>> PandaGetListAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.panda.getList");

            var r = await GetResponseAsync<UnknownResponse>(parameters);
                
            FlickrResult<string[]> result = new FlickrResult<string[]>();
            result.HasError = r.HasError;
            if (r.HasError)
            {
                result.Error = r.Error;
            }
            else
            {
                result.Result = r.Result.GetElementArray("panda");
            }
            return result;
            

        }

        /// <summary>
        /// Gets a list of photos for the given panda.
        /// </summary>
        /// <param name="pandaName">The name of the panda to return photos for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PandaPhotoCollection>> PandaGetPhotosAsync(string pandaName)
        {
            return await PandaGetPhotosAsync(pandaName, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets a list of photos for the given panda.
        /// </summary>
        /// <param name="pandaName">The name of the panda to return photos for.</param>
        /// <param name="extras">The extras to return with the photos.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PandaPhotoCollection>> PandaGetPhotosAsync(string pandaName, PhotoSearchExtras extras)
        {
            return await PandaGetPhotosAsync(pandaName, extras, 0, 0);
        }

        /// <summary>
        /// Gets a list of photos for the given panda.
        /// </summary>
        /// <param name="pandaName">The name of the panda to return photos for.</param>
        /// <param name="perPage">The number of photos to return per page.</param>
        /// <param name="page">The age to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PandaPhotoCollection>> PandaGetPhotosAsync(string pandaName, int page, int perPage)
        {
            return await PandaGetPhotosAsync(pandaName, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Gets a list of photos for the given panda.
        /// </summary>
        /// <param name="pandaName">The name of the panda to return photos for.</param>
        /// <param name="extras">The extras to return with the photos.</param>
        /// <param name="perPage">The number of photos to return per page.</param>
        /// <param name="page">The age to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PandaPhotoCollection>> PandaGetPhotosAsync(string pandaName, PhotoSearchExtras extras, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.panda.getPhotos");
            parameters.Add("panda_name", pandaName);
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PandaPhotoCollection>(parameters);
        }
    }
}
