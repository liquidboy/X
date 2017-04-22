using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace X.UI.MoreTab
{

    public class LastRefreshedDateConverter : IValueConverter
    {
        
   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToDateTime(value);
            
            if (val.Equals(DateTime.MinValue)) return "Never";
            else
            {
                var dateAgo = "{0} ago";
                var diff = DateTime.Now.Subtract(val.ToLocalTime());

                if (diff.TotalMinutes < 1)
                    dateAgo = string.Format(dateAgo, Math.Round(diff.TotalSeconds,0) + " sec's ");
                else if (diff.TotalMinutes >= 60)
                    dateAgo = string.Format(dateAgo, Math.Round(diff.TotalHours,2) + " hr's ");
                else
                    dateAgo = string.Format(dateAgo, Math.Round(diff.TotalMinutes,0) + " min's ");

                return dateAgo;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
