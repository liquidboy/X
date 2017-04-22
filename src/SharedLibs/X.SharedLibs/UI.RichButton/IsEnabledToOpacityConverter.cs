using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace X.UI.RichButton
{

    public class IsEnabledToOpacityConverter : IValueConverter
    {
   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);


            if (val)
            {
                return 1;
            }

            return 0.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
