using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.SharedLibs.Viewers.FileExplorer
{

    public sealed partial class ScrapePage : Page
    {
        public ScrapePage()
        {
            this.InitializeComponent();
        }
        HtmlDocument htmlDoc;

        IConfiguration config;
        IBrowsingContext context;
        IDocument document;
        

        string html;
        private async void ButScrape_Click(object sender, RoutedEventArgs e)
        {
            html = await wvMain.InvokeScriptAsync("eval",
              new string[]
              {
                "document.documentElement.innerHTML"
              });

            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            document = await context.OpenAsync(wvMain.Source.OriginalString);

            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            await ParseHtml("Images");
            await ParseHtmlAS("Images");
        }

        private async Task ParseHtmlAS(string type) {
            if (document == null) return;
            switch (type)
            {
                case "Images":
                    var imgs = document.QuerySelectorAll("img");
                    var host = wvMain.Source.Host;
                    var scheme = wvMain.Source.Scheme;
                    var htmlImages = "";
                    foreach (IHtmlImageElement img in imgs)
                    {
                        var srcUrl = img.Source.StartsWith("data") || img.Source.StartsWith("http") ? img.Source : $"{scheme}://{host}/{img.Source}";
                        htmlImages += $"<img src=\"{srcUrl}\" style=\"width:300px;\" />";
                        htmlImages += $"<div>{srcUrl}</div>";
                        htmlImages += $"<br/>";
                    }
                    wvResults.NavigateToString(htmlImages);
                    break;
            }
        }

        private async Task ParseHtml(string type) {
            if (htmlDoc == null) return;
            switch (type) {
                case "Images":

                    var imgs = htmlDoc.DocumentNode.SelectNodes("//img");
                    var host = wvMain.Source.Host;
                    var scheme = wvMain.Source.Scheme;
                    var htmlImages = "";
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

                    wvMain.NavigateToString(html + executeJS);

                    var results = await wvMain.InvokeScriptAsync("eval", new string[] { "styleInPage", "fontFamily" });

                    break;
            }

        }

        private void TbUri_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if ((int)e.Key == 13 )
            {
                var url = tbUri.Text;
                if (!url.Contains("http", StringComparison.InvariantCultureIgnoreCase)) url = "http://" + url;
                wvMain.Source = new Uri(url);
            }
        }

        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tbi = (Microsoft.Toolkit.Uwp.UI.Controls.TabViewItem)e.AddedItems.FirstOrDefault();
                switch (tbi.Header)
                {
                    case "Fonts": await ParseHtml("Fonts"); break;
                    case "Images": await ParseHtml("Images"); break;
                }
                await ParseHtmlAS(tbi.Header.ToString());
            }
        }
    }
}
