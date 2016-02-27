using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.UI.AddTab
{

    public class LoadCompletedEventArgs : EventArgs
    {
        public double ActualWidth { get; set; }
        public double ActualHeight { get; set; }
        public string SearchQuery { get; set; }
    }
}
