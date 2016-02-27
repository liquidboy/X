using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;
namespace CoreLib.Converters
{
    public class StripHtmlTagsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = string.Empty;
            string html = value as string;
            result = Regex.Replace(html, @"<[^>]*>", String.Empty);
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
