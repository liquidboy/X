﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.FileProperties;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.CoreLib.Shared.Framework.Services.DataEntity;
using X.Viewer.NodeGraph;

namespace X.Viewer.FileExplorer
{
    public sealed partial class Explorer : UserControl
    {
        private ObservableCollection<Item> DataSource = new ObservableCollection<Item>();
        Item _selectedFolder;

        private double AssetsAreaWidth = 500;

        public Explorer()
        {
            this.InitializeComponent();



            InitializeStorage();
            CreateSampleData();
        }


        private void DoTestStorage(object sender, RoutedEventArgs e)
        {
            FileExplorerGlobalStorage.Current.TestStorage();
        }


        private void CreateSampleData() {
            var found = RetrieveFolders();
            if (found.Count == 0)
            {
                var newGlobalFolder = new SavedFolder("Global", DateTime.Now, DateTime.Now, string.Empty);
                Save(newGlobalFolder);
                var newPrivateFolder = new SavedFolder("Private", DateTime.Now, DateTime.Now, string.Empty);
                Save(newPrivateFolder);
                var newFontsFolder = new SavedFolder("Fonts", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newFontsFolder);
                var newglTFFolder = new SavedFolder("glTF", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newglTFFolder);
                var newImagesFolder = new SavedFolder("Images", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newImagesFolder);
                var newLotteFolder = new SavedFolder("Lotte", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newLotteFolder);

                found = RetrieveFolders();
            }

            DataSource = GetFolderChildren(found, string.Empty);            
        }

        private ObservableCollection<Item> GetFolderChildren(List<SavedFolder> nodes, string grouping) {

            var rootItems = nodes.Where(x => x.Grouping.Equals(grouping, StringComparison.InvariantCultureIgnoreCase));
            var col = new ObservableCollection<Item>();
            foreach (var rootItem in rootItems)
            {
                col.Add(new Item() { Folder = rootItem, Children = GetFolderChildren(nodes, rootItem.UniqueId.ToString())});
            }
            return col;
        }

        private void LoadFolderItems(SavedFolder folderToLoad) {
            var col = new ObservableCollection<ItemAsset>();
            var foundAssets = RetrieveAssets(folderToLoad.UniqueId.ToString());
            if (foundAssets.FoundAssets) {
                foreach(var asset in foundAssets.SavedAssets)
                {
                    col.Add(new ItemAsset() { Asset = asset });
                }   
            }
            lbAssets.ItemsSource = col;
        }

        private void TestVisual() {

            //ParticleEmitterVisual
            //ElementCompositionPreview
        }

        private void DoClearStorage(object sender, RoutedEventArgs e)
        {
            ClearStorage();
        }

        private async void DoUpload(object sender, RoutedEventArgs e)
        {
            if (_selectedFolder != null) {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                //picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".gltf");
                picker.FileTypeFilter.Add(".json");
                picker.FileTypeFilter.Add(".zip");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    var properties = await file.GetBasicPropertiesAsync();
                    var newAsset = new SavedAsset(file.Name, DateTime.UtcNow, DateTime.UtcNow, _selectedFolder.Folder.UniqueId.ToString(), file.FileType, MapUlongToLong(properties.Size), false);
                    Save(newAsset);

                    var newFile = await FileExplorerGlobalStorage.Current.CreateFileAndReplaceIfExists( $"{newAsset.UniqueId.ToString()}{file.FileType}");
                    await FileExplorerGlobalStorage.Current.WriteToFile(newFile, file);

                    //// Get thumbnail
                    //const uint requestedSize = 190;
                    //const ThumbnailMode thumbnailMode = ThumbnailMode.PicturesView;
                    //const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                    //var thumbnail = await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions);
                    //if (thumbnail != null) {
                    //    var newFileThumb = await FileExplorerGlobalStorage.Current.CreateFileAndReplaceIfExists($"{newAsset.UniqueId.ToString()}_thumb{file.FileType}");
                    //    await FileExplorerGlobalStorage.Current.WriteToFile(newFileThumb, thumbnail);
                    //}
                    
                }
                else
                {
                    //operation cancelled
                }
            }
        }

        private long MapUlongToLong(ulong ulongValue) => unchecked((long)ulongValue + long.MinValue);

        private ulong MapLongToUlong(long longValue) => unchecked((ulong)(longValue - long.MinValue));

        

