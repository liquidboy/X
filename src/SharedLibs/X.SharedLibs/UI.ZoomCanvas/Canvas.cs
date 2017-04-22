using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace X.UI.ZoomCanvas
{
    public sealed class Canvas : ContentControl
    {
        private Windows.UI.Xaml.Controls.Canvas _cvMainContainer;
        
        public double ZoomIntensity { get; set; } = 0.05d;
        public double Scale { get; set; } = 1;

        public Canvas()
        {
            this.DefaultStyleKey = typeof(Canvas);


            this.Loaded += Canvas_Loaded;
            this.Unloaded += Canvas_Unloaded;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
         
        }
        private void Canvas_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_cvMainContainer == null)
            {
                _cvMainContainer = GetTemplateChild("cvMainContainer") as Windows.UI.Xaml.Controls.Canvas;
                _cvMainContainer.Children.Add( (UIElement) this.Content);
            }


        }


        public void Zoom(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var mousePoint = e.GetCurrentPoint(null).Position;
            var wheel = e.GetCurrentPoint(null).Properties.MouseWheelDelta / 120;
            var zoom = Math.Exp(wheel * ZoomIntensity);

            _zoom(zoom, mousePoint.X, mousePoint.Y);
        }

        private void _zoom(double s, double x, double y)
        {
            //line0.Text = $"scale : {s} x : {x} y : {y}";
            var ct = _cvMainContainer.RenderTransform as CompositeTransform;
            Point worldPos = new Point((x - ct.TranslateX) / ct.ScaleX, (y - ct.TranslateY) / ct.ScaleY);
            Point newScale = new Point(ct.ScaleX * s, ct.ScaleY * s);

            Point newScreenPos = new Point((worldPos.X) * newScale.X + ct.TranslateX, (worldPos.Y) * newScale.Y + ct.TranslateY);

            ct.TranslateX -= (newScreenPos.X - x);
            ct.TranslateY -= (newScreenPos.Y - y);
            ct.ScaleX = newScale.X;
            ct.ScaleY = newScale.Y;

            Scale = newScale.Y;

        }

    }
}
