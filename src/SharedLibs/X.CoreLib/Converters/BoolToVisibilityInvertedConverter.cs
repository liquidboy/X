using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CoreLib.Converters
{
    public class BoolToVisibilityInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool temp = (bool)value;

            if (temp)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility temp = (Visibility)value;

            if (temp == Visibility.Collapsed)
                return true;
            else
                return false;

        }
    }
}
