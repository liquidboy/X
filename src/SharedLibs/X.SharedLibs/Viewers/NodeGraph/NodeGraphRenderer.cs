using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace X.Viewer.NodeGraph
{
    public class NodeGraphRenderer : IContentRenderer
    {
        public FrameworkElement RenderElement { get; set; }

        public string Uri { get; set; }

        public event EventHandler<ContentViewEventArgs> SendMessage;

        public async Task CaptureThumbnail(InMemoryRandomAccessStream ms)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(RenderElement);

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

        }

        public void Load()
        {
            if (RenderElement == null) {
                RenderElement = new NodeGraphView();
                var cvre = RenderElement as IContentView;
                cvre.SendMessage += SendMessageThru;
                cvre.Load();
            }
        }

        public void Unload()
        {
            if (RenderElement != null) {
                var cvre = RenderElement as IContentView;
                cvre.SendMessage -= SendMessageThru;
                cvre.Unload();
                cvre = null;
                RenderElement = null;
            }
        }

        public void UpdateSource(string uri)
        {
            
        }

        public void SendMessageThru(object source, ContentViewEventArgs ea)
        {
            this.SendMessage?.Invoke(source, ea);
        }
    }
}
