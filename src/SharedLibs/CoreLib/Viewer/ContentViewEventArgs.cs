using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Viewer
{
    public class ContentViewEventArgs : EventArgs
    {

        public string Type { get; set; }

        public Uri Uri { get; set; }
        public Uri CallingUri { get; set; }
        public Uri Source { get; set; }

        public string Favicon { get; set; }


 

    }
}
