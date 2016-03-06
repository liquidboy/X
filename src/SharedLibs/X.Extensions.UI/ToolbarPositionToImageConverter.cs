using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace X.Extensions.UI
{
    public class ToolbarPositionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var toolbarPosition = (string)value;

            var positions = "";
            
            if (toolbarPosition.Contains(ExtensionInToolbarPositions.Left.ToString())) {
                positions += "left";
            }
            if (toolbarPosition.Contains(ExtensionInToolbarPositions.Top.ToString()))
            {
                positions += "top";
            }
            if (toolbarPosition.Contains(ExtensionInToolbarPositions.Right.ToString()))
            {
                positions += "right";
            }
            if (toolbarPosition.Contains(ExtensionInToolbarPositions.Bottom.ToString()))
            {
                if (toolbarPosition.Contains(ExtensionInToolbarPositions.BottomFull.ToString()))
                    positions += "bottom-full";
                else 
                    positions += "bottom";
            }
            

            if (positions.Length == 0) return "ms-appx:///X.Extensions.UI/Assets/Extensions/tb-none.png";
            else return "ms-appx:///X.Extensions.UI/Assets/Extensions/tb-" + positions + ".png";

        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
