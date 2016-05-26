using System;
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


    }

    public class ImagePickerEventArgs : EventArgs
    {
        public string Text;
    }
}
