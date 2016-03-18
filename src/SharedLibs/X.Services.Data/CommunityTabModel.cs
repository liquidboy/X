using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class CommunityTabModel
    {
        public string Id { get; set; }
        public string Uid { get; set; }
        public string Grouping { get; set; }
        public string DisplayTitle { get; set; }
        public string Uri { get; set; }
        public string FaviconUri { get; set; }
        public string CreatorAvatarUri { get; set; }
        public bool HasFocus { get; set; }
    }
}
