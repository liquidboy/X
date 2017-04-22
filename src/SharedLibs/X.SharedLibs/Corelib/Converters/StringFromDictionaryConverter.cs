using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CoreLib.Converters
{
    public class StringFromDictionaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var key = (string)parameter;

            //if(value is IList<Models.AWSTag>)
            //{
            //    var dict = (IList<Models.AWSTag>)value;
            //    try
            //    {
            //        return dict.Where(x => x.Key == key).Select(x => x.Value)?.FirstOrDefault();
            //    }
            //    catch
            //    {
            //        return string.Empty;
            //    }

            //}


            return string.Empty;

        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
