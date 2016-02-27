using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Return a list of unique namespaces, in alphabetical order.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NamespaceCollection>> MachineTagsGetNamespacesAsync()
        {
            return await MachineTagsGetNamespacesAsync(null, 0, 0);
        }

        /// <summary>
        /// Return a list of unique namespaces, in alphabetical order.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NamespaceCollection>> MachineTagsGetNamespacesAsync(int page, int perPage)
        {
            return await MachineTagsGetNamespacesAsync(null, page, perPage);
        }

        /// <summary>
        /// Return a list of unique namespaces, optionally limited by a given predicate, in alphabetical order.
        /// </summary>
        /// <param name="predicate">Limit the list of namespaces returned to those that have the following predicate.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NamespaceCollection>> MachineTagsGetNamespacesAsync(string predicate)
        {
            return await MachineTagsGetNamespacesAsync(predicate, 0, 0);
        }

        /// <summary>
        /// Return a list of unique namespaces, optionally limited by a given predicate, in alphabetical order.
        /// </summary>
        /// <param name="predicate">Limit the list of namespaces returned to those that have the following predicate.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NamespaceCollection>> MachineTagsGetNamespacesAsync(string predicate, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.machinetags.getNamespaces");
            if (!String.IsNullOrEmpty(predicate)) parameters.Add("predicate", predicate);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<NamespaceCollection>(parameters);

        }

        /// <summary>
        /// Return a list of unique predicates, in alphabetical order.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PredicateCollection>> MachineTagsGetPredicatesAsync()
        {
            return await MachineTagsGetPredicatesAsync(null, 0, 0);
        }

        /// <summary>
        /// Return a list of unique predicates, in alphabetical order.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of namespaces to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PredicateCollection>> MachineTagsGetPredicatesAsync(int page, int perPage)
        {
            return await MachineTagsGetPredicatesAsync(null, page, perPage);
        }

        /// <summary>
        /// Return a list of unique predicates, optionally limited by a given namespace, in alphabetical order.
        /// </summary>
        /// <param name="namespaceName">Limit the list of predicates returned to those that have the following namespace.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PredicateCollection>> MachineTagsGetPredicatesAsync(string namespaceName)
        {
            return await MachineTagsGetPredicatesAsync(namespaceName, 0, 0);
        }

        /// <summary>
        /// Return a list of unique predicates, optionally limited by a given namespace, in alphabetical order.
        /// </summary>
        /// <param name="namespaceName">Limit the list of predicates returned to those that have the following namespace.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of namespaces to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PredicateCollection>> MachineTagsGetPredicatesAsync(string namespaceName, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.machinetags.getPredicates");
            if (!String.IsNullOrEmpty(namespaceName)) parameters.Add("namespace", namespaceName);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PredicateCollection>(parameters);
        }

        /// <summary>
        /// Return a list of unique namespace and predicate pairs, in alphabetical order.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PairCollection>> MachineTagsGetPairsAsync()
        {
            return await MachineTagsGetPairsAsync(null, null, 0, 0);
        }

        /// <summary>
        /// Return a list of unique namespace and predicate pairs, in alphabetical order.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of pairs to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PairCollection>> MachineTagsGetPairsAsync(int page, int perPage)
        {
            return await MachineTagsGetPairsAsync(null, null, page, perPage);
        }

        /// <summary>
        /// Return a list of unique namespace and predicate pairs, optionally limited by predicate or namespace, in alphabetical order.
        /// </summary>
        /// <param name="namespaceName">Limit the list of pairs returned to those that have the following namespace.</param>
        /// <param name="predicate">Limit the list of pairs returned to those that have the following predicate.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PairCollection>> MachineTagsGetPairsAsync(string namespaceName, string predicate)
        {
            return await MachineTagsGetPairsAsync(namespaceName, predicate, 0, 0);
        }

        /// <summary>
        /// Return a list of unique namespace and predicate pairs, optionally limited by predicate or namespace, in alphabetical order.
        /// </summary>
        /// <param name="namespaceName">Limit the list of pairs returned to those that have the following namespace.</param>
        /// <param name="predicate">Limit the list of pairs returned to those that have the following predicate.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of pairs to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PairCollection>> MachineTagsGetPairsAsync(string namespaceName, string predicate, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.machinetags.getPairs");
            if (!String.IsNullOrEmpty(namespaceName)) parameters.Add("namespace", namespaceName);
            if (!String.IsNullOrEmpty(predicate)) parameters.Add("predicate", predicate);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<PairCollection>(parameters);
        }

        /// <summary>
        /// Return a list of unique values for a namespace and predicate.
        /// </summary>
        /// <param name="namespaceName">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetValuesAsync(string namespaceName, string predicate)
        {
            return await MachineTagsGetValuesAsync(namespaceName, predicate, 0, 0);
        }

        /// <summary>
        /// Return a list of unique values for a namespace and predicate.
        /// </summary>
        /// <param name="namespaceName">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of values to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetValuesAsync(string namespaceName, string predicate, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.machinetags.getValues");
            parameters.Add("namespace", namespaceName);
            parameters.Add("predicate", predicate);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<ValueCollection>(parameters);
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// </summary>
        /// <param name="addedSince">Only return machine tags values that have been added since this timestamp.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetRecentValuesAsync(DateTime addedSince)
        {
            return await MachineTagsGetRecentValuesAsync(null, null, addedSince, 0, 0);
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// </summary>
        /// <param name="addedSince">Only return machine tags values that have been added since this timestamp.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of values to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetRecentValuesAsync(DateTime addedSince, int page, int perPage)
        {
            return await MachineTagsGetRecentValuesAsync(null, null, addedSince, page, perPage);
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// </summary>
        /// <param name="namespaceName">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetRecentValuesAsync(string namespaceName, string predicate)
        {
            return await MachineTagsGetRecentValuesAsync(namespaceName, predicate, DateTime.MinValue, 0, 0);
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// </summary>
        /// <param name="namespaceName">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of values to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetRecentValuesAsync(string namespaceName, string predicate, int page, int perPage)
        {
            return await MachineTagsGetRecentValuesAsync(namespaceName, predicate, DateTime.MinValue, page, perPage);
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// </summary>
        /// <param name="namespaceName">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="addedSince">Only return machine tags values that have been added since this timestamp.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of values to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<ValueCollection>> MachineTagsGetRecentValuesAsync(string namespaceName, string predicate, DateTime addedSince, int page, int perPage)
        {
            if (String.IsNullOrEmpty(namespaceName) && String.IsNullOrEmpty(predicate) && addedSince == DateTime.MinValue)
            {
                throw new ArgumentException("Must supply one of namespaceName, predicate or addedSince");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.machinetags.getRecentValues");
            if (!String.IsNullOrEmpty(namespaceName)) parameters.Add("namespace", namespaceName);
            if (!String.IsNullOrEmpty(predicate)) parameters.Add("predicate", predicate);
            if (addedSince != DateTime.MinValue) parameters.Add("added_since", UtilityMethods.DateToUnixTimestamp(addedSince));
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<ValueCollection>(parameters);
        }

    }
}
