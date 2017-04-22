using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace X.UI.MoreTab
{

    public class IsPinnedToImageUriConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);
         
            if (val)
            {
                return "ms-appx:///Assets/Pinned-sel.png";
            }

            return "ms-appx:///Assets/Pinned-nor.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
