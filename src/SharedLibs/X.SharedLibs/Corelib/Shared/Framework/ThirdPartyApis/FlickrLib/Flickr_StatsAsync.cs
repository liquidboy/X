using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Get a list of referring domains for a collection.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <returns></returns>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetCollectionDomainsAsync(DateTime date)
        {
            return await StatsGetCollectionDomainsAsync(date, null, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for a collection.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="collectionId">The id of the collection to get stats for. If not provided, stats for all collections will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetCollectionDomainsAsync(DateTime date, string collectionId)
        {
            return await StatsGetCollectionDomainsAsync(date, collectionId, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for a collection.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetCollectionDomainsAsync(DateTime date, int page, int perPage)
        {
            return await StatsGetCollectionDomainsAsync(date, null, page, perPage);
        }

        /// <summary>
        /// Get a list of referring domains for a collection.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="collectionId">The id of the collection to get stats for. If not provided, stats for all collections will be returned.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetCollectionDomainsAsync(DateTime date, string collectionId, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getCollectionDomains");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            if (!String.IsNullOrEmpty(collectionId)) parameters.Add("collection_id", collectionId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatDomainCollection>(parameters);
        }

        /// <summary>
        /// Gets the collection of CSV files of archived stats from Flickr.
        /// </summary>
        /// <remarks>
        /// Archived files only available till the 1st June 2010.
        /// </remarks>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<CsvFileCollection>> StatsGetCsvFilesAsync()
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getCSVFiles");

            return await GetResponseAsync<CsvFileCollection>(parameters);
        }

        /// <summary>
        /// Get a list of referring domains for all photos.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotoDomainsAsync(DateTime date)
        {
            return await StatsGetPhotoDomainsAsync(date, null, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for a photo.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photoId">The id of the photo to get stats for. If not provided, stats for all photos will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotoDomainsAsync(DateTime date, string photoId)
        {
            return await StatsGetPhotoDomainsAsync(date, photoId, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for all photos.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotoDomainsAsync(DateTime date, int page, int perPage)
        {
            return await StatsGetPhotoDomainsAsync(date, null, page, perPage);
        }

        /// <summary>
        /// Get a list of referring domains for a photo.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photoId">The id of the photo to get stats for. If not provided, stats for all photos will be returned.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotoDomainsAsync(DateTime date, string photoId, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotoDomains");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            if (!String.IsNullOrEmpty(photoId)) parameters.Add("photo_id", photoId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatDomainCollection>(parameters);
        }

        /// <summary>
        /// Get a list of referring domains for a photostream.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotostreamDomainsAsync(DateTime date)
        {
            return await StatsGetPhotostreamDomainsAsync(date, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for a photostream.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotostreamDomainsAsync(DateTime date, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotostreamDomains");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatDomainCollection>(parameters);
        }

        /// <summary>
        /// Get a list of referring domains for a photoset.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotosetDomainsAsync(DateTime date)
        {
            return await StatsGetPhotosetDomainsAsync(date, null, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for a photoset.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photosetId">The id of the photoset to get stats for. If not provided, stats for all sets will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotosetDomainsAsync(DateTime date, string photosetId)
        {
            return await StatsGetPhotosetDomainsAsync(date, photosetId, 0, 0);
        }

        /// <summary>
        /// Get a list of referring domains for a photoset.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotosetDomainsAsync(DateTime date, int page, int perPage)
        {
            return await StatsGetPhotosetDomainsAsync(date, null, page, perPage);
        }

        /// <summary>
        /// Get a list of referring domains for a photoset.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photosetId">The id of the photoset to get stats for. If not provided, stats for all sets will be returned.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatDomainCollection>> StatsGetPhotosetDomainsAsync(DateTime date, string photosetId, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotosetDomains");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            if (!String.IsNullOrEmpty(photosetId)) parameters.Add("photoset_id", photosetId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatDomainCollection>(parameters);
        }

        /// <summary>
        /// Returns the number of views on the given date for the given collection. Only <see cref="Stats.Views"/> will be populated.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="collectionId">The collection to return stats for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Stats>> StatsGetCollectionStatsAsync(DateTime date, string collectionId, Action<FlickrResult<Stats>> callback)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getCollectionStats");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("collection_id", collectionId);

            return await GetResponseAsync<Stats>(parameters);
        }

        /// <summary>
        /// Returns the number of views, comments and favorites on the given date for the given photo.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="photoId">The photo to return stats for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Stats>> StatsGetPhotoStatsAsync(DateTime date, string photoId, Action<FlickrResult<Stats>> callback)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotoStats");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("photo_id", photoId);

            return await GetResponseAsync<Stats>(parameters);
        }

        /// <summary>
        /// Returns the number of views on the given date for the users photostream. Only <see cref="Stats.Views"/> will be populated.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Stats>> StatsGetPhotostreamStatsAsync(DateTime date, Action<FlickrResult<Stats>> callback)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotostreamStats");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));

            return await GetResponseAsync<Stats>(parameters);
        }

        /// <summary>
        /// Returns the number of views and comments on the given date for the given photoset. Only <see cref="Stats.Views"/> and <see cref="Stats.Comments"/> will be populated.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="photosetId">The photoset to return stats for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Stats>> StatsGetPhotosetStatsAsync(DateTime date, string photosetId, Action<FlickrResult<Stats>> callback)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotosetStats");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("photoset_id", photosetId);

            return await GetResponseAsync<Stats>(parameters);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photo.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotoReferrersAsync(DateTime date, string domain)
        {
            return await StatsGetPhotoReferrersAsync(date, domain, null, 0, 0);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photo.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photoId">The photo to return referrers for. If missing then referrers for all photos will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotoReferrersAsync(DateTime date, string domain, string photoId)
        {
            return await StatsGetPhotoReferrersAsync(date, domain, photoId, 0, 0);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photo.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotoReferrersAsync(DateTime date, string domain, int page, int perPage)
        {
            return await StatsGetPhotoReferrersAsync(date, domain, null, page, perPage);
        }


        /// <summary>
        /// Get a list of referrers from a given domain to a photo.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photoId">The photo to return referrers for. If missing then referrers for all photos will be returned.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotoReferrersAsync(DateTime date, string domain, string photoId, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotoReferrers");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("domain", domain);
            if (!String.IsNullOrEmpty(photoId)) parameters.Add("photo_id", photoId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatReferrerCollection>(parameters);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photoset.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotosetReferrersAsync(DateTime date, string domain)
        {
            return await StatsGetPhotosetReferrersAsync(date, domain, null, 0, 0);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photoset.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photosetId">The photoset to return referrers for. If missing then referrers for all photosets will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotosetReferrersAsync(DateTime date, string domain, string photosetId)
        {
            return await StatsGetPhotosetReferrersAsync(date, domain, photosetId, 0, 0);
        }


        /// <summary>
        /// Get a list of referrers from a given domain to a photoset.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotosetReferrersAsync(DateTime date, string domain, int page, int perPage)
        {
            return await StatsGetPhotosetReferrersAsync(date, domain, null, page, perPage);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photoset.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photosetId">The photoset to return referrers for. If missing then referrers for all photosets will be returned.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotosetReferrersAsync(DateTime date, string domain, string photosetId, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotosetReferrers");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("domain", domain);
            if (!String.IsNullOrEmpty(photosetId)) parameters.Add("photoset_id", photosetId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatReferrerCollection>(parameters);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a collection.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetCollectionReferrersAsync(DateTime date, string domain)
        {
            return await StatsGetCollectionReferrersAsync(date, domain, null, 0, 0);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a collection.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="collectionId">The collection to return referrers for. If missing then referrers for all photosets will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetCollectionReferrersAsync(DateTime date, string domain, string collectionId)
        {
            return await StatsGetCollectionReferrersAsync(date, domain, collectionId, 0, 0);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a collection.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetCollectionReferrersAsync(DateTime date, string domain, int page, int perPage)
        {
            return await StatsGetCollectionReferrersAsync(date, domain, null, page, perPage);
        }


        /// <summary>
        /// Get a list of referrers from a given domain to a collection.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="collectionId">The collection to return referrers for. If missing then referrers for all photosets will be returned.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetCollectionReferrersAsync(DateTime date, string domain, string collectionId, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getCollectionReferrers");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("domain", domain);
            if (!String.IsNullOrEmpty(collectionId)) parameters.Add("collection_id", collectionId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatReferrerCollection>(parameters);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photostream.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotostreamReferrersAsync(DateTime date, string domain)
        {
            return await StatsGetPhotostreamReferrersAsync(date, domain, 0, 0);
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photostream.
        /// </summary>
        /// <param name="date">The date to return stats for.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="page">The page of the results to return. Default is 1.</param>
        /// <param name="perPage">The number of referrers to return per page. The default is 25 and the maximum is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatReferrerCollection>> StatsGetPhotostreamReferrersAsync(DateTime date, string domain, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.stats.getPhotostreamReferrers");
            parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            parameters.Add("domain", domain);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<StatReferrerCollection>(parameters);
        }

        /// <summary>
        /// Get the overall view counts for an account.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatViews>> StatsGetTotalViewsAsync()
        {
            return await StatsGetTotalViewsAsync(DateTime.MinValue);
        }

        /// <summary>
        /// Get the overall view counts for an account on a given date.
        /// </summary>
        /// <param name="date">The date to return the overall view count for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<StatViews>> StatsGetTotalViewsAsync(DateTime date)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.stats.getTotalViews");
            if (date != DateTime.MinValue) parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));

            return await GetResponseAsync<StatViews>(parameters);
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PopularPhotoCollection>> StatsGetPopularPhotosAsync()
        {
            return await StatsGetPopularPhotosAsync(DateTime.MinValue, PopularitySort.None, 0, 0);
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PopularPhotoCollection>> StatsGetPopularPhotosAsync(DateTime date)
        {
            return await StatsGetPopularPhotosAsync(date, PopularitySort.None, 0, 0);
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// </summary>
        /// <param name="sort">The order in which to sort returned photos. Defaults to views. The possible values are views, comments and favorites. </param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PopularPhotoCollection>> StatsGetPopularPhotosAsync(PopularitySort sort)
        {
            return await StatsGetPopularPhotosAsync(DateTime.MinValue, sort, 0, 0);
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// </summary>
        /// <param name="sort">The order in which to sort returned photos. Defaults to views. The possible values are views, comments and favorites. </param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PopularPhotoCollection>> StatsGetPopularPhotosAsync(PopularitySort sort, int page, int perPage)
        {
            return await StatsGetPopularPhotosAsync(DateTime.MinValue, sort, page, perPage);
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PopularPhotoCollection>> StatsGetPopularPhotosAsync(DateTime date, int page, int perPage)
        {
            return await StatsGetPopularPhotosAsync(date, PopularitySort.None, page, perPage);
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <param name="sort">The order in which to sort returned photos. Defaults to views. The possible values are views, comments and favorites. </param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PopularPhotoCollection>> StatsGetPopularPhotosAsync(DateTime date, PopularitySort sort, int page, int perPage)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.stats.getPopularPhotos");
            if (date != DateTime.MinValue) parameters.Add("date", UtilityMethods.DateToUnixTimestamp(date));
            if (sort != PopularitySort.None) parameters.Add("sort", UtilityMethods.SortOrderToString(sort));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PopularPhotoCollection>(parameters);
        }

    }
}
