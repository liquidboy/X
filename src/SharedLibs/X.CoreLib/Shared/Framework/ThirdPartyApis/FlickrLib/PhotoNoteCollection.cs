using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlickrNet
{
    /// <summary>
    /// A list of <see cref="PhotoNote"/> items.
    /// </summary>
    public sealed class PhotoNoteCollection : ObservableCollection<PhotoNote>, IFlickrParsable
    {
        /// <summary>
        /// The ID of photo for these notes.
        /// </summary>
        public string PhotoId { get; set; }

        void IFlickrParsable.Load(XmlReader reader)
        {
            if (reader.LocalName != "notes")
                UtilityMethods.CheckParsingException(reader);

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "photo_id":
                        PhotoId = reader.Value;
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();

            while (reader.LocalName == "note")
            {
                PhotoNote note = new PhotoNote();
                ((IFlickrParsable)note).Load(reader);
                Add(note);
            }
            reader.Skip();
        }
    }
}
