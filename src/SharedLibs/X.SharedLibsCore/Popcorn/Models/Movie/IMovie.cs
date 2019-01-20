using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popcorn.Models.Movie
{
    public interface IMovie
    {
        string ImdbId { get; set; }

        bool IsFavorite { get; set; }

        bool HasBeenSeen { get; set; }

        string TranslationLanguage { get; set; }

        string Title { get; set; }
    }
}
