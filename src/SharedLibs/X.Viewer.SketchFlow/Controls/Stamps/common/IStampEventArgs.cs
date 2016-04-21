using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.Viewer.SketchFlow.Controls.Stamps
{
    interface IStampEventArgs
    {
        eActionTypes ActionType { get; set; }
    }
}
