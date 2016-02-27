using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CoreLib.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            object result;
            try
            {
                if (((string)value).StartsWith("/"))
                {
                    result = new BitmapImage(new Uri("ms-resource:" + (string)value));
                }
                else
                {
                    result = new BitmapImage(new Uri((string)value));
                }
            }
            catch (Exception ex)
            {
                result = new BitmapImage();
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
