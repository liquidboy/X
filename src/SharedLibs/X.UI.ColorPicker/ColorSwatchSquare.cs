using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace X.UI.ColorPicker
{
    public enum eSwatchTypes {
        Circle,
        Square1,
        Square2
    }

    public sealed class ColorSwatchSquare : Control
    {
        //references: 
        //(a) https://msdn.microsoft.com/library/windows/apps/xaml/windows.ui.xaml.media.imaging.writeablebitmap.pixelbuffer.aspx
        //(b) https://social.msdn.microsoft.com/Forums/sqlserver/en-US/ab86a816-ee11-49f1-bd37-9cee2b8f48f4/uwphow-to-access-the-pixel-data-of-an-image?forum=wpdevelop
        //(c) http://stackoverflow.com/questions/30542505/c-sharp-how-do-i-convert-my-get-getpixel-setpixel-color-processing-to-lockbits

        Image _colorImage;
        Ellipse _ellipsePixel;
        RenderTargetBitmap _rtb;
        IBuffer _pixelBuffer;
        Byte[] _pixelBufferData;

        Canvas _main;
        Rectangle _rectSelectedColor;
        TextBlock _tbData;
        Boolean IsMouseDown = false;

        public ColorSwatchSquare()
        {
            this.DefaultStyleKey = typeof(ColorSwatchSquare);



            this.Loaded += ColorSwatchSquare_Loaded;
            this.Unloaded += ColorSwatchSquare_Unloaded;
        }

        protected override async void OnApplyTemplate()
        {
            if (_main == null) {
                _main = GetTemplateChild("grdMain") as Canvas;
                _main.PointerPressed += _main_PointerPressed;
                _main.PointerReleased += _main_PointerReleased;
                _main.PointerMoved += _main_PointerMoved;

                _colorImage = GetTemplateChild("imgSwatch") as Image;
                _colorImage.ImageOpened += _colorImage_ImageOpened;
                
                _rectSelectedColor = GetTemplateChild("rectSelectedColor") as Rectangle;
                _ellipsePixel = GetTemplateChild("ellipsePixel") as Ellipse;
                _tbData = GetTemplateChild("tbData") as TextBlock; 
            }

            SetSwatch();

            base.OnApplyTemplate();
        }

        private async void _colorImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            _rtb = new RenderTargetBitmap();
            await _rtb.RenderAsync(_colorImage);
            _pixelBuffer = await _rtb.GetPixelsAsync();
            _pixelBufferData = _pixelBuffer.ToArray();
        }

        private void _main_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if(IsMouseDown) UpdateColor(e.GetCurrentPoint(_main));

            e.Handled = true;
        }

        private void _main_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsMouseDown = false;

            e.Handled = true;
        }

        private void _main_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsMouseDown = true;

            e.Handled = true;
        }

        private void ColorSwatchSquare_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_main != null) {
                _main.PointerPressed -= _main_PointerPressed;
                _main.PointerReleased -= _main_PointerReleased;
                _main.PointerMoved -= _main_PointerMoved;

                _colorImage.ImageOpened -= _colorImage_ImageOpened;

                _pixelBuffer = null;
                _pixelBufferData = null;
                _rtb = null;
            }
        }

        private async void ColorSwatchSquare_Loaded(object sender, RoutedEventArgs e)
        {
            SetSwatch();
        }

        private void SetSwatch() {
            if (_colorImage != null)
            {
                if (Swatch == eSwatchTypes.Circle)
                {
                    _colorImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/ColorSwatchCircle.png"));
                }
                else if (Swatch == eSwatchTypes.Square1)
                {
                    _colorImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/ColorSwatchSquare1.png"));
                }
                else if (Swatch == eSwatchTypes.Square2)
                {
                    _colorImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/ColorSwatchSquare2.png"));
                }
            }
        }


        private void UpdateColor(PointerPoint pointer)
        {
            // Test to ensure we do not get bad mouse positions along the edges
            int imageX = (int)pointer.Position.X;
            int imageY = (int)pointer.Position.Y;
            if ((imageX < 0) || (imageY < 0) || (imageX > _colorImage.Width - 1) || (imageY > _colorImage.Height - 1)) return;
            
            // Update the mouse cursor position and the Selected Color
            double x = (double)(pointer.Position.X - (_ellipsePixel.Width / 2.0));
            double y = (double)(pointer.Position.Y - (_ellipsePixel.Width / 2.0));
           
            //ref (c)
            var bytesPerPixel = 4;
            int maxPointerLenght = _rtb.PixelWidth * _rtb.PixelHeight * bytesPerPixel;
            int stride = (int)_colorImage.Width * bytesPerPixel;
            byte r, g, b, a;

            int certainPixel = bytesPerPixel * imageX + stride * imageY;

            b = _pixelBufferData[certainPixel + 0];
            g = _pixelBufferData[certainPixel + 1];
            r = _pixelBufferData[certainPixel + 2];
            a = _pixelBufferData[certainPixel + 3];
            
            _rectSelectedColor.Fill = new SolidColorBrush(
                Color.FromArgb(a, r, g, b));

            _tbData.Text = $"x:{imageX} y:{imageY}  r:{r} g:{g} b:{b}   ";

            _ellipsePixel.SetValue(Canvas.LeftProperty, x);
            _ellipsePixel.SetValue(Canvas.TopProperty, y);



        }



        public eSwatchTypes Swatch
        {
            get { return (eSwatchTypes)GetValue(SwatchProperty); }
            set { SetValue(SwatchProperty, value); }
        }

        public static readonly DependencyProperty SwatchProperty =
            DependencyProperty.Register("Swatch", typeof(eSwatchTypes), typeof(ColorSwatchSquare), new PropertyMetadata(eSwatchTypes.Circle));


    }
}
