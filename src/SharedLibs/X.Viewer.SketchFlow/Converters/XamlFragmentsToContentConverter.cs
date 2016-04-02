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

    public class XamlFragmentsToContentConverter : IValueConverter
    {

   
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var layers = (IList<PageLayer>)value;
            
            foreach (var layer in layers.Where(x=>x.IsEnabled)) {
                
                var xaml = "";
                foreach (var xamlFragment in layer.XamlFragments) {
                    xaml += xamlFragment;
                }

                var nsXaml = $"<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Stretch\" > {xaml} </Grid>";
                
                if (xaml.Length > 0) { 
                    var xamlFe = (FrameworkElement)XamlReader.Load(UnescapeString(nsXaml));
                    return xamlFe;
                }
            }
            
            return null;
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