        private void TvMain_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            _selectedFolder = (Item)args.InvokedItem;
            LoadFolderItems(_selectedFolder.Folder);
        }

        private void DoUpdateAsset(object sender, RoutedEventArgs e)
        {
            // update local savedasset
            var selectedAsset = (ItemAsset)lbAssets.SelectedItem;
            if (!selectedAsset.Asset.Name.Equals(tbAssetName.Text, StringComparison.InvariantCultureIgnoreCase)) {
                selectedAsset.Asset.Name = tbAssetName.Text;
                Save(selectedAsset.Asset);
            }
        }

        private async void DoGenerateThumbnail(object sender, RoutedEventArgs e)
        {
            var selectedAsset = (ItemAsset)lbAssets.SelectedItem;
            if (selectedAsset != null) {

                // save thumb
                //var thumbFileName = $"{selectedAsset.Asset.UniqueId.ToString()}_thumb{selectedAsset.Asset.FileType}";
                var thumbFileName = $"{selectedAsset.Asset.UniqueId.ToString()}_thumb.jpg";
                var newThumbFile = await FileExplorerGlobalStorage.Current.CreateFileAndReplaceIfExists(thumbFileName);
                using (var newFileStream = await newThumbFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    if (IsImageType(selectedAsset.Asset.FileType))
                    {
                        await imgAssetCropper.SaveAsync(newFileStream, Microsoft.Toolkit.Uwp.UI.Controls.BitmapFileFormat.Jpeg, false);
                    }
                    else if (IsLotteType(selectedAsset.Asset.FileType))
                    {
                        var renderTargetBitmap = new Windows.UI.Xaml.Media.Imaging.RenderTargetBitmap();
                        await renderTargetBitmap.RenderAsync(lottiePlayer);

                        var pixels = await renderTargetBitmap.GetPixelsAsync();

                        var logicalDpi = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;
                        var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId, newFileStream);
                        encoder.SetPixelData(
                            Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                            Windows.Graphics.Imaging.BitmapAlphaMode.Ignore,
                            (uint)renderTargetBitmap.PixelWidth,
                            (uint)renderTargetBitmap.PixelHeight,
                            logicalDpi,
                            logicalDpi,
                            pixels.ToArray());

                        await encoder.FlushAsync();
                    }
                }

                // update asset in local storage
                selectedAsset.Asset.HasThumb = true;
                Save(selectedAsset.Asset);
            }
        }

        private void ClearContent() {
            imgAssetCropper.Source = null;
            imgAssetCropper.Visibility = Visibility.Collapsed;
            lottieJsonSource.UriSource = null;
            lottieJsonSource.ClearValue(Microsoft.Toolkit.Uwp.UI.Lottie.LottieVisualSource.UriSourceProperty);
            lottiePlayer.Visibility = Visibility.Collapsed;
        }

        private async void DaAssetChanged(object sender, RoutedEventArgs e)
        {
            //SelectionChanged = "{x:Bind DoAssetChanged}"
            ClearContent();
            var selectedAsset = (ItemAsset)lbAssets.SelectedItem;
            await LoadAsset(selectedAsset);
        }

        private async Task LoadAsset(ItemAsset selectedAsset) {
            if (selectedAsset != null)
            {
                var fileName = $"{selectedAsset.Asset.UniqueId.ToString()}{selectedAsset.Asset.FileType}";

                if (IsImageType(selectedAsset.Asset.FileType))
                {
                    //imgAssetSelected.Source = await FileExplorerGlobalStorage.Current.ReadImageSourceFromFileViaStream(fileName);
                    imgAssetCropper.Source = await FileExplorerGlobalStorage.Current.ReadWriteableBitmapFromFileViaStream(fileName);
                    imgAssetCropper.Visibility = Visibility.Visible;
                    Tabs.SelectedIndex = 0;

                }
                else if (IsLotteType(selectedAsset.Asset.FileType))
                {
                    var doesFileExist = await FileExplorerGlobalStorage.Current.DoesFileExist(fileName);
                    if (doesFileExist.FileExists)
                    {
                        lottiePlayer.Visibility = Visibility.Visible;
                        await lottieJsonSource.SetSourceAsync(doesFileExist.FileThatWasFound);
                        Tabs.SelectedIndex = 1;
                    }
                }
            }
        }

        private bool IsImageType(string contentType) {
            switch (contentType.ToLower()) {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return true;
                default: return false;
            }
        }

        private bool IsLotteType(string contentType)
        {
            switch (contentType.ToLower())
            {
                case ".json":
                    return true;
                default: return false;
            }
        }

        private void DoDeleteAsset(object sender, RoutedEventArgs e)
        {

        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0) {
                //ClearContent();
                //var selectedAsset = (ItemAsset)lbAssets.SelectedItem;
                //await LoadAsset(selectedAsset);

                var tbi = (Microsoft.Toolkit.Uwp.UI.Controls.TabViewItem)e.AddedItems.FirstOrDefault();
                switch (tbi.Header) {
                    case "Thumbnail": imgAssetCropper.Visibility = Visibility.Visible; break;
                    case "Lotte": lottiePlayer.Visibility = Visibility.Visible; break;
                }
            }
        }

        private AppWindow appWindow;
        private Frame appWindowFrame = new Frame();
        private async void DoWebScrape(object sender, RoutedEventArgs e)
        {
            if (appWindow == null)
            {
                // Create a new window
                appWindow = await AppWindow.TryCreateAsync();
                // Make sure we release the reference to this window, and release XAML resources, when it's closed
                appWindow.Closed += delegate { appWindow = null; appWindowFrame.Content = null; };
                // Navigate the frame to the page we want to show in the new window
                appWindowFrame.Navigate(typeof(X.SharedLibs.Viewers.FileExplorer.ScrapePage));
                // Attach the XAML content to our window
                Windows.UI.Xaml.Hosting.ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowFrame);
            }

            // Now show the window
            await appWindow.TryShowAsync();

        }
    }


    public class Item
    {
        public SavedFolder Folder { get; set; }

        public ObservableCollection<Item> Children { get; set; } = new ObservableCollection<Item>();

        public override string ToString()
        {
            return Folder.Name;
        }
    }

    public class ItemAsset
    {
        public SavedAsset Asset { get; set; }

        public override string ToString()
        {
            return Asset.Name;
        }

        public string FileSize {
            get { return $"{(unchecked((ulong)(Asset.FileSize - long.MinValue)) / 1024)} KB";  }
        }

    }
}
