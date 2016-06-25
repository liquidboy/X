// Copyright (c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Globalization;
using Microsoft.Xbox.Music.Platform.Contract.DataModel;

namespace Microsoft.Xbox.Music.Platform.Client
{
    /// <summary>
    /// Artist, album and track content item extensions.
    /// </summary>
    public static class ContentExtensions
    {
        /// <summary>
        /// Get the content item's image URL. Optionally allows specifying resize parameters.
        /// </summary>
        /// <param name="content">An artist, album or track content item.</param>
        /// <param name="width">Image width, if set height must be set too.</param>
        /// <param name="height">Image height, if set width must be set too.</param>
        /// <returns>An image URL.</returns>
        public static string GetImageUrl(this Content content, int width = 0, int height = 0)
        {
            string imageUrl = content.ImageUrl;
            if (0 < width && 0 < height)
            {
                if (String.IsNullOrEmpty(imageUrl))
                {
                    return imageUrl;
                }
                return String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}w={2}&h={3}", imageUrl, imageUrl.Contains("?") ? "&" : "?", width, height);
            }
            if (0 < width || 0 < height)
            {
                throw new ArgumentException("width and height must both be set");
            }
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("width and height must be positive");
            }
            return imageUrl;
        }

        /// <summary>
        /// Content deep link actions.
        /// </summary>
        public enum LinkAction
        {
            /// <summary>
            /// Default. Currently launches the content details view.
            /// </summary>
            Default = 0,
            /// <summary>
            /// Launches the content details view.
            /// </summary>
            View,
            /// <summary>
            /// Launches playback of the media content.
            /// </summary>
            Play,
            /// <summary>
            /// Opens the "add to collection" screen on the Xbox Music service.
            /// </summary>
            AddToCollection,
            /// <summary>
            /// Opens the appropriate purchase flow on the Xbox Music service.
            /// </summary>
            Buy,
        }

        /// <summary>
        /// Get the content's deep linking URL. Optionaly allows specifying an action.
        /// </summary>
        /// <param name="content">An artist, album or track content item.</param>
        /// <param name="action">An action to take when the link opens the Xbox Music client.</param>
        /// <returns>The deep link.</returns>
        public static string GetLink(this Content content, LinkAction action = LinkAction.Default)
        {
            string link = content.Link;
            if (action == LinkAction.Default || String.IsNullOrEmpty(link))
            {
                return link;
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}action={2}", link, link.Contains("?") ? "&" : "?", action);
            }
        }
    }
}
