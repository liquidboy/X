using System;
using Windows.UI.Xaml.Data;
namespace CoreLib.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = string.Empty;
            if (parameter != null)
            {
                string a;
                if ((a = parameter.ToString()) != null)
                {
                    if (a == "TotalMinutes")
                    {
                        result = string.Format("Total Time : approx. {0} mns", new object[]
						{
							value
						});
                        return result;
                    }
                    if (a == "ActiveMinutes")
                    {
                        result = string.Format("Active time : {0} mns", new object[]
						{
							value
						});
                        return result;
                    }
                    if (a == "YieldNumber")
                    {
                        result = string.Format("Yield : {0} persons", new object[]
						{
							value
						});
                        return result;
                    }

                    return string.Format(a, value);
                }
                result = value.ToString();
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
