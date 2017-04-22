using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Imaging;

namespace X.UI.ColorPicker
{

    public class SwatchToImageUrlConverter : IValueConverter
    {

   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage (new Uri("ms-appx:///Assets/ColorSwatchCircle.png"));
        }

        private string UnescapeString(string escapedString) {
            var output = Regex.Replace(escapedString, @"\\[rnt]", m =>
            {
                switch (m.Value)
                {
                    case @"\r": return "\r";
                    case @"\n": return "\n";
                    case @"\t": return "\t";
                    default: return m.Value;
                }
            });
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
