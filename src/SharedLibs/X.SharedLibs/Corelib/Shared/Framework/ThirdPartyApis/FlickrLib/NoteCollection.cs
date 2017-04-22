using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace FlickrNet
{

    public sealed class NoteCollection : Collection<Note>, IFlickrParsable
    {
        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "notes")
                UtilityMethods.CheckParsingException(reader);

            reader.Read();

            while (reader.LocalName == "note")
            {
                Note n = new Note();
                ((IFlickrParsable)n).Load(reader);
                Add(n);
            }

            reader.Skip();
        }
    }
}