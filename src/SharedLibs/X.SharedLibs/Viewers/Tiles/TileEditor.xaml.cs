using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace X.Viewer.Tiles
{
    public sealed partial class TileEditor : UserControl
    {
        public TileEditor()
        {
            this.InitializeComponent();
        }

        private async void butDoIt_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            TileLayouts layout = new TileLayouts();
            hiddenRenderingSurface.Children.Add(layout);

            await RenderUIElement(layout.FindName("tile310x310") as FrameworkElement, "tile310x310", 310, 310);
            await RenderUIElement(layout.FindName("tile310x150") as FrameworkElement, "tile310x150", 310, 150);
            

            hiddenRenderingSurface.Children.Remove(layout);

            layout = null;

        }

        private async Task RenderUIElement(FrameworkElement elm, string fileName, int width, int height) {
            
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));

            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(elm);

                var pixels = await renderTargetBitmap.GetPixelsAsync();

                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, ms);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    logicalDpi,
                    logicalDpi,
                    pixels.ToArray());

                await encoder.FlushAsync();

                await X.Services.Image.Service.Instance.GenerateResizedImageAsync(width, elm.ActualWidth, elm.ActualHeight, ms, fileName + ".png", Services.Image.Service.location.TileFolder, height);
            }

        }
    }
}
