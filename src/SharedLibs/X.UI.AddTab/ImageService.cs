using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage;
using CoreLib;

namespace X.Services
{
    public class ImageService : IDisposable
    {
        //string tempFolderLocation;

        Windows.Storage.StorageFolder _localFolder;
        Windows.Storage.StorageFolder _mediumFolder;
        Windows.Storage.StorageFolder _thumbFolder;
        Windows.Storage.StorageFolder _originalFolder;

        //public string MediumLocation { get { return System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "medium"); } private set { } }
        public string MediumLocation { get { return _mediumFolder.Path; } private set { } }

        public enum location
        {
            MediumFolder,
            ThumbFolder,
            Original
        }

        public ImageService()
        {
            //tempFolderLocation = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "ImageService");

            try
            {
                InitFolders();
            }
            catch (Exception ex) { }
            

        }

        private async void InitFolders()
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

            try {
                if (_originalFolder == null) _originalFolder = _localFolder.CreateFolderAsync("original").GetResults();
            }
            catch {

            }

            
        }


        public async Task<bool> GenerateImageAsync(InMemoryRandomAccessStream ms, string newImageName)
        {
            using (var a = ms.AsStream())
            {
                byte[] b = new byte[a.Length];
                await a.ReadAsync(b, 0, (int)a.Length);
                StorageFile sampleFile = await _originalFolder.CreateFileAsync(newImageName);
                await FileIO.WriteBytesAsync(sampleFile, b);
            }

            return true;
        }

        public async Task<bool> GenerateResizedImageAsync(int longlength, double srcWidth, double srcHeight, InMemoryRandomAccessStream srcMemoryStream, string newImageName, location subFolder)
        {
            try
            {
                int width = 0, height = 0;
                double factor = srcWidth / srcHeight;
                if (factor < 1)
                {
                    height = longlength;
                    width = (int)(longlength * factor);
                }
                else
                {
                    width = longlength;
                    height = (int)(longlength / factor);
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
                }

                return true;
            }catch(Exception ex){
                return false;
            }
            

        }

        public void Dispose()
        {
           
        }
    }

}
