﻿using System;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace FlickrNet
{

    /// <summary>
    /// The <see cref="PhotoPerson"/> class contains details returned by the <see cref="Flickr.PhotosPeopleGetList"/>
    /// method.
    /// </summary>
    public sealed class PhotoPerson : IFlickrParsable
    {
        /// <summary>The user id of the user.</summary>
        /// <remarks/>
        public string UserId { get; set; }

        /// <summary>The server that will serve up the users Buddy Icon.</summary>
        public string IconServer { get; set; }

        /// <summary>The server farm that will serve up the users Buddy Icon.</summary>
        public string IconFarm { get; set; }

        /// <summary>The users username, also known as their screenname.</summary>
        public string UserName { get; set; }

        /// <summary>The users real name, as entered in their profile.</summary>
        public string RealName { get; set; }

        /// <summary>
        /// The user ID of the person who added this person, to this photo.
        /// </summary>
        public string AddedByUserId { get; set; }

        /// <summary>
        /// The left most position of the persons bounding box, if any.
        /// </summary>
        public int? PositionX { get; set; }

        /// <summary>
        /// The top most position of the persons bounding box, if any.
        /// </summary>
        public int? PositionY { get; set; }

        /// <summary>
        /// The width of the persons bounding box, if any.
        /// </summary>
        public int? PositionWidth { get; set; }

        /// <summary>
        /// The height of the persons bounding box, if any.
        /// </summary>
        public int? PositionHeight { get; set; }

        /// <summary>
        /// The path alias for the users photostream, if they have set it.
        /// </summary>
        public string PathAlias { get; set; }

        /// <summary>
        /// The URL for the users Flickr home page.
        /// </summary>
        public string PhotostreamUrl
        {
            get
            {
                return "http://www.flickr.com/photos/" + (String.IsNullOrEmpty(PathAlias) ? UserId : PathAlias);
            }
        }
        /// <summary>
        /// Returns the <see cref="Uri"/> for the users Buddy Icon.
        /// </summary>
        public string BuddyIconUrl
        {
            get
            {
                if (String.IsNullOrEmpty(IconServer) || IconServer == "0")
                    return "http://www.flickr.com/images/buddyicon.jpg";
                else
                    return String.Format(System.Globalization.CultureInfo.InvariantCulture, "http://farm{0}.static.flickr.com/{1}/buddyicons/{2}.jpg", IconFarm, IconServer, UserId);
            }
        }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "id":
                    case "nsid":
                        UserId = reader.Value;
                        break;
                    case "iconserver":
                        IconServer = reader.Value;
                        break;
                    case "iconfarm":
                        IconFarm = reader.Value;
                        break;
                    case "username":
                        UserName = reader.Value;
                        break;
                    case "realname":
                        RealName = reader.Value;
                        break;
                    case "added_by":
                        AddedByUserId = reader.Value;
                        break;
                    case "path_alias":
                        PathAlias = reader.Value;
                        break;
                    case "x":
                        PositionX = reader.ReadContentAsInt();
                        break;
                    case "y":
                        PositionY = reader.ReadContentAsInt();
                        break;
                    case "w":
                        PositionWidth = reader.ReadContentAsInt();
                        break;
                    case "h":
                        PositionHeight = reader.ReadContentAsInt();
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();

        }
    }
}
