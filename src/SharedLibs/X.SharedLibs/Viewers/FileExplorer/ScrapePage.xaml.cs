using AngleSharp;
using AngleSharp.Dom;
//using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeExplode;

namespace X.SharedLibs.Viewers.FileExplorer
{

    public sealed partial class ScrapePage : Page
    {
        public ScrapePage()
        {
            this.InitializeComponent();
            //ClearStorage();
            SetupWebView();
            DrawVideos();
        }
        HtmlDocument htmlDoc;

        IConfiguration config;
        IBrowsingContext context;
        IDocument document;
        

        string html;
        private async void ButScrape_Click(object sender, RoutedEventArgs e)
        {
            await startScrape("Images");
        }

        private async Task startScrape(string type) {

            try
            {
                html = await _wvMain.InvokeScriptAsync("eval", new string[] { "document.documentElement.innerHTML" });
                config = Configuration.Default.WithDefaultLoader();
                context = BrowsingContext.New(config);
                document = await context.OpenAsync(_wvMain.Source.OriginalString);

                htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                await ParseHtml(type);
                //await ParseHtmlAS(type);
            }
            catch (Exception ex) { }
            
        }

        WebView _wvMain;
        private void SetupWebView() {
            if (_wvMain != null) return;
            _wvMain = new WebView(WebViewExecutionMode.SeparateThread);
            _wvMain.FrameNavigationCompleted += WvMain_FrameNavigationCompleted;
            _wvMain.FrameNavigationStarting += WvMain_FrameNavigationStarting;
            _wvMain.NavigationCompleted += WvMain_NavigationCompleted;
            _wvMain.NavigationFailed += WvMain_NavigationFailed;
            _wvMain.NavigationStarting += WvMain_NavigationStarting;
            //_wvMain.AllowedScriptNotifyUris = true;
            _wvMain.ScriptNotify += wvMain_ScriptNotify;

            grdWebView.Children.Add(_wvMain);
        }

        private void wvMain_ScriptNotify(object sender, NotifyEventArgs e)
        {
            
        }

        //private async Task ParseHtmlAS(string type) {
        //    if (document == null) return;
        //    switch (type)
        //    {
        //        case "Images":
        //            //var imgs = document.QuerySelectorAll("img");
        //            //var host = wvMain.Source.Host;
        //            //var scheme = wvMain.Source.Scheme;
        //            //var htmlImages = "";
        //            //foreach (IHtmlImageElement img in imgs)
        //            //{
        //            //    var srcUrl = img.Source.StartsWith("data") || img.Source.StartsWith("http") ? img.Source : $"{scheme}://{host}/{img.Source}";
        //            //    htmlImages += $"<img src=\"{srcUrl}\" style=\"width:300px;\" />";
        //            //    htmlImages += $"<div>{srcUrl}</div>";
        //            //    htmlImages += $"<br/>";
        //            //}
        //            //wvResults.NavigateToString(htmlImages);
        //            break;
        //    }
        //}

