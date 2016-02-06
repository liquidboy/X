using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;


namespace X.UI.LiteTab
{
    public class LiteTabSelectedThinknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isSelected = (bool)value;

            if (isSelected)
                return new Thickness(1, 1, 1, 0); 
            else
                return new Thickness(0, 0, 0, 1);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }
    }
}
