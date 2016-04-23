using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace X.Viewer.SketchFlow.Converters
{

    public class StampTypeToContentConverter : IValueConverter
    {

   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var data = (string)value;
            if (string.IsNullOrEmpty(data)) return null;

            //var nc = (FrameworkElement)Activator.CreateInstance((Type)value, new object[] { });
            //var nc = new Windows.UI.Xaml.Shapes.Path();
            
            string pthString = $"<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Data=\"{ data }\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Stretch\" Fill=\"DarkOrange\" Stroke=\"DarkOrange\" Stretch=\"Uniform\" />";
            var pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            return pth;
            
        }

     

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
