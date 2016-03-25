using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage;
using CoreLib;

namespace X.Services.Image
{
    public static class Service 
    {
        //string tempFolderLocation;

        static Windows.Storage.StorageFolder _localFolder;
        static Windows.Storage.StorageFolder _mediumFolder;
        static Windows.Storage.StorageFolder _thumbFolder;
        static Windows.Storage.StorageFolder _tileFolder;
        static Windows.Storage.StorageFolder _originalFolder;

        //public string MediumLocation { get { return System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "medium"); } private set { } }
        public static string MediumLocation { get { return _mediumFolder.Path; } private set { } }

        public enum location
        {
            MediumFolder,
            ThumbFolder,
            Original,
            TileFolder
        }
        
        private static async void InitFolders()
        {
            _localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                _mediumFolder = await _localFolder.GetFolderAsync("medium");
            }
            catch (System.IO.FileNotFoundException ex)
            {

            }
            if (_mediumFolder == null) _mediumFolder = await _localFolder.CreateFolderAsync("medium");

            try
            {
                _thumbFolder = await _localFolder.GetFolderAsync("thumb");
            }
            catch (System.IO.FileNotFoundException ex)
            {

            }
            if (_thumbFolder == null) _thumbFolder = _localFolder.CreateFolderAsync("thumb").GetResults();

            try
            {
                _originalFolder = await _localFolder.GetFolderAsync("original");
            }
            catch (System.IO.FileNotFoundException ex)
            {

            }

            try
            {
                if (_originalFolder == null) _originalFolder = _localFolder.CreateFolderAsync("original").GetResults();
            }
            catch
            {

            }


            try
            {
                _tileFolder = await _localFolder.GetFolderAsync("tile");
            }
            catch (System.IO.FileNotFoundException ex)
            {

            }
            if (_tileFolder == null) _tileFolder = _localFolder.CreateFolderAsync("tile").GetResults();

        }


        //public static async Task<bool> GenerateImageAsync(InMemoryRandomAccessStream ms, string newImageName)
        //{
        //    using (var a = ms.AsStream())
        //    {
        //        byte[] b = new byte[a.Length];
        //        await a.ReadAsync(b, 0, (int)a.Length);
        //        StorageFile sampleFile = await _originalFolder.CreateFileAsync(newImageName);
        //        await FileIO.WriteBytesAsync(sampleFile, b);
        //    }

        //    return true;
        //}

        public static async Task<bool> GenerateResizedImageAsync(int longWidth, double srcWidth, double srcHeight, InMemoryRandomAccessStream srcMemoryStream, string newImageName, location subFolder, int longHeight = 0)
        {
            if (_localFolder == null) InitFolders();

            try
            {
                int width = 0, height = 0;
                double factor = srcWidth / srcHeight;
                if (factor < 1)
                {
                    height = longWidth;
                    width = (int)(longWidth * factor);
                }
                else
                {
                    width = longWidth;
                    height = (int)(longWidth / factor);
                }

                if (longHeight > 0)
                {
                    width = longWidth;
                    height = longHeight;
                }
                
                WriteableBitmap wb = await BitmapFactory.New((int)srcWidth, (int)srcHeight).FromStream(srcMemoryStream);
                
                //WRITEABLE BITMAP IS THROWING AN ERROR

                var wbthumbnail = wb.Resize(width, height, Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions.Interpolation.Bilinear);
                
                switch (subFolder)
                {
                    case location.MediumFolder:
                        StorageFile sampleFile1 = await _mediumFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
                        await wbthumbnail.SaveToFile(sampleFile1, BitmapEncoder.PngEncoderId);
                        break;
                    case location.ThumbFolder:
                        StorageFile sampleFile2 = await _thumbFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
                        await wbthumbnail.SaveToFile(sampleFile2, BitmapEncoder.PngEncoderId);
                        break;
                    case location.TileFolder:
                        StorageFile sampleFile3 = await _tileFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
                        await wbthumbnail.SaveToFile(sampleFile3, BitmapEncoder.PngEncoderId);
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

    }
}
