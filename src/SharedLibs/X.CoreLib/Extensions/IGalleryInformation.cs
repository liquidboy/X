using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public interface IGalleryInformation
    {
        string GalleryApiUrl { get; set; }
        string Id { get; set; }
        string PublisherId { get; set; }
        string PublisherDisplayName { get; set; }
        DateTime Date { get; set; }
        
    }
}
