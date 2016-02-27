using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Framework.Controls.Explosions
{
    public interface IExplosion
    {
        void Explode(double durationInSeconds, double x, double y, int randomColorCode, Action callbackOnComplete);
        void SetMargin(Windows.UI.Xaml.Thickness margin);
        void SetHorizontalAlignment(Windows.UI.Xaml.HorizontalAlignment align);
        void SetVerticalAlignment(Windows.UI.Xaml.VerticalAlignment align);
    }
}
