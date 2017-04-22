using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{
    /// <summary>
    /// Contains the details of a notes made on a photo.
    /// </summary>
    public sealed class PhotoNote : IFlickrParsable
    {

        public string NoteId { get; set; }
        

  

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "comment")
                UtilityMethods.CheckParsingException(reader);

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "id":
                        NoteId = reader.Value;
                        break;
                    
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();
            

            reader.Skip();
        }
    }
}
