using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CoreLib.Converters
{
    public class SurroundStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var test = (string)parameter;
            var returnString = (string)value;
            switch (test)
            {
                case "quotes": returnString  =  "\"" + returnString +"\""; break;
                case "brackets": returnString = "(" + returnString + ")"; break;
            }

            return returnString;
            
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
