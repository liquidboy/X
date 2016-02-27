using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.UI.Toolbar
{
    public class ToolbarRoutedEventArgs : RoutedEventArgs
    {

        public Guid UniqueGuid { get; set; }
    }
}
