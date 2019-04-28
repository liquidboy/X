using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using X.Viewer.FileExplorer;

namespace CoreLib.Converters
{
    public class XAssetThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage bitmapImage = new BitmapImage();


            var savedAsset = (SavedAsset)value;

            if (savedAsset == null || !savedAsset.HasThumb) return null;

            var thumbUrl = $"{savedAsset.UniqueId}_thumb.jpg";


            //DID NOT WORK
            //var t = new Task(async () => {
            //    StorageFolder folder = Windows.Storage.KnownFolders.PicturesLibrary;
            //    var appFolder = await folder.CreateFolderAsync("FavouriteMX", CreationCollisionOption.OpenIfExists);
            //    var sessionsFolder = await appFolder.CreateFolderAsync("Sessions", CreationCollisionOption.OpenIfExists);
            //    try
            //    {
            //        var file = await sessionsFolder.GetFileAsync("session-" + (string)value + ".png");
            //        using (var stream = await file.OpenReadAsync())
            //        {
            //            await bitmapImage.SetSourceAsync(stream);
            //        }
            //    }
            //    catch (Exception ex) { }
            //});
            //t.Start();




            //DID NOT WORK
            //Task.Factory.StartNew(async () => {
            //    StorageFolder folder = Windows.Storage.KnownFolders.PicturesLibrary;
            //    var appFolder = await folder.CreateFolderAsync("FavouriteMX", CreationCollisionOption.OpenIfExists);
            //    var sessionsFolder = await appFolder.CreateFolderAsync("Sessions", CreationCollisionOption.OpenIfExists);
            //    var file = await sessionsFolder.GetFileAsync("session-" + (string)value + ".png");

            //    using (var stream = await file.OpenReadAsync())
            //    {
            //        await bitmapImage.SetSourceAsync(stream);
            //    }
            //    return bitmapImage;
            //});





            //DIDN'T WORK
            //Task.Factory.StartNew(async () =>
            //{
            //    bitmapImage = await RetrieveImageFromLocalAsync((string)value);
            //});





            //DIDN'T WORK
            //bitmapImage = RetrieveImageFromLocalAsync((string)value).Result;






            //WORKED :(
            StorageFolder folder = Windows.Storage.KnownFolders.PicturesLibrary;
            var appFolder = folder.CreateFolderAsync("XAssets", CreationCollisionOption.OpenIfExists).AsTask().Result;

            try
            {
                var file = appFolder.GetFileAsync(thumbUrl).AsTask().Result;

                using (var stream = file.OpenReadAsync().AsTask().Result)
                {
                    bitmapImage.SetSourceAsync(stream).AsTask().RunSynchronously();
                }

            }
            catch (Exception ex) { }

            return bitmapImage;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }



        private async Task<BitmapImage> RetrieveImageFromLocalAsync(string fileName) {

            BitmapImage bitmapImage = new BitmapImage();

            StorageFolder folder = Windows.Storage.KnownFolders.PicturesLibrary;
            var appFolder = await folder.CreateFolderAsync("FavouriteMX", CreationCollisionOption.OpenIfExists);
            var sessionsFolder = await appFolder.CreateFolderAsync("Sessions", CreationCollisionOption.OpenIfExists);

            try
            {
                var file = await sessionsFolder.GetFileAsync("session-" + fileName + ".png");

                using (var stream = await file.OpenReadAsync())
                {
                    await bitmapImage.SetSourceAsync(stream);
                }

            }
            catch //(Exception ex) 
            { }

            return bitmapImage;
        }
    }
}
