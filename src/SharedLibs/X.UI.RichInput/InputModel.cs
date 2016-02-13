using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace X.UI.RichInput
{
    class InputModel : DependencyObject
    {

        public object Data { get; set; }

        public Color FocusColor { get; set; }

        public Color FocusHoverColor { get; set; }

        public Color FocusForegroundColor { get; set; }


    }
}
