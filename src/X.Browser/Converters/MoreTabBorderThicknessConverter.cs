using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace X.Browser.Converters
{
    public class MoreTabBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isShowingMoreTab = (bool)value;

            if (isShowingMoreTab) return new Thickness(0, 0, 0, 0);

            return new Thickness(0, 0, 1, 0);

        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
