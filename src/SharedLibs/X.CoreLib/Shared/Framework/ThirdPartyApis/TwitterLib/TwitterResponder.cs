using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Linq;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using System.Collections;

namespace FlickrNet
{
    /// <summary>
    /// Flickr library interaction with the web goes in here.
    /// </summary>
    public static partial class TwitterResponder
    {

        private const string PostContentType = "application/x-www-form-urlencoded";
        private const string GetContentType = "application/json";


        /// <summary>
        /// Returns the string for the Authorisation header to be used for OAuth authentication.
        /// Parameters other than OAuth ones are ignored.
        /// </summary>
        /// <param name="parameters">OAuth and other parameters.</param>
        /// <returns></returns>
        public static string OAuthCalculateAuthHeader(Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder("OAuth ");

            var sorted = parameters.OrderBy(x => x.Key);

            foreach (KeyValuePair<string, string> pair in sorted)
            {
                if (pair.Key.StartsWith("oauth"))
                {
                    sb.Append(pair.Key + "=\"" + Uri.EscapeDataString(pair.Value) + "\",");
                }
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// Calculates for form encoded POST data to be included in the body of an OAuth call.
        /// </summary>
        /// <remarks>This will include all non-OAuth parameters. The OAuth parameter will be included in the Authentication header.</remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string OAuthCalculatePostData(Dictionary<string, string> parameters)
        {
            string data = String.Empty;
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                if (!pair.Key.StartsWith("oauth") && !pair.Key.StartsWith("method"))
                {
                    data += pair.Key + "=" + UtilityMethods.EscapeOAuthString(pair.Value) + "&";
                }
            }

            string ret = data.Length > 1 ? data.Remove(data.Length - 1, 1).ToString() : data;
            return ret;
        }

    }
}

