﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{

    /// <summary>
    /// Provides details of a specific blog, as configured by the user.
    /// </summary>
    public sealed class Blog : IFlickrParsable
    {
        /// <summary>
        /// The ID Flickr has assigned to the blog. Use this to post to the blog using 
        /// <see cref="Flickr.BlogsPostPhoto(string, string, string, string)"/> or 
        /// <see cref="Flickr.BlogsPostPhoto(string, string, string, string, string)"/>. 
        /// </summary>
        public string BlogId { get; set; }

        /// <summary>
        /// The name you have assigned to the blog in Flickr.
        /// </summary>
        public string BlogName { get; set; }

        /// <summary>
        /// The URL of the blog website.
        /// </summary>
        public string BlogUrl { get; set; }

        /// <summary>
        /// If Flickr stores the password for this then this will be 0, meaning you do not need to pass in the
        /// password when posting.
        /// </summary>
        public bool NeedsPassword { get; set; }

        /// <summary>
        /// The service that his blog uses. See <see cref="Flickr.BlogsGetServices()"/>
        /// </summary>
        public string Service { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "blog")
                UtilityMethods.CheckParsingException(reader);

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "id":
                        BlogId = reader.Value;
                        break;
                    case "name":
                        BlogName = reader.Value;
                        break;
                    case "url":
                        BlogUrl = reader.Value;
                        break;
                    case "needspassword":
                        NeedsPassword = reader.Value == "1";
                        break;
                    case "service":
                        Service = reader.Value;
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;

                }
            }
        }
    }
}
