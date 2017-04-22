using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CoreLib.Converters
{

    public class ValueDividedConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (double)value;
            var divisor = (double)parameter;

            return val / divisor;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
