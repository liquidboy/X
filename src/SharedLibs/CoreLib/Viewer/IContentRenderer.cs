using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.Viewer
{
    public interface IContentRenderer
    {
        FrameworkElement RenderElement { get; set; }
        event EventHandler<ContentViewEventArgs> SendMessage;

        string Uri { get; set; }

        void Load();
        void Unload();
        void UpdateSource(string uri);

        Task CaptureThumbnail(Windows.Storage.Streams.InMemoryRandomAccessStream ms);
        
    }
}
