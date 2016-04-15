using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace X.Viewer.SketchFlow.Converters
{

    public class UIElementAsClipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var root = (FrameworkElement)value;
            return new Rect(0,0, root.Width, root.Height) ;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
