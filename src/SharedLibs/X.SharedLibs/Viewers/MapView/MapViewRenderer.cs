using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.MapView
{
    public class MapViewRenderer : IContentRenderer
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
            _renderElement = new MapLayer();
        }

        public void Unload()
        {
            _renderElement = null;
        }

        public void UpdateSource(string uri)
        {
            
        }

        public void SendMessageThru(object source, ContentViewEventArgs ea)
        {
            throw new NotImplementedException();
        }
    }
}
