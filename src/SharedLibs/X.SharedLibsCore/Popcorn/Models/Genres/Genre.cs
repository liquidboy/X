using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace Popcorn.Models.Genres
{
    public class GenreJson
    {
        [DataMember(Name = "EnglishName")]
        public string EnglishName { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }

    public class GenreResponse
    {
        [DataMember(Name = "genres")]
        public List<GenreJson> Genres { get; set; }
    }
}
