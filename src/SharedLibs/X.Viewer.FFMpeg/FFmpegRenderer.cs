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
using X.Viewer.FFmpeg;

namespace X.Viewer
{
    public class FFmpegRenderer : IContentRenderer
    {
        //private FFmpegInteropMSS FFmpegMSS;
        Grid _renderElement;
        MediaElement _media;
        MediaControls _mediaControls;

        public FrameworkElement RenderElement
        {
            get
            {
                return _renderElement;
            }

            set
            {
                _renderElement = (Grid)value;
            }
        }

        public string Uri { get; set; }

        public event EventHandler<ContentViewEventArgs> SendMessage;


        public async Task CaptureThumbnail(Windows.Storage.Streams.InMemoryRandomAccessStream ms)
        {
            
        }

        public void Load()
        {
            _renderElement = new Grid();
            _media = new MediaElement() { Margin= new Thickness(0,0,0,80)};
            //_media.SetValue(Canvas.ZIndexProperty, 10);

            _mediaControls = new MediaControls() { HorizontalAlignment  = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Bottom, Height = 80 };
            //_mediaControls.SetValue(Canvas.ZIndexProperty, 11);
            _mediaControls.DataContext = _media;

            _renderElement.Children.Add(_media);
            _renderElement.Children.Add(_mediaControls);
        }

        public void Unload()
        {

            if(_media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Opening ||
                _media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Paused ||
                _media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing ||
                _media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Stopped ||
                _media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Buffering ||
                _media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Closed)
                _media?.Stop();
            

            _renderElement.Children.Remove(_media);

            _mediaControls.DataContext = null;
            _renderElement.Children.Remove(_mediaControls);

            
            _media = null;
            _mediaControls = null;
            _renderElement = null;
            

        }

        public async void UpdateSource(string uri)
        {
            //_renderElement.Source = uri;
            if (string.IsNullOrEmpty(uri)) return;

            _media.Stop();


            var uriToUse = uri;
            if (HasALocalDriveLetter(uri))
            {
                uriToUse = uriToUse.Replace("http://", string.Empty);
                uriToUse = uriToUse.Replace("https://", string.Empty);
            }


            if (uriToUse.Contains("http://") || uriToUse.Contains("https://")) {
                
                PropertySet options = new PropertySet();
                //FFmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromUri(uriToUse.ToString(), false, false, options);
            }
            else {
                var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(uriToUse);
                //var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync( new Uri(uriToUse));
                if (file != null)
                {
                    // Open StorageFile as IRandomAccessStream to be passed to FFmpegInteropMSS
                    IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

                    //FFmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromStream(readStream, false, false);
                }
            }
            
            //MediaStreamSource mss = FFmpegMSS?.GetMediaStreamSource();

            //if (mss != null)
            //{
            //    // Pass MediaStreamSource to Media Element
            //    _media.AutoPlay = false;
            //    _media.PlaybackRate = 3;
            //    _media.SetMediaStreamSource(mss);
            //}
            //else
            //{
            //    //Cannot open media
            //}
            
        }

        private bool HasALocalDriveLetter(string testString) {
            var re = new  System.Text.RegularExpressions.Regex("^(http|https)://[A-Za-z][:]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var match = re.Match(testString);

            return match.Success;
        }
    }
}
