using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{
    /// <summary>
    /// The timezone.
    /// </summary>
    public sealed class Timezone : IFlickrParsable
    {
        /// <summary>
        /// The Label of the timezone.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The offset of the timezone.
        /// </summary>
        public string Offset { get; set; }


        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "label":
                        Label = reader.Value;
                        break;
                    case "offset":
                        Offset = reader.Value;
                        break;
                }
            }

            reader.Skip();
        }
    }
}
