using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using X.Browser.ViewModels;
using X.Viewer;

namespace X.Browser.Views
{
    partial class MainLayout
    {
        private async void wvMain_SendMessage(object sender, System.EventArgs e)
        {
            try {

                var ea = (ContentViewEventArgs)e;

                if (ea.Type == "ContentLoading")
                {
                    if (wvMain.Uri.Contains("about:blank")) return;
                    //prLoading.IsActive = true;
                    //ShowHideUriArea(0);
                    //sbShowLoading.Begin();
                }
                else if (ea.Type == "NavigationFailed")
                {
                    App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVNavigationFailed);
                }
                else if (ea.Type == "DOMContentLoaded")
                {
                    App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVDOMContentLoaded);
                }
                else if (ea.Type == "NavigationCompleted")
                {
                    App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVNavigationCompleted);
                }
                else if (ea.Type == "NavigationStarting")
                {
                    try
                    {
                        BrowserVM vm = this.DataContext as BrowserVM;
                        vm.SelectedTab.Uri = ea.Uri.OriginalString;
                        App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVNavigationStarting);
                    }
                    catch (Exception ex)
                    {
                        App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVNavigationFailed);
                    }
                }
                else if (ea.Type == "NewWindowRequested")
                {
                    App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVNewWindowRequest);
                    //args.Handled = true;

                }
                else if (ea.Type == "LongRunningScriptDetected")
                {
                    App.ExtensionsSvc.SendMessage(null, ExtensionType.WVLongRunningScriptDetected);
                }
                else if (ea.Type == "ScriptNotify")
                {
                    App.ExtensionsSvc.SendMessage(ea.CallingUri, ExtensionType.WVScriptNotify);
                }
                else if (ea.Type == "FoundFavicon")
                {
                    BrowserVM vm = this.DataContext as BrowserVM;

                    vm.SelectedTab.FaviconUri = ea.Favicon;
                    vm.SelectedTab.ExternalRaisePropertyChanged("FaviconUri");
                    //vm.ExposedRaisePropertyChanged("SelectedTab");


                    ////and update local storage
                    //try
                    //{
                    //    //App.DataService.Storage.UpdateFieldByUid(vm.SelectedTab.Uid.ToString(), "WebPageModel", "FaviconUri", ea.Favicon);
                    //    var found = App.DataService.Storage.RetrieveByUid(vm.SelectedTab.Uid.ToString());
                    //    var foundItem = found[0];
                    //    foundItem.FaviconUri = ea.Favicon;
                    //    App.DataService.Storage.Update(foundItem);
                    //}
                    //catch { }



                }
                else if (ea.Type == "LoadTimedout")
                {
                    //sbHideLoading.Begin();
                }
                else if (ea.Type == "LoadCompleted")
                {

                    App.ExtensionsSvc.SendMessage(ea.Uri, ExtensionType.WVLoadCompleted);

                    BrowserVM vm = this.DataContext as BrowserVM;


                    //todo: work out if this is necessary as this is causing a double postback
                    //if (vm.SelectedTab != null &&  vm.SelectedTab.Uri != e.Uri.AbsoluteUri) vm.UpdateSelectedTabManually(e.Uri);

                    if (vm.SelectedTab == null) return;

                    var uriHash = FlickrNet.UtilityMethods.MD5Hash(vm.SelectedTab.OriginalUri); //   e.Uri);

                    await Task.Delay(1000);

                    //capture screenshot
                    using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                    {
                        //todo :drive this from the x.webview
                        await wvMain.Renderer?.CaptureThumbnail(ms);
                        //await wvMain.CapturePreviewToStreamAsync(ms);

                        //img: Banner 400 width
                        //ms.Seek(0);
                        await X.Services.Image.Service.Instance.GenerateResizedImageAsync(400, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + ".png", X.Services.Image.Service.location.MediumFolder);

                        //img: Thumbnail
                        ms.Seek(0);
                        await X.Services.Image.Service.Instance.GenerateResizedImageAsync(180, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + ".png", X.Services.Image.Service.location.ThumbFolder);

                        //img: Tile
                        ms.Seek(0);
                        await X.Services.Image.Service.Instance.GenerateResizedImageAsync(71, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + ".png", X.Services.Image.Service.location.TileFolder, 71);

                        ms.Seek(0);
                        await X.Services.Image.Service.Instance.GenerateResizedImageAsync(150, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + "-150x150.png", X.Services.Image.Service.location.TileFolder, 150);

                        ms.Seek(0);
                        await X.Services.Image.Service.Instance.GenerateResizedImageAsync(310, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + "-310x150.png", X.Services.Image.Service.location.TileFolder, 150);

                        ms.Seek(0);
                        await X.Services.Image.Service.Instance.GenerateResizedImageAsync(310, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + "-310x310.png", X.Services.Image.Service.location.TileFolder, 310);


                        //update tile
                        //var sxxxxx = Windows.Storage.ApplicationData.Current.LocalFolder;
                        X.Services.Tile.Service.UpdatePrimaryTile(string.Empty, "ms-appdata:///local/tile/" + uriHash + "-310x150.png", string.Empty, Windows.UI.Notifications.TileTemplateType.TileWide310x150ImageAndText01);

                        X.Services.Tile.Service.UpdatePrimaryTile(string.Empty, "ms-appdata:///local/tile/" + uriHash + "-310x310.png", string.Empty, Windows.UI.Notifications.TileTemplateType.TileSquare310x310ImageAndText01);

                        X.Services.Tile.Service.UpdatePrimaryTile(string.Empty, "ms-appdata:///local/tile/" + uriHash + "-150x150.png", string.Empty, Windows.UI.Notifications.TileTemplateType.TileSquare150x150PeekImageAndText01);

                        X.Services.Tile.Service.UpdatePrimaryTile(string.Empty, "ms-appdata:///local/tile/" + uriHash + ".png", string.Empty, Windows.UI.Notifications.TileTemplateType.TileSquare71x71Image);
                        ms.Dispose();

                    }


                    //update thumb in VM
                    var fullUriHash = string.Concat(X.Services.Image.Service.Instance.MediumLocation, "\\", uriHash, ".png");
                    //if (!vm.SelectedTab.ThumbUri.Equals(fullUriHash)) {
                    vm.SelectedTab.ThumbUri = fullUriHash + "?v=" + Guid.NewGuid().ToString();
                    vm.SelectedTab.ExternalRaisePropertyChanged("ThumbUri");
                    //}

                    vm.SelectedTab.LastRefreshedDate = DateTime.UtcNow;

                    ////update UI
                    //prLoading.IsActive = false;
                    //ShowHideUriArea(1);
                    //sbHideLoading.Begin();
                }
            }
            catch (Exception ex){

                //todo : handle this BUT need to work out why this happens

            }


        }
    }
}
