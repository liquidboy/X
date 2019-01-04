using Microsoft.Graphics.Canvas;
using System;
using Windows.UI.Xaml.Data;

namespace X.Viewer.NodeGraph.Converters
{
    public class CanvasAlphaModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueAsDouble = value == null ? 0d : (double)value;

            int.TryParse(valueAsDouble.ToString(), out int canvasAlphaModeValue);

            var mode = (CanvasAlphaMode)canvasAlphaModeValue;

            return mode.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
