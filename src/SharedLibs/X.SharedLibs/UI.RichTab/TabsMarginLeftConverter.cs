using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace X.UI.RichTab.Converters
{

    public class TabsMarginLeftConverter : IValueConverter
    {
   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToInt16(value);

            return new Thickness(val * 40, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
