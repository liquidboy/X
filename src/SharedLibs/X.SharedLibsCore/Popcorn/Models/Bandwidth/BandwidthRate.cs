using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popcorn.Models.Bandwidth
{
    public class BandwidthRate
    {
        public double DownloadRate { get; set; }
        public double UploadRate { get; set; }
        public TimeSpan ETA { get; set; }
    }
}
