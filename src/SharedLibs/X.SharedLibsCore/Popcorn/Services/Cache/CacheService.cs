using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popcorn.Services.Cache
{
    public class CacheService : ICacheService
    {
        public CacheService(string localPath)
        {
            LocalPath = localPath;
        }

        /// <summary>
        /// Directory of assets
        /// </summary>
        public string Assets => $@"{LocalPath}\Assets\";

        /// <summary>
        /// Directory of downloaded movies
        /// </summary>
        public string MovieDownloads => $@"{LocalPath}\Downloads\Movies\";

        /// <summary>
        /// Directory of dropped files
        /// </summary>
        public string DropFilesDownloads => $@"{LocalPath}\Downloads\Dropped\";

        /// <summary>
        /// Directory of downloaded shows
        /// </summary>
        public string ShowDownloads => $@"{LocalPath}\Downloads\Shows\";

        /// <summary>
        /// Directory of downloaded movie torrents
        /// </summary>
        public string MovieTorrentDownloads => $@"{LocalPath}\Torrents\Movies\";

        /// <summary>
        /// Subtitles directory
        /// </summary>
        public string Subtitles => $@"{LocalPath}\Subtitles\";

        /// <summary>
        /// Popcorn temp directory
        /// </summary>
        public string LocalPath { get; }
    }
}
