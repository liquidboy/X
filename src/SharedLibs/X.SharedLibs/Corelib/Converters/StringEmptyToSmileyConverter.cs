using System;
using Windows.UI.Xaml.Data;

namespace CoreLib.Converters
{
    public class StringEmptyToSmileyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = string.Empty;
            if (value == null) return ":)";

            if (string.IsNullOrEmpty(value.ToString()))
            {
                result = ":)";
            }
            else
            {
                result = value.ToString();
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}