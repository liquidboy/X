using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
namespace SumoNinjaMonkey.Framework.Controls
{
    public class Clip
    {
        public static readonly DependencyProperty ToBoundsProperty = DependencyProperty.RegisterAttached("ToBounds", typeof(bool), typeof(Clip), new PropertyMetadata(0, new PropertyChangedCallback(Clip.OnToBoundsPropertyChanged)));
        public static bool GetToBounds(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(Clip.ToBoundsProperty);
        }
        public static void SetToBounds(DependencyObject depObj, bool clipToBounds)
        {
            depObj.SetValue(Clip.ToBoundsProperty, clipToBounds);
        }
        private static void OnToBoundsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = d as FrameworkElement;
            if (frameworkElement != null)
            {
                Clip.ClipToBounds(frameworkElement);
                frameworkElement.Loaded += Clip.fe_Loaded;
                frameworkElement.SizeChanged += Clip.fe_SizeChanged;
    
            }
        }
        private static void ClipToBounds(FrameworkElement fe)
        {
            if (Clip.GetToBounds(fe))
            {
                RectangleGeometry rectangleGeometry = new RectangleGeometry();
                rectangleGeometry.Rect = new Rect(0.0, 0.0, fe.ActualWidth, fe.ActualHeight);
                fe.Clip = rectangleGeometry;
                return;
            }
            fe.Clip = null;
        }
        private static void fe_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip.ClipToBounds(sender as FrameworkElement);
        }
        private static void fe_Loaded(object sender, RoutedEventArgs e)
        {
            Clip.ClipToBounds(sender as FrameworkElement);
        }
    }
}
