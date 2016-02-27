using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace CoreLib.Converters
{
    /// <summary>
    /// Inverts a Boolean .
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);
            
            return val = !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
