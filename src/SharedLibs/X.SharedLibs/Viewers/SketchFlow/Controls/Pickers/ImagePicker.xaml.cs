using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using X.Services.Extensions;

namespace X.Viewer.SketchFlow.Controls.Pickers
{
    public sealed partial class ImagePicker : UserControl
    {
        //public event EventHandler PerformAction;
        public event EventHandler ImageChanged;
        
        public ImagePicker()
        {
            this.InitializeComponent();

            this.ImageSource = new BitmapImage(new Uri("http://livehdwallpaper.com/wp-content/uploads/2014/08/Beautiful-Rainbow-Scene.jpg"));
        }

        private void tbMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            ImageChanged?.Invoke(sender, new ImagePickerEventArgs() { Text = (string)tbMain.Text });
        }

        private void rcb_ValueChanged(object sender, RoutedEventArgs e)
        {
            ImageChanged?.Invoke(sender, new ImagePickerEventArgs() { Text = (string)tbMain.Text });
        }


        public async void LoadData(IEnumerable<XElement> sprites, StorageFile spriteSheetFile, string appExtensionId)
        {
            var bitmapImage = new BitmapImage();
            using (var stream = await spriteSheetFile.OpenReadAsync()) {
                await bitmapImage.SetSourceAsync(stream);
            }
            
            //xaml
            List<ImageListItem> listOfImages = new List<ImageListItem>();
            foreach (var sprite in sprites) {
                var row = int.Parse(sprite.Attributes("Row").First().Value);
                var col = int.Parse(sprite.Attributes("Column").First().Value);

                var brush = new ImageBrush();
                brush.ImageSource = bitmapImage;
                brush.Stretch = Stretch.UniformToFill;
                brush.AlignmentX = AlignmentX.Left;
                brush.AlignmentY = AlignmentY.Top;
                brush.Transform = new CompositeTransform() { ScaleX = 2.35, ScaleY = 2.35, TranslateX = col * (-140), TranslateY = row * (-87) };
                listOfImages.Add(new ImageListItem() {
                    Title = sprite.Attributes("Title").First().Value,
                    SpriteSheetBrush = brush,
                    File = sprite.Attributes("File").First().Value,
                    AppExtensionId = appExtensionId
                });
            }
            lbPictures.ItemsSource = listOfImages;
        }

        private async void lbPictures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e != null &&  e.AddedItems.Count > 0)
            {
                var ili = e.AddedItems[0] as ImageListItem;

                ImageUri = $"x-ext://{ili.AppExtensionId}/{ili.File}";

                ImageChanged?.Invoke(sender, new ImagePickerEventArgs() {
                    Text = ImageUri,
                    AppExtensionId = ili.AppExtensionId,
                    File = ili.File });

                

                //set imagesource
                var el = ExtensionsService.Instance.FindExtensionLiteInstance(ili.AppExtensionId);
                var packageDirectory = el.AppExtension.Package.InstalledLocation;
                var publicDirectory = await packageDirectory.GetFolderAsync("public");
                var ImageFile = await publicDirectory.GetFileAsync(ili.File);
                var bitmapImage = new BitmapImage();
                using (var stream = await ImageFile.OpenReadAsync())
                {
                    await bitmapImage.SetSourceAsync(stream);
                }
                ImageSource = bitmapImage;
            }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImagePicker), new PropertyMetadata(null));

        public string ImageUri
        {
            get { return (string)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        public static readonly DependencyProperty ImageUriProperty =
            DependencyProperty.Register("ImageUri", typeof(string), typeof(ImagePicker), new PropertyMetadata(null));



    }

    public class ImagePickerEventArgs : EventArgs
    {
        public string Text;
        public string AppExtensionId { get; set; }
        public string File { get; set; }
    }

    public class ImageListItem
    {
        public ImageBrush SpriteSheetBrush { get; set; }
        public string Title { get; set; }
        public string AppExtensionId { get; set; }
        public string File { get; set; }
    }
}
