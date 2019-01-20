using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popcorn.Models.Torrent
{
    public interface ITorrent
    {
        int? Seeds { get; set; }

        int? Peers { get; set; }

        string Size { get; set; }

        string Url { get; set; }

        string Quality { get; set; }
    }
}
