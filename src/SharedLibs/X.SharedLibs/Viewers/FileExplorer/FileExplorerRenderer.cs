//using FFmpegInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.FileExplorer
{
    public class FileExplorerRenderer : IContentRenderer
    {
        public FrameworkElement RenderElement { get; set; }

        public string Uri { get; set; }

        public event EventHandler<ContentViewEventArgs> SendMessage;


        public async Task CaptureThumbnail(Windows.Storage.Streams.InMemoryRandomAccessStream ms)
        {
            
        }

        public void Load()
        {
            RenderElement = new Explorer();
           
        }

        public void Unload()
        {
            RenderElement = null;
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
