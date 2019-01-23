using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popcorn.Services.Cache
{
    public class CacheService : ICacheService
    {
        public CacheService(string appPath)
        {
            _appPath = appPath;
        }

        /// <summary>
        /// Directory of assets
        /// </summary>
        public string Assets => $@"{LocalPath}\{_appPath}\Assets\";

        /// <summary>
        /// Directory of downloaded movies
        /// </summary>
        public string MovieDownloads => $@"{LocalPath}\{_appPath}\Downloads\Movies\";

        /// <summary>
        /// Directory of dropped files
        /// </summary>
        public string DropFilesDownloads => $@"{LocalPath}\{_appPath}\Downloads\Dropped\";

        /// <summary>
        /// Directory of downloaded shows
        /// </summary>
        public string ShowDownloads => $@"{LocalPath}\{_appPath}\Downloads\Shows\";

        /// <summary>
        /// Directory of downloaded movie torrents
        /// </summary>
        public string MovieTorrentDownloads => $@"{LocalPath}\{_appPath}\Torrents\Movies\";

        /// <summary>
        /// Subtitles directory
        /// </summary>
        public string Subtitles => $@"{LocalPath}\{_appPath}\Subtitles\";

        /// <summary>
        /// Popcorn temp directory
        /// </summary>
        public string LocalPath { get; set; }

        private string _appPath;
        
    }
}
