using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Samples
{

    public class BooleanToStringConverter : IValueConverter
    {
        public bool IsOnOff { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);

            if (val)
            {
                return (string)parameter + (IsOnOff ? " Is ON" : " is True");
            }

            return (string)parameter + (IsOnOff ? " Is OFF" : " is False");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