        List<YoutubeExplode.Models.Video> _videos = new List<YoutubeExplode.Models.Video>();
        private async Task ParseHtml(string type) {
            //if (htmlDoc == null) return;
            switch (type) {
                case "Images":

                    var imgs = htmlDoc.DocumentNode.SelectNodes("//img");
                    var host = _wvMain.Source.Host;
                    var scheme = _wvMain.Source.Scheme;
                    var htmlImages = "<script type=\"text/javascript\">function callApp(param){ window.external.notify(param); }</script>";
                    foreach (var img in imgs)
                    {
                        var src = img.Attributes["src"]?.Value;
                        if (src.Length > 0)
                        {
                            var srcUrl = src.StartsWith("data") || src.StartsWith("http") ? src : $"{scheme}://{host}/{src}";
                            htmlImages += $"<img src=\"{srcUrl}\" style=\"width:300px;\" />";
                            htmlImages += $"<div>{srcUrl}</div>";
                            htmlImages += $"<br/>";

                            //try
                            //{
                            //    var rootFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("MyAppName\\CoverPics", CreationCollisionOption.OpenIfExists);
                            //    var coverpic_file = await rootFolder.CreateFileAsync(filename, CreationCollisionOption.FailIfExists);
                            //    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient(); 
                            //    byte[] buffer = await client.GetByteArrayAsync(srcUrl); // Download file
                            //    using (Stream stream = await coverpic_file.OpenStreamForWriteAsync())
                            //        stream.Write(buffer, 0, buffer.Length); // Save
                            //}
                            //catch
                            //{
                            //    saved = false;
                            //}

                        }
                    }

                    wvResults.NavigateToString(htmlImages);
                    break;
                case "Videos":
                    tbMain.Text = "processing video ..";
                    var yturl = _wvMain.Source.AbsoluteUri;
                    try {
                        var id = YoutubeClient.ParseVideoId(yturl);
                        var ytclient = new YoutubeClient();
                        var video = await ytclient.GetVideoAsync(id);
                        if (!_videos.Where(x => x.Id == video.Id).Any()) {
                            _videos.Add(video);

                            Save(new SavedVideo(video.Id,video.Author, video.UploadDate.UtcDateTime, video.Title, video.Description, video.Duration.Ticks, video.Thumbnails.MediumResUrl, "recent"));

                            tbMain.Text = "video successfully processed ..";
                        }
                        DrawVideos();
                    } catch (Exception ex){
                        tbMain.Text = "error processing video ..";
                    }
                    
                    break;
                case "Fonts":
                    return;
                    string executeJS = @"
                        <script type=""javascript"">
                        function styleInPage(css, verbose){
                            if(typeof getComputedStyle== ""undefined"")
                            getComputedStyle= function(elem){
                                return elem.currentStyle;
                            }
                            var who, hoo, values= [], val,
                            nodes= document.body.getElementsByTagName('*'),
                            L= nodes.length;
                            for(var i= 0; i<L; i++){
                                who= nodes[i];
                                if(who.style){
                                    hoo= '#'+(who.id || who.nodeName+'('+i+')');
                                    val= who.style.fontFamily || getComputedStyle(who, '')[css];
                                    if(val){
                                        if(verbose) values.push([hoo, val]);
                                        else if(values.indexOf(val)== -1) values.push(val);
                                    }
                                    val_before = getComputedStyle(who, ':before')[css];
                                    if(val_before){
                                        if(verbose) values.push([hoo, val_before]);
                                        else if(values.indexOf(val_before)== -1) values.push(val_before);
                                    }
                                    val_after= getComputedStyle(who, ':after')[css];
                                    if(val_after){
                                        if(verbose) values.push([hoo, val_after]);
                                        else if(values.indexOf(val_after)== -1) values.push(val_after);
                                    }
                                }
                            }
                            return values;
                        }
                        </script>
                    ";

                    //await wvMain.InvokeScriptAsync("eval", new string[] { $"var script = document.createElement('script');script.innerHTML = 'var el = document.createElement(\"div\");el.innerHTML=\"{executeJS}\";document.body.appendChild(el);';document.body.appendChild(script);" });

                    _wvMain.NavigateToString(html + executeJS);

                    var results = await _wvMain.InvokeScriptAsync("eval", new string[] { "styleInPage", "fontFamily" });

                    break;
            }

        }

        private void DrawVideos() {
            var html = "<script type=\"text/javascript\">function callApp(param){ window.external.notify(param); }</script>";
            var videos = RetrieveVideos();
            //foreach (var video in _videos) {
            foreach (var video in videos)
            {
                if (video != null)
                {
                    var srcUrl = video.ThumbnailMediumResUrl;
                    html += $"<h3>{video.Title}</h3>";
                    html += $"<h4>{video.Author}</h4>"; 
                    html += $"<img src=\"{srcUrl}\" style=\"width:300px;\" />";
                    html += $"<a href=\"#\" onclick=\"callApp('{srcUrl}');\">{srcUrl}</a>";
                    html += $"<br/>";
                }
            }
            //wvResults.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ()=> {
            //    wvResults.NavigateToString(html);
            //}).GetResults();
            wvResults.NavigateToString(html);

        }

        private void TbUri_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if ((int)e.Key == 13 )
            {
                var url = tbUri.Text;
                if (!url.Contains("http", StringComparison.InvariantCultureIgnoreCase)) url = "http://" + url;
                _wvMain.Source = new Uri(url);
            }
        }

        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tbi = (Microsoft.Toolkit.Uwp.UI.Controls.TabViewItem)e.AddedItems.FirstOrDefault();

                //switch (tbi.Header)
                //{
                //    case "Fonts": await ParseHtml("Fonts"); break;
                //    case "Images": await ParseHtml("Images"); break;
                //    case "Videos": await ParseHtml("Videos"); break;
                //}
                //await ParseHtmlAS(tbi.Header.ToString());

                await startScrape(tbi.Header.ToString());
            }
        }

        private void WvMain_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            tbMain.Text = "NavigationCompleted ..";
        }

        private void WvMain_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            tbMain.Text = "NavigationFailed ..";
        }

        private void WvMain_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            tbMain.Text = "NavigationStarting ..";
            
        }

        private void WvMain_FrameNavigationCompleted(WebView sender,WebViewNavigationCompletedEventArgs args)
        {
            tbMain.Text = "FrameNavigationCompleted ..";
        }

        private async void WvMain_FrameNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            tbMain.Text = "FrameNavigationStarting ..";
            await ParseHtml("Videos");
        }
    }
}
