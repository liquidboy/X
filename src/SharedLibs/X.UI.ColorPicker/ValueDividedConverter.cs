using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace X.UI.ColorPicker
{

    public class ValueDividedConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null) return 0;

            var val = (double)value;
            var divisor = double.Parse((string)parameter);

            if (divisor == 0) return 0;

            return val / divisor;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
