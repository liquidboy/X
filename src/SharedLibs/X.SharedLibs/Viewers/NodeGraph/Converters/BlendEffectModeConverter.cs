using Microsoft.Graphics.Canvas.Effects;
using System;
using Windows.UI.Xaml.Data;

namespace X.Viewer.NodeGraph.Converters
{
    public class BlendEffectModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueAsDouble = value == null? 0d: (double)value;

            int.TryParse(valueAsDouble.ToString(), out int blendEffectModeValue);

            var mode = (BlendEffectMode)blendEffectModeValue;

            return mode.ToString() ;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
