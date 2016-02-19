using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;


namespace X.UI.UserCard
{
    public class RightToFlowDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isOnRight = (bool)value;

            if (isOnRight)
                return FlowDirection.LeftToRight;
            else
                return FlowDirection.RightToLeft;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }
    }
}
