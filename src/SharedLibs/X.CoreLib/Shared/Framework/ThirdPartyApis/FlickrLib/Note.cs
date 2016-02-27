using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{

    public sealed class Note : IFlickrParsable
    {
 
        public string NoteId { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "note")
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
        }
    }
}
