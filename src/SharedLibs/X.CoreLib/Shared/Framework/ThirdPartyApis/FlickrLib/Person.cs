using System;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace FlickrNet
{

    /// <summary>
    /// The <see cref="Person"/> class contains details returned by the <see cref="Flickr.PeopleGetInfo"/>
    /// method.
    /// </summary>
    public sealed class Person : IFlickrParsable
    {
        /// <summary>The user id of the user.</summary>
        /// <remarks/>
        public string UserId { get; set; }
    
        /// <summary>Does the user posses a pro account.
        /// 0 = free acouunt, 1 = pro account holder.</summary>
        public bool IsPro { get; set; }
    
        /// <summary>The server that will serve up the users Buddy Icon.</summary>
        public string IconServer { get; set; }

        /// <summary>The server farm that will serve up the users Buddy Icon.</summary>
        public string IconFarm { get; set; }

        /// <summary>The gender of the user on Flickr. May be null, or X for unspecified.</summary>
        public string Gender { get; set; }

        /// <summary>
        /// Is the person ignored by the calling user. Will be null if not an authenticated call.
        /// </summary>
        public bool? IsIgnored { get; set; }

        /// <summary>
        /// The timezone of the person
        /// </summary>
        public Timezone Timezone { get; set; }

        /// <summary>
        /// Is the person a contact of the calling user. Will be null if not an authenticated call.
        /// </summary>
        public bool? IsContact { get; set; }

        /// <summary>
        /// Is the person a friend of the calling user. Will be null if not an authenticated call.
        /// </summary>
        public bool? IsFriend { get; set; }

        /// <summary>
        /// Is the person family of the calling user. Will be null if not an authenticated call.
        /// </summary>
        public bool? IsFamily { get; set; }

        /// <summary>
        /// Has the person marked the calling user as a contact.  Will be null if not an authenticated call.
        /// </summary>
        public bool? IsReverseContact { get; set; }

        /// <summary>
        /// Has the person marked the calling user as a friend.  Will be null if not an authenticated call.
        /// </summary>
        public bool? IsReverseFriend { get; set; }

        /// <summary>
        /// Has the person marked the calling user as family.  Will be null if not an authenticated call.
        /// </summary>
        public bool? IsReverseFamily { get; set; }

        /// <summary>The users username, also known as their screenname.</summary>
        public string UserName { get; set; }

        /// <summary>The users description.</summary>
        public string Description { get; set; }

        /// <summary>The users real name, as entered in their profile.</summary>
        public string RealName { get; set; }

        
        public string DateCreated { get; set; }

        /// <summary>Does the user posses a pro account.
        /// 0 = free acouunt, 1 = pro account holder.</summary>
        public bool CanBuyPro { get; set; }

		public bool HasStats { get; set; }

        public bool Expire { get; set; }


        /// <summary>The SHA1 hash of the users email address - used for FOAF networking.</summary>
        public string MailboxSha1Hash { get; set; }
    
        /// <summary>Consists of your current location followed by country.</summary>
        /// <example>e.g. Newcastle, UK.</example>
        public string Location { get; set; }

        /// <summary>Sub element containing a summary of the users photo information.</summary>
        /// <remarks/>
        public PersonPhotosSummary PhotosSummary { get; set; }

        /// <summary>
        /// The users URL alias, if any.
        /// </summary>
        public string PathAlias { get; set; }

        /// <summary>
        /// The users photo location on Flickr
        /// http://www.flickr.com/photos/username/
        /// </summary>
        public string PhotosUrl { get; set; }

        /// <summary>
        /// The users profile location on Flickr
        /// http://www.flickr.com/people/username/
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        /// The users profile location on Flickr
        /// http://m.flickr.com/photostream.gne?id=ID
        /// </summary>
        public string MobileUrl { get; set; }

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
            LoadAttributes(reader);

            LoadElements(reader);

        }

        private void LoadElements(System.Xml.XmlReader reader)
        {
            while (reader.LocalName != "person")
            {
                switch (reader.LocalName)
                {
                    case "timezone":
                        Timezone = new Timezone();
                        ((IFlickrParsable)Timezone).Load(reader);
                        break;
                    case "username":
                        UserName = reader.ReadElementContentAsString();
                        break;
                    case "location":
                        Location = reader.ReadElementContentAsString();
                        break;
                    case "realname":
                        RealName = reader.ReadElementContentAsString();
                        break;
                    case "photosurl":
                        PhotosUrl = reader.ReadElementContentAsString();
                        break;
                    case "profileurl":
                        ProfileUrl = reader.ReadElementContentAsString();
                        break;
                    case "description":
                        Description = reader.ReadElementContentAsString();
                        break;
                    case "mobileurl":
                        MobileUrl = reader.ReadElementContentAsString();
                        break;
                    case "photos":
                        PhotosSummary = new PersonPhotosSummary();
                        ((IFlickrParsable)PhotosSummary).Load(reader);
                        break;
                    case "mbox_sha1sum":
                        MailboxSha1Hash = reader.ReadElementContentAsString();
                        break;
                    case "datecreate":
                        DateCreated = reader.ReadElementContentAsString();
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        reader.Skip();
                        break;
                }
            }
        }

        private void LoadAttributes(System.Xml.XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "id":
                    case "nsid":
                        UserId = reader.Value;
                        break;
                    case "ispro":
                        IsPro = reader.Value == "1";
                        break;
                    case "iconserver":
                        IconServer = reader.Value;
                        break;
                    case "iconfarm":
                        IconFarm = reader.Value;
                        break;
                    case "path_alias":
                        PathAlias = reader.Value;
                        break;
                    case "gender":
                        Gender = reader.Value;
                        break;
                    case "ignored":
                        IsIgnored = reader.Value == "1";
                        break;
                    case "contact":
                        IsContact = reader.Value == "1";
                        break;
                    case "friend":
                        IsFriend = reader.Value == "1";
                        break;
                    case "family":
                        IsFamily = reader.Value == "1";
                        break;
                    case "revcontact":
                        IsReverseContact = reader.Value == "1";
                        break;
                    case "revfriend":
                        IsReverseFriend = reader.Value == "1";
                        break;
                    case "revfamily":
                        IsReverseFamily = reader.Value == "1";
                        break;
                    case "datecreate":
                        DateCreated = reader.Value;
                        break;
                    case "can_buy_pro":
                        CanBuyPro = reader.Value == "1";
                        break;
					case "has_stats":
						HasStats = reader.Value == "1";
                        break;
                    case "expire":
                        Expire = reader.Value == "1";
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();
        }
    }

    /// <summary>
    /// A summary of a users photos.
    /// </summary>
    public sealed class PersonPhotosSummary : IFlickrParsable
    {
        /// <summary>The first date the user uploaded a picture, converted into <see cref="DateTime"/> format.</summary>
        public DateTime FirstDate { get; set; }

        /// <summary>The first date the user took a picture, converted into <see cref="DateTime"/> format.</summary>
        public DateTime FirstTakenDate { get; set; }

        /// <summary>The total number of photos for the user.</summary>
        /// <remarks/>
        public int PhotoCount { get; set; }

        /// <summary>The total number of photos for the user.</summary>
        /// <remarks/>
        public int Views { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            reader.Read();

            while (reader.LocalName != "photos")
            {
                switch (reader.LocalName)
                {
                    case "firstdatetaken":
                        FirstTakenDate = UtilityMethods.ParseDateWithGranularity(reader.ReadElementContentAsString());
                        break;
                    case "firstdate":
                        FirstDate = UtilityMethods.UnixTimestampToDate(reader.ReadElementContentAsString());
                        break;
                    case "count":
                        PhotoCount = reader.ReadElementContentAsInt();
                        break;
                    case "views":
                        Views = reader.ReadElementContentAsInt();
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        reader.Skip();
                        break;
                }
            }

            reader.Read();
        }
    }
}
