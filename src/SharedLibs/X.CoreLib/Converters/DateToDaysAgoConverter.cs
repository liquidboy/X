using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CoreLib.Converters
{
    public class DateToDaysAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime dateTime = (DateTime)value;

                var dateTimeDiff = DateTime.Now.Subtract(dateTime);


                if (dateTimeDiff.TotalDays < 1)
                    if (dateTimeDiff.TotalHours < 1)
                        return dateTimeDiff.TotalMinutes.ToString("#0") + " mins old";
                    else
                        return dateTimeDiff.TotalHours.ToString("#0") + " hours old";
                else
                    return dateTimeDiff.TotalDays.ToString("###") + " days old";
            }
            catch {
                return string.Empty;
            }
            
            
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
