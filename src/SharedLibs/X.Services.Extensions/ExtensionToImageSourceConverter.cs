using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace X.Services.Extensions.Converters
{
    public class ExtensionToImageSourceConverter : IValueConverter
    {
        //dont do task.result as it may cause deadlocks  
        //http://stackoverflow.com/questions/15003827/async-implementation-of-ivalueconverter


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var url = (string)value;
            var bi = new BitmapImage();

            if (url.Contains("x-ext"))
            {
                using (var stream = getImageStreamAsync(bi, url).Result)
                {
                    bi.SetSourceAsync(stream).AsTask().ConfigureAwait(false);
                }
            }
            else if (url.Contains("http://") || url.Contains("https://"))
            {
                bi.UriSource = new Uri(url);
            }
            else
                bi = null;


            return bi;

        }


        private async Task<IRandomAccessStreamWithContentType> getImageStreamAsync(BitmapImage bi, string url) {
            
                //eg. x-ext://X.Extension.ThirdParty.Backgrounds/bkg02.jpg
                var urlParts = url.Split("/".ToCharArray());
                
                var el = ExtensionsService.Instance.FindExtensionLiteInstance(urlParts[2]);
                var packageDirectory = el.AppExtension.Package.InstalledLocation;
                var publicDirectory = await packageDirectory.GetFolderAsync("public").AsTask().ConfigureAwait(false);
                var imageFile = await publicDirectory.GetFileAsync(urlParts[urlParts.Length - 1]).AsTask().ConfigureAwait(false);
            
                return await imageFile.OpenReadAsync().AsTask().ConfigureAwait(false);
           
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
