using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace X.Viewer
{
    public class WebViewRenderer : IContentRenderer
    {
        WebView _renderElement;

        public FrameworkElement RenderElement
        {
            get
            {
                return _renderElement;
            }

            set
            {
                _renderElement = (WebView)value;
            }
        }

        public string Uri { get; set; }
        
        public event EventHandler<ContentViewEventArgs> SendMessage;

        public void Load()
        {
            dtMaxLoadTime = new DispatcherTimer();
            dtMaxLoadTime.Interval = TimeSpan.FromSeconds(7);
            dtMaxLoadTime.Tick += DtMaxLoadTime_Tick;


            _renderElement = new WebView();


            _renderElement.ScriptNotify += wvMain_ScriptNotify;
            _renderElement.ContentLoading += wvMain_ContentLoading;
            _renderElement.NavigationStarting += wvMain_NavigationStarting;
            _renderElement.NavigationCompleted += wvMain_NavigationCompleted;
            _renderElement.LoadCompleted += wvMain_LoadCompleted;
            _renderElement.LongRunningScriptDetected += wvMain_LongRunningScriptDetected;
            _renderElement.UnsafeContentWarningDisplaying += wvMain_UnsafeContentWarningDisplaying;
            _renderElement.UnviewableContentIdentified += wvMain_UnviewableContentIdentified;
            _renderElement.NavigationFailed += wvMain_NavigationFailed;
            _renderElement.NewWindowRequested += wvMain_NewWindowRequested;
            _renderElement.DOMContentLoaded += wvMain_DOMContentLoaded;

            
        }

        public void Unload()
        {
            if (_renderElement != null) {
                
                _renderElement.ScriptNotify -= wvMain_ScriptNotify;
                _renderElement.ContentLoading -= wvMain_ContentLoading;
                _renderElement.NavigationStarting -= wvMain_NavigationStarting;
                _renderElement.NavigationCompleted -= wvMain_NavigationCompleted;
                _renderElement.LoadCompleted -= wvMain_LoadCompleted;
                _renderElement.LongRunningScriptDetected -= wvMain_LongRunningScriptDetected;
                _renderElement.UnsafeContentWarningDisplaying -= wvMain_UnsafeContentWarningDisplaying;
                _renderElement.UnviewableContentIdentified -= wvMain_UnviewableContentIdentified;
                _renderElement.NavigationFailed -= wvMain_NavigationFailed;
                _renderElement.NewWindowRequested -= wvMain_NewWindowRequested;
                _renderElement.DOMContentLoaded -= wvMain_DOMContentLoaded;
                
                _renderElement = null;

                dtMaxLoadTime.Stop();
                dtMaxLoadTime.Tick -= DtMaxLoadTime_Tick;
                dtMaxLoadTime = null;
            }
        }

        public void UpdateSource(string uri)
        {
            if (string.IsNullOrEmpty(uri)) return;

            var FormatedUri = new System.Uri(uri);
            _renderElement.Source = FormatedUri;
        }
















        DispatcherTimer dtMaxLoadTime;

        private void DtMaxLoadTime_Tick(object sender, object e)
        {
            dtMaxLoadTime.Stop();
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "LoadTimedout" });
        }


        private void wvMain_ScriptNotify(object sender, NotifyEventArgs e)
        {
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "ScriptNotify", CallingUri = e.CallingUri });

            var found = e.Value;
        }

        private void wvMain_ContentLoading(Windows.UI.Xaml.Controls.WebView sender, WebViewContentLoadingEventArgs args)
        {
            dtMaxLoadTime.Start();
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "ContentLoading", Uri = args.Uri });
        }

        private void wvMain_NavigationStarting(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationStartingEventArgs args)
        {
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "NavigationStarting", Uri = args.Uri });
        }

        private void wvMain_NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            dtMaxLoadTime.Stop();
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "NavigationCompleted", Uri = args.Uri });
        }

        private async void wvMain_LoadCompleted(object sender, NavigationEventArgs e)
        {
            dtMaxLoadTime.Stop();

            var wvMain = (WebView)sender;

            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "LoadCompleted", Uri = e.Uri, Source = wvMain.Source });

            try
            {

                //reach into the webview's loaded webpage and find any "links" that have a "rel" attribute
                //then return back a concatenated string of all the "href"'s of the links
                //note: the InvokeScriptAsync only lets you return STRING's
                var result = await wvMain.InvokeScriptAsync("eval", new string[] { "var links = document.querySelectorAll('link[rel]'); var result = ''; for( var i = 0; i< links.length; i++){ result += links[i].href + '^';  }; result.toString();  " });

                //get all the found href's into an array
                var parts = result.Split("^".ToCharArray());

                var foundFavi = "";

                //first try for favicon or .ico
                foreach (var part in parts)
                {
                    if (part.Contains("favicon") || part.Contains(".ico"))
                    {
                        foundFavi = part;
                        break;
                    }
                }

                //try for png/jpg if it didnt find a favicon / .ico
                if (string.IsNullOrEmpty(foundFavi))
                {
                    foreach (var part in parts)
                    {
                        if (part.Contains(".png") || part.Contains(".jpg"))
                        {
                            foundFavi = part;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(foundFavi)) SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "FoundFavicon", Favicon = foundFavi });



            }
            catch (Exception ea)
            {
                // normally this exception is raised from the Invokcation of a webview script natively .. 
                // todo: work out what to do if we can't find a favicon
            }
        }

        private void wvMain_LongRunningScriptDetected(Windows.UI.Xaml.Controls.WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "LongRunningScriptDetected" });
        }

        private void wvMain_UnsafeContentWarningDisplaying(Windows.UI.Xaml.Controls.WebView sender, object args)
        {
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "UnsafeContentWarningDisplaying" });
        }

        private void wvMain_UnviewableContentIdentified(Windows.UI.Xaml.Controls.WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "UnviewableContentIdentified" });
        }

        private void wvMain_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            dtMaxLoadTime.Stop();
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "NavigationFailed", Uri = e.Uri, ExtraDetails1 = e.WebErrorStatus.ToString() });
        }

        private void wvMain_NewWindowRequested(Windows.UI.Xaml.Controls.WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "NewWindowRequested", Uri = args.Uri });
            args.Handled = true;
        }

        private void wvMain_DOMContentLoaded(Windows.UI.Xaml.Controls.WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            dtMaxLoadTime.Stop();
            SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = "DOMContentLoaded", Uri = args.Uri });

        }


        public async Task CaptureThumbnail(Windows.Storage.Streams.InMemoryRandomAccessStream ms)
        {
            await _renderElement.CapturePreviewToStreamAsync(ms);
        }

        public void SendMessageThru(object source, ContentViewEventArgs ea)
        {
            this.SendMessage?.Invoke(source, ea);
        }
    }
}
