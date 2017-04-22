﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{
    /// <summary>
    /// The details of a referring domain. Used in the Flickr Stats API.
    /// </summary>
    public sealed class StatDomain : IFlickrParsable
    {
        /// <summary>
        /// The name of the referring domain.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The number of views from this domain.
        /// </summary>
        public int Views { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "domain")
                UtilityMethods.CheckParsingException(reader);

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "name":
                        Name = reader.Value;
                        break;
                    case "views":
                        Views = int.Parse(reader.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Skip();
        }
    }
}
