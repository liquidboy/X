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
    public class Service 
    {
        //string tempFolderLocation;

        Windows.Storage.StorageFolder _localFolder;
        Windows.Storage.StorageFolder _mediumFolder;
        Windows.Storage.StorageFolder _thumbFolder;
        Windows.Storage.StorageFolder _tileFolder;
        Windows.Storage.StorageFolder _originalFolder;

        //public string MediumLocation { get { return System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "medium"); } private set { } }
        public  string MediumLocation { get { return _mediumFolder.Path; } private set { } }

        public enum location
        {
            MediumFolder,
            ThumbFolder,
            Original,
            TileFolder
        }


        public Service() {
            
        }


        public async void InitFolders()
        {
            _localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            var folderName = "";
            try
            {
                folderName = "medium";
                if (Directory.Exists($"{_localFolder.Path}\\{folderName}")) _mediumFolder = await _localFolder.GetFolderAsync(folderName);
                else _mediumFolder = await _localFolder.CreateFolderAsync(folderName);

                folderName = "thumb";
                if (Directory.Exists($"{_localFolder.Path}\\{folderName}")) _thumbFolder = await _localFolder.GetFolderAsync(folderName);
                else _thumbFolder = await _localFolder.CreateFolderAsync(folderName);

                folderName = "original";
                if (Directory.Exists($"{_localFolder.Path}\\{folderName}")) _originalFolder = await _localFolder.GetFolderAsync(folderName);
                else _originalFolder = await _localFolder.CreateFolderAsync(folderName);

                folderName = "tile";
                if (Directory.Exists($"{_localFolder.Path}\\{folderName}")) _tileFolder = await _localFolder.GetFolderAsync(folderName);
                else _tileFolder = await _localFolder.CreateFolderAsync(folderName);

            }
            catch //(System.IO.FileNotFoundException ex)
            {
                //todo: what would ever cause this ??! need to work out how to handle this type of error
            }
            
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

        //public async Task<bool> GenerateResizedImageAsyncOld(int longWidth, double srcWidth, double srcHeight, InMemoryRandomAccessStream srcMemoryStream, string newImageName, location subFolder, int longHeight = 0)
        //{
        //    if (_localFolder == null) InitFolders();

        //    try
        //    {
        //        int width = 0, height = 0;
        //        double factor = srcWidth / srcHeight;
        //        if (factor < 1)
        //        {
        //            height = longWidth;
        //            width = (int)(longWidth * factor);
        //        }
        //        else
        //        {
        //            width = longWidth;
        //            height = (int)(longWidth / factor);
        //        }

        //        if (longHeight > 0)
        //        {
        //            width = longWidth;
        //            height = longHeight;
        //        }
                
        //        WriteableBitmap wb = await BitmapFactory.New((int)srcWidth, (int)srcHeight).FromStream(srcMemoryStream);
                
        //        //WRITEABLE BITMAP IS THROWING AN ERROR

        //        var wbthumbnail = wb.Resize(width, height, Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions.Interpolation.Bilinear);

        //        switch (subFolder)
        //        {
        //            case location.MediumFolder:
        //                StorageFile sampleFile1 = await _mediumFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
        //                await wbthumbnail.SaveToFile(sampleFile1, BitmapEncoder.PngEncoderId);
        //                break;
        //            case location.ThumbFolder:
        //                StorageFile sampleFile2 = await _thumbFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
        //                await wbthumbnail.SaveToFile(sampleFile2, BitmapEncoder.PngEncoderId);
        //                break;
        //            case location.TileFolder:
        //                StorageFile sampleFile3 = await _tileFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
        //                await wbthumbnail.SaveToFile(sampleFile3, BitmapEncoder.PngEncoderId);
        //                break;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }


        //}

        public async Task<bool> GenerateResizedImageAsync(int longWidth, double srcWidth, double srcHeight, InMemoryRandomAccessStream srcMemoryStream, string newImageName, location subFolder, int longHeight = 0)
        {
            if (_localFolder == null) InitFolders();
            if (srcMemoryStream.Size == 0) return false;

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



                if (subFolder == location.MediumFolder) {
                    WriteableBitmap wb = await BitmapFactory.New((int)srcWidth, (int)srcHeight).FromStream(srcMemoryStream);
                    var wbthumbnail = wb.Resize(width, height, Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions.Interpolation.Bilinear);
                    StorageFile sampleFile1 = await _mediumFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
                    await wbthumbnail.SaveToFile(sampleFile1, BitmapEncoder.PngEncoderId);
                }
                else if (subFolder == location.ThumbFolder)
                {
                    WriteableBitmap wb = await BitmapFactory.New((int)srcWidth, (int)srcHeight).FromStream(srcMemoryStream);
                    var wbthumbnail = wb.Resize(width, height, Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions.Interpolation.Bilinear);
                    StorageFile sampleFile2 = await _thumbFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
                    await wbthumbnail.SaveToFile(sampleFile2, BitmapEncoder.PngEncoderId);
                }
                else if (subFolder == location.TileFolder) {

                    //https://social.msdn.microsoft.com/Forums/en-US/490b9c01-db4b-434f-8aff-d5c495e67e55/how-to-crop-an-image-using-bitmaptransform?forum=winappswithcsharp

                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(srcMemoryStream);

                    using (InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream()) { 
                        BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

                        var h = longHeight / srcHeight;
                        var w = longWidth / srcWidth;
                        var r = Math.Max(h, w);
                        
                        enc.BitmapTransform.ScaledWidth = (uint)(srcWidth * r);
                        enc.BitmapTransform.ScaledHeight = (uint)(srcHeight * r);
                    
                        BitmapBounds bounds = new BitmapBounds();
                        bounds.Width = (uint)longWidth;
                        bounds.Height = (uint)longHeight;
                        bounds.X = 0;
                        bounds.Y = 0;
                        enc.BitmapTransform.Bounds = bounds;

                        await enc.FlushAsync();

                        WriteableBitmap wb = await BitmapFactory.New(longWidth, longHeight).FromStream(ras);

                        StorageFile sampleFile3 = await _tileFolder.CreateFileAsync(newImageName, CreationCollisionOption.ReplaceExisting);
                        await wb.SaveToFile(sampleFile3, BitmapEncoder.PngEncoderId);
                    }
                    
                }
                
            
            

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }










        //=========================
        //singleton
        //=========================
        private static Service instance;

        public static Service Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Service();
                }
                return instance;
            }
        }
    }
}
