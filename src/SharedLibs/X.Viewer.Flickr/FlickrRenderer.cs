using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.CoreLib.GenericMessages;

namespace X.Viewer.Flickr
{
    public class FlickrRenderer : IContentRenderer
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
            _renderElement = new FlickrView();

            Messenger.Default.Register<LoadPhoto>(this, DoLoadPhoto);
            Messenger.Default.Register<LoadPhotoDetail>(this, DoLoadPhotoDetail);
        }

        private void DoLoadPhoto(LoadPhoto msg)
        {
            ((FlickrView)_renderElement).Photo = msg.Photo;
        }

        private void DoLoadPhotoDetail(LoadPhotoDetail msg)
        {
            ((FlickrView)_renderElement).User = msg.User;
        }

        public void Unload()
        {
            Messenger.Default.Unregister<LoadPhoto>(this, DoLoadPhoto);
            Messenger.Default.Unregister<LoadPhotoDetail>(this, DoLoadPhotoDetail);
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
