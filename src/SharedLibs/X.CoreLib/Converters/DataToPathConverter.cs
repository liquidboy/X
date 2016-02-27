using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CoreLib.Converters
{
    public class DataToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string temp = (string)Application.Current.Resources[(string)value];
            string pthString = @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                Data=""" + temp + @""" />";
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            pth.Stretch = Stretch.Uniform;
            pth.Fill = new SolidColorBrush( Windows.UI.Colors.White);
            return pth;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
