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
                //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVNavigationFailed);
            }
            else if (ea.Type == "DOMContentLoaded")
            {
                //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVDOMContentLoaded);
            }
            else if (ea.Type == "NavigationCompleted")
            {
                //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVNavigationCompleted);
            }
            else if (ea.Type == "NavigationStarting")
            {
                Windows.Graphics.DirectX.Direct3D11.IDirect3DDevice d3dDevice = null;


                try
                {
                    BrowserVM vm = this.DataContext as BrowserVM;
                    vm.SelectedTab.Uri = ea.Uri.OriginalString;
                    //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVNavigationStarting);
                }
                catch (Exception ex)
                {
                    //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVNavigationFailed);
                }
            }
            else if (ea.Type == "NewWindowRequested")
            {
                //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVNewWindowRequest);
                ////args.Handled = true;
            }
            else if (ea.Type == "LongRunningScriptDetected")
            {
                //_extensionsService.SendMessage(null, ExtensionType.WVLongRunningScriptDetected);
            }
            else if (ea.Type == "ScriptNotify")
            {
                //_extensionsService.SendMessage(ea.CallingUri, ExtensionType.WVScriptNotify);
            }
            else if (ea.Type == "FoundFavicon")
            {
                BrowserVM vm = this.DataContext as BrowserVM;

                vm.SelectedTab.FaviconUri = ea.Favicon;
                vm.SelectedTab.RaisePropChangeOnUIThread("FaviconUri");
                ////vm.ExposedRaisePropertyChanged("SelectedTab");


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
                
                //_extensionsService.SendMessage(ea.Uri, ExtensionType.WVLoadCompleted);

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

                    //Banner 400 width
                    //ms.Seek(0);

                    await App.ImageService.GenerateResizedImageAsync(400, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + ".png", Services.ImageService.location.MediumFolder);


                    //Thumbnail
                    ms.Seek(0);
                    await App.ImageService.GenerateResizedImageAsync(180, wvMain.ActualWidth, wvMain.ActualHeight, ms, uriHash + ".png", Services.ImageService.location.ThumbFolder);

                    ms.Dispose();

                }


                //update thumb in VM
                var fullUriHash = string.Concat(App.ImageService.MediumLocation, "\\", uriHash, ".png");
                //if (!vm.SelectedTab.ThumbUri.Equals(fullUriHash)) {
                vm.SelectedTab.ThumbUri = fullUriHash + "?v=" + Guid.NewGuid().ToString();
                vm.SelectedTab.RaisePropChangeOnUIThread("ThumbUri");
                //}



                ////update UI
                //prLoading.IsActive = false;
                //ShowHideUriArea(1);
                //sbHideLoading.Begin();
            }
        }
    }
}
