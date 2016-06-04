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
            var grd = new Grid();
            foreach (var layer in layers.Where(x=>x.IsEnabled)) {
                
                var xaml = "";
                foreach (var xamlFragment in layer.XamlFragments.Where(x=>x.IsEnabled)) {
                    xaml += xamlFragment.Xaml;
                }

                var nsXaml = string.Empty;
                if (layer.HasChildContainerCanvas) nsXaml = $"<Canvas>{xaml}</Canvas>";
                else nsXaml = xaml;

                var nsTemplate = $"<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" HorizontalAlignment=\"Stretch\" xmlns:xuip=\"using:X.UI.Path\" xmlns:lc=\"using:X.Viewer.SketchFlow.Controls\" xmlns:lcs=\"using:X.Viewer.SketchFlow.Controls.Stamps\" VerticalAlignment=\"Stretch\" >{nsXaml}</Grid>";
                //todo: create then inject the converter "ExtensionToImageSourceConverter"  <-- after much fucking around i added it to apps root resource dict ... wont work if injected in..

                if (xaml.Length > 0) { 
                    var xamlFe = (FrameworkElement)XamlReader.Load(UnescapeString(nsTemplate));
                    grd.Children.Add(xamlFe);
                }
            }

            if (grd.Children.Count > 0) return grd;

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
