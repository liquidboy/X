using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Samples
{

    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);
            
            if (val)
            {
                return (string)parameter + " Is ON";
            }

            return (string)parameter + " Is OFF";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
