using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.Tiles
{
    public class TileRenderer : IContentRenderer
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

        }

        public void Load()
        {
            _renderElement = new TileEditor();
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public void UpdateSource(string uri)
        {
            
        }
    }
}
