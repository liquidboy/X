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

namespace X.Viewer.SketchFlow.Controls.Pickers
{
    public sealed partial class ImagePicker : UserControl
    {
        public event EventHandler PerformAction;
        public event EventHandler ImageChanged;
        
        public ImagePicker()
        {
            this.InitializeComponent();

            this.ImageSource = "http://livehdwallpaper.com/wp-content/uploads/2014/08/Beautiful-Rainbow-Scene.jpg";
        }

        private void tbMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            ImageChanged?.Invoke(sender, new ImagePickerEventArgs() { Text = (string)tbMain.Text });
        }

        private void rcb_ValueChanged(object sender, RoutedEventArgs e)
        {
            ImageChanged?.Invoke(sender, new ImagePickerEventArgs() { Text = (string)tbMain.Text });
        }

        public String ImageSource
        {
            get { return (String)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(String), typeof(ImagePicker), new PropertyMetadata(null));

        public async void LoadData(IEnumerable<XElement> sprites, StorageFile spriteSheetFile)
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
                brush.Transform = new CompositeTransform() { ScaleX = 1.65, ScaleY = 1.65, TranslateX = col * (-140), TranslateY = row * (-87) };
                listOfImages.Add(new ImageListItem() { Title = sprite.Attributes("Title").First().Value, SpriteSheetBrush = brush });
            }
            lbPictures.ItemsSource = listOfImages;
        }

   
    }

    public class ImagePickerEventArgs : EventArgs
    {
        public string Text;
    }

    public class ImageListItem
    {
        public ImageBrush SpriteSheetBrush { get; set; }
        public string Title { get; set; }
    }
}
