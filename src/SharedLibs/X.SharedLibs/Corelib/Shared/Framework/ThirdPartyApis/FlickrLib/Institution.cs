﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{
    /// <summary>
    /// A Flickr Commons institution.
    /// </summary>
    public sealed class Institution : IFlickrParsable
    {
        /// <summary>
        /// The ID of the institution. Acts like a user ID for most method calls.
        /// </summary>
        public string InstitutionId { get; set; }

        /// <summary>
        /// The date this commons collection was launched.
        /// </summary>
        public DateTime DateLaunched { get; set; }

        /// <summary>
        /// The name of this commons institution.
        /// </summary>
        public string InstitutionName { get; set; }

        /// <summary>
        /// The URL to the institution's main site.
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// The URL to the institution's page on Flickr.
        /// </summary>
        public string FlickrUrl { get; set; }

        /// <summary>
        /// The URL to the institution's copyright/license page.
        /// </summary>
        public string LicenseUrl { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "nsid":
                        InstitutionId = reader.Value;
                        break;
                    case "date_launch":
                        DateLaunched = UtilityMethods.UnixTimestampToDate(reader.Value);
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                switch (reader.LocalName)
                {
                    case "name":
                        InstitutionName = reader.ReadElementContentAsString();
                        break;
                    case "urls":
                        reader.Read();
                        while (reader.LocalName == "url")
                        {
                            string type = reader.GetAttribute("type");
                            string url = reader.ReadElementContentAsString();
                            switch (type)
                            {
                                case "site":
                                    SiteUrl = url;
                                    break;
                                case "flickr":
                                    FlickrUrl = url;
                                    break;
                                case "license":
                                    LicenseUrl = url;
                                    break;
                            }
                        }
                        reader.Read();
                        break;
                }
            }

            reader.Read();
        }
    }
}
