using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Popcorn.Models.Movie
{
    public class MovieResponse
    {
        [DataMember(Name = "totalMovies")]
        public int TotalMovies { get; set; }

        [DataMember(Name = "movies")]
        public List<MovieJson> Movies { get; set; }
    }
}
