using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace X.UI.RichTab.Converters
{

    public class CleanFaviconConverter : IValueConverter
    {
   
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            var val = (string)value;

            if (val.Contains("azure"))
            {
                //twitter returns an svg from its favicon hence it breaks the image element as it doesn't support that
                return "http://www.windowsazure.com/favicon.ico?v2";
            }

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
