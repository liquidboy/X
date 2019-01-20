using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Popcorn.Models.Image
{
    public class ImageShowJson
    {
        [DataMember(Name = "poster")]
        public string Poster { get; set; }

        [DataMember(Name = "banner")]
        public string Banner { get; set; }
    }
}
