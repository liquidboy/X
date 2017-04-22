using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using X.Viewer.SketchFlow;

namespace X.Viewer.SketchFlow
{
    public class SketchFlowRenderer : IContentRenderer
    {
        FrameworkElement _renderElement;

        public FrameworkElement RenderElement
        {
            get
            {
                return _renderElement;
            }

            set
            {
                _renderElement = value;
            }
        }

        public string Uri { get; set; }

        public event EventHandler<ContentViewEventArgs> SendMessage;

        public async Task CaptureThumbnail(InMemoryRandomAccessStream ms)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(_renderElement);

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
            if (_renderElement == null) {
                _renderElement = new SketchView();
                var cvre = _renderElement as IContentView;
                cvre.SendMessage += SendMessageThru;
                cvre.Load();
            }
        }

        public void Unload()
        {
            if (_renderElement != null) {
                var cvre = _renderElement as IContentView;
                cvre.SendMessage -= SendMessageThru;
                cvre.Unload();
                cvre = null;
                _renderElement = null;
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
