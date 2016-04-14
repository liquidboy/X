using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.Viewer.SketchFlow.Controls.Stamps
{
    interface IStamp
    {
        string GenerateXAML(double scaleX, double scaleY, double left, double top);
        void PopulateFromUIElement(UIElement element);
    }
}
