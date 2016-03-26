using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WeakEvent;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using X.Viewer;

namespace X.UI.AddTab
{
    public sealed class Tab : Control
    {
        ContentView _cvMain;
        ProgressRing _prLoading;
        TextBox _tbSearchUrl;
        TextBlock _tbText;
        Button _butAddTab;
        

        private readonly WeakEventSource<EventArgs> _DoSearchSource = new WeakEventSource<EventArgs>();
        private readonly WeakEventSource<EventArgs> _LoadCompletedSource = new WeakEventSource<EventArgs>();



        public event EventHandler<EventArgs> DoSearch
        {
            add { _DoSearchSource.Subscribe(value); }
            remove { _DoSearchSource.Unsubscribe(value); }
        }
        public event EventHandler<EventArgs> LoadCompleted
        {
            add { _LoadCompletedSource.Subscribe(value); }
            remove { _LoadCompletedSource.Unsubscribe(value); }
        }



        public Tab()
        {
            this.DefaultStyleKey = typeof(Tab);

            this.Loaded += Tab_Loaded;
            this.Unloaded += Tab_Unloaded;
        }

        private void Tab_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_cvMain != null) {
                _cvMain.SendMessage -= _cvMain_SendMessage;
            }

            if (_prLoading != null)
            {
                _prLoading.IsActive = false;
            }

            if (_tbSearchUrl != null)
            {
                _tbSearchUrl.KeyUp -= _tbSearchUrl_KeyUp;
            }

            this.Loaded -= Tab_Loaded;
            this.Unloaded -= Tab_Unloaded;
            
        }

        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {

            if (_cvMain == null) {
                _cvMain = GetTemplateChild("cvMain") as ContentView;
                _cvMain.SendMessage += _cvMain_SendMessage;
            }
            
            if (_prLoading == null)
            {
                _prLoading = GetTemplateChild("prLoading") as ProgressRing;
            }

            if (_tbSearchUrl  == null)
            {
                _tbSearchUrl = GetTemplateChild("tbSearchUrl") as TextBox;
                _tbSearchUrl.KeyUp += _tbSearchUrl_KeyUp;
            }

            if (_tbText == null)
            {
                _tbText = GetTemplateChild("tbText") as TextBlock;
            }

            if (_butAddTab == null)
            {
                _butAddTab = GetTemplateChild("butAddTab") as Button;
            }
            


            base.OnApplyTemplate();
        }

        private void _tbSearchUrl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                _DoSearchSource?.Raise(this, new DoSearchEventArgs() { SearchQuery = _tbSearchUrl.Text });
            

            if (_tbSearchUrl.Text.Length > 0) _tbText.Visibility = Visibility.Collapsed;
            else _tbText.Visibility = Visibility.Visible;

            if (_tbSearchUrl.Text.Length == 0)
                _DoSearchSource?.Raise(this, new DoSearchEventArgs() { SearchQuery = "http://www.msn.com/spartan/ntp?dpir=1" });
            
        }

        public void CloseFlyout() {
            _butAddTab.Flyout.Hide();
        }

        public void FocusOnSearchBox() {
            _tbSearchUrl?.Focus(Windows.UI.Xaml.FocusState.Pointer);
        }


        private async void _cvMain_SendMessage(object sender, ContentViewEventArgs e)
        {
            var cv = (ContentViewEventArgs)e;
            if (cv.Type == "LoadCompleted")
            {
                if (cv.Source == null || cv.Source.OriginalString.Length == 0) return;

                _LoadCompletedSource?.Raise(this, new LoadCompletedEventArgs() { ActualHeight = _cvMain.ActualHeight, ActualWidth = _cvMain.ActualWidth, SearchQuery = cv.Source.OriginalString } );
                
                var uriHash = FlickrNet.UtilityMethods.MD5Hash(cv.Source.OriginalString); //   e.Uri.OriginalString);

                await Task.Delay(1000);

                //capture screenshot
                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {

                    await _cvMain.Renderer?.CaptureThumbnail(ms);

                    //img: Banner 400 width
                    //ms.Seek(0);
                    await X.Services.Image.Service.Instance.GenerateResizedImageAsync(400, _cvMain.ActualWidth, _cvMain.ActualHeight, ms, uriHash + ".png", X.Services.Image.Service.location.MediumFolder);
                    
                    //img: Thumbnail
                    ms.Seek(0);
                    await X.Services.Image.Service.Instance.GenerateResizedImageAsync(180, _cvMain.ActualWidth, _cvMain.ActualHeight, ms, uriHash + ".png", X.Services.Image.Service.location.ThumbFolder);

                    //img: Tile
                    ms.Seek(0);
                    await X.Services.Image.Service.Instance.GenerateResizedImageAsync(71, _cvMain.ActualWidth, _cvMain.ActualHeight, ms, uriHash + ".png", X.Services.Image.Service.location.TileFolder, 71);

                    ms.Seek(0);
                    await X.Services.Image.Service.Instance.GenerateResizedImageAsync(150, _cvMain.ActualWidth, _cvMain.ActualHeight, ms, uriHash + "-150x150.png", X.Services.Image.Service.location.TileFolder, 150);

                    ms.Seek(0);
                    await X.Services.Image.Service.Instance.GenerateResizedImageAsync(310, _cvMain.ActualWidth, _cvMain.ActualHeight, ms, uriHash + "-310x150.png", X.Services.Image.Service.location.TileFolder, 150);

                    ms.Seek(0);
                    await X.Services.Image.Service.Instance.GenerateResizedImageAsync(310, _cvMain.ActualWidth, _cvMain.ActualHeight, ms, uriHash + "-310x310.png", X.Services.Image.Service.location.TileFolder, 310);

                    //update tile
                    X.Services.Tile.Service.UpdatePrimaryTile("X.Browser", 
                        "ms-appdata:///local/tile/" + uriHash + "-150x150.png",
                        "ms-appdata:///local/tile/" + uriHash + "-310x150.png",
                        "ms-appdata:///local/tile/" + uriHash + "-310x310.png",
                        "ms-appdata:///local/tile/" + uriHash + ".png"
                        );

                    
                }
            }
            else if (cv.Type == "NavigationFailed")
            {
                _prLoading.IsActive = false;
                _prLoading.Visibility = Visibility.Collapsed;
            }
            else if (cv.Type == "NavigationCompleted")
            {
                _prLoading.IsActive = false;
                _prLoading.Visibility = Visibility.Collapsed;
            }
            else if (cv.Type == "NavigationStarting")
            {
                _prLoading.IsActive = true;
                _prLoading.Visibility = Visibility.Visible;
            }
        }

        public Brush Accent1
        {
            get { return (Brush)GetValue(Accent1Property); }
            set { SetValue(Accent1Property, value); }
        }

        public Brush Accent2
        {
            get { return (Brush)GetValue(Accent2Property); }
            set { SetValue(Accent2Property, value); }
        }
        





        public static readonly DependencyProperty Accent2Property =
            DependencyProperty.Register("Accent2", typeof(Brush), typeof(Tab), new PropertyMetadata(null));
        
        public static readonly DependencyProperty Accent1Property =
            DependencyProperty.Register("Accent1", typeof(Brush), typeof(Tab), new PropertyMetadata(null));






    }



}
