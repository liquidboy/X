using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Popcorn.Models.Torrent.Show
{
    public class TorrentShowNodeJson
    {
        [DataMember(Name = "0")]
        public TorrentShowJson Torrent_0 { get; set; }

        [DataMember(Name = "480p")]
        public TorrentShowJson Torrent_480p { get; set; }

        [DataMember(Name = "720p")]
        public TorrentShowJson Torrent_720p { get; set; }

        [DataMember(Name = "1080p")]
        public TorrentShowJson Torrent_1080p { get; set; }
    }
}
