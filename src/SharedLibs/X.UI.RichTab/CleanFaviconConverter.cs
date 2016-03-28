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

            if (val.Contains("twitter"))
            {
                //twitter returns an svg from its favicon hence it breaks the image element as it doesn't support that
                return "ms-appx:///assets/favicon/twitter.png";
            }

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
