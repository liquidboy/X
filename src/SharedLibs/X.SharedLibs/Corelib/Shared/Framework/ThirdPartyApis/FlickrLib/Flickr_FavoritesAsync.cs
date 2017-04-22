using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Adds a photo to the logged in favourites.
        /// Requires authentication.
        /// </summary>
        /// <param name="photoId">The id of the photograph to add.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> FavoritesAddAsync(string photoId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.favorites.add");
            parameters.Add("photo_id", photoId);
            return await GetResponseAsync<NoResponse>(parameters);
        }



        /// <summary>
        /// Removes a photograph from the logged in users favourites.
        /// Requires authentication.
        /// </summary>
        /// <param name="photoId">The id of the photograph to remove.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> FavoritesRemoveAsync(string photoId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.favorites.remove");
            parameters.Add("photo_id", photoId);
            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Get a list of the currently logger in users favourites.
        /// Requires authentication.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync()
        {
            return await FavoritesGetListAsync(null, DateTime.MinValue, DateTime.MinValue, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Get a list of the currently logger in users favourites.
        /// Requires authentication.
        /// </summary>
        /// <param name="extras">The extras to return for each photo.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync(PhotoSearchExtras extras)
        {
            return await FavoritesGetListAsync(null, DateTime.MinValue, DateTime.MinValue, extras, 0, 0);
        }

        /// <summary>
        /// Get a list of the currently logger in users favourites.
        /// Requires authentication.
        /// </summary>
        /// <param name="perPage">Number of photos to include per page.</param>
        /// <param name="page">The page to download this time.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync(int page, int perPage)
        {
            return await FavoritesGetListAsync(null, DateTime.MinValue, DateTime.MinValue, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Get a list of the currently logger in users favourites.
        /// Requires authentication.
        /// </summary>
        /// <param name="extras">The extras to return for each photo.</param>
        /// <param name="perPage">Number of photos to include per page.</param>
        /// <param name="page">The page to download this time.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync(PhotoSearchExtras extras, int page, int perPage)
        {
            return await FavoritesGetListAsync(null, DateTime.MinValue, DateTime.MinValue, extras, page, perPage);
        }

        /// <summary>
        /// Get a list of favourites for the specified user.
        /// </summary>
        /// <param name="userId">The user id of the user whose favourites you wish to retrieve.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync(string userId)
        {
            return await FavoritesGetListAsync(userId, DateTime.MinValue, DateTime.MinValue, PhotoSearchExtras.IconServer, 0, 0);
        }

        /// <summary>
        /// Get a list of favourites for the specified user.
        /// </summary>
        /// <param name="userId">The user id of the user whose favourites you wish to retrieve.</param>
        /// <param name="extras">The extras to return for each photo.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync(string userId, PhotoSearchExtras extras)
        {
            return await FavoritesGetListAsync(userId, DateTime.MinValue, DateTime.MinValue, extras, 0, 0);
        }

        /// <summary>
        /// Get a list of favourites for the specified user.
        /// </summary>
        /// <param name="userId">The user id of the user whose favourites you wish to retrieve.</param>
        /// <param name="minFavoriteDate">Minimum date that a photo was favorited on.</param>
        /// <param name="maxFavoriteDate">Maximum date that a photo was favorited on. </param>
        /// <param name="extras">The extras to return for each photo.</param>
        /// <param name="perPage">Number of photos to include per page.</param>
        /// <param name="page">The page to download this time.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetListAsync(string userId, DateTime minFavoriteDate, DateTime maxFavoriteDate, PhotoSearchExtras extras, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.favorites.getList");
            if (userId != null) parameters.Add("user_id", userId);
            if (minFavoriteDate != DateTime.MinValue) parameters.Add("min_fav_date", UtilityMethods.DateToUnixTimestamp(minFavoriteDate));
            if (maxFavoriteDate != DateTime.MinValue) parameters.Add("max_fav_date", UtilityMethods.DateToUnixTimestamp(maxFavoriteDate));
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Gets the public favourites for a specified user.
        /// </summary>
        /// <remarks>This function difers from <see cref="Flickr.FavoritesGetList(string)"/> in that the user id 
        /// is not optional.</remarks>
        /// <param name="userId">The is of the user whose favourites you wish to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetPublicListAsync(string userId)
        {
            return await FavoritesGetPublicListAsync(userId, DateTime.MinValue, DateTime.MinValue, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets the public favourites for a specified user.
        /// </summary>
        /// <remarks>This function difers from <see cref="Flickr.FavoritesGetList(string)"/> in that the user id 
        /// is not optional.</remarks>
        /// <param name="userId">The is of the user whose favourites you wish to return.</param>
        /// <param name="minFavoriteDate">Minimum date that a photo was favorited on.</param>
        /// <param name="maxFavoriteDate">Maximum date that a photo was favorited on. </param>
        /// <param name="extras">The extras to return for each photo.</param>
        /// <param name="perPage">The number of photos to return per page.</param>
        /// <param name="page">The specific page to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> FavoritesGetPublicListAsync(string userId, DateTime minFavoriteDate, DateTime maxFavoriteDate, PhotoSearchExtras extras, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.favorites.getPublicList");
            parameters.Add("user_id", userId);
            if (minFavoriteDate != DateTime.MinValue) parameters.Add("min_fav_date", UtilityMethods.DateToUnixTimestamp(minFavoriteDate));
            if (maxFavoriteDate != DateTime.MinValue) parameters.Add("max_fav_date", UtilityMethods.DateToUnixTimestamp(maxFavoriteDate));
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }

        /// <summary>
        /// Get the next and previous favorites in a users list of favorites, based on one of their favorites.
        /// </summary>
        /// <param name="photoId">The photo id of the photo for which to find the next and previous favorites.</param>
        /// <param name="userId">The user id of the users whose favorites you wish to search.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        /// <returns></returns>
        public async Task<FlickrResult<FavoriteContext>> FavoritesGetContextAsync(string photoId, string userId)
        {
            return await FavoritesGetContextAsync(photoId, userId, 1, 1, PhotoSearchExtras.None);
        }

        /// <summary>
        /// Get the next and previous favorites in a users list of favorites, based on one of their favorites.
        /// </summary>
        /// <param name="photoId">The photo id of the photo for which to find the next and previous favorites.</param>
        /// <param name="userId">The user id of the users whose favorites you wish to search.</param>
        /// <param name="extras">Any extras to return for each photo in the previous and next list.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        /// <returns></returns>
        public async Task<FlickrResult<FavoriteContext>> FavoritesGetContextAsync(string photoId, string userId, PhotoSearchExtras extras)
        {
            return await FavoritesGetContextAsync(photoId, userId, 1, 1, extras);
        }

        /// <summary>
        /// Get the next and previous favorites in a users list of favorites, based on one of their favorites.
        /// </summary>
        /// <param name="photoId">The photo id of the photo for which to find the next and previous favorites.</param>
        /// <param name="userId">The user id of the users whose favorites you wish to search.</param>
        /// <param name="numPrevious">The number of previous favorites to list. Defaults to 1.</param>
        /// <param name="numNext">The number of next favorites to list. Defaults to 1.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        /// <returns></returns>
        public async Task<FlickrResult<FavoriteContext>> FavoritesGetContextAsync(string photoId, string userId, int numPrevious, int numNext)
        {
            return await FavoritesGetContextAsync(photoId, userId, numPrevious, numNext, PhotoSearchExtras.None);
        }


        /// <summary>
        /// Get the next and previous favorites in a users list of favorites, based on one of their favorites.
        /// </summary>
        /// <param name="photoId">The photo id of the photo for which to find the next and previous favorites.</param>
        /// <param name="userId">The user id of the users whose favorites you wish to search.</param>
        /// <param name="numPrevious">The number of previous favorites to list. Defaults to 1.</param>
        /// <param name="numNext">The number of next favorites to list. Defaults to 1.</param>
        /// <param name="extras">Any extras to return for each photo in the previous and next list.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        /// <returns></returns>
        public async Task<FlickrResult<FavoriteContext>> FavoritesGetContextAsync(string photoId, string userId, int numPrevious, int numNext, PhotoSearchExtras extras)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.favorites.getContext");
            parameters.Add("user_id", userId);
            parameters.Add("photo_id", photoId);
            parameters.Add("num_prev", Math.Max(1, numPrevious).ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("num_next", Math.Max(1, numNext).ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));

            return await GetResponseAsync<FavoriteContext>(parameters);
        }

    }
}
