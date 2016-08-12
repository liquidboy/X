using FlickrNet;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.Services.Data;

namespace X.Extensions.ThirdParty.Flickr
{
    public sealed partial class YourFavourites : UserControl
    {

        public PhotoCollection FavouritePhotos
        {
            get { return (PhotoCollection)GetValue(FavouritePhotosProperty); }
            set { SetValue(FavouritePhotosProperty, value); }
        }

        public static readonly DependencyProperty FavouritePhotosProperty = DependencyProperty.Register("FavouritePhotos", typeof(PhotoCollection), typeof(YourFavourites), new PropertyMetadata(null));



        public YourFavourites()
        {
            this.InitializeComponent();
        }
        
    }
}
