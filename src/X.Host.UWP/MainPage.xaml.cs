using Popcorn.Models.Movie;
using Popcorn.Models.Shows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.SharedLibsCore;
using X.SharedLibsCore.Storage;
using X.Store.UWP;

namespace X.Designer
{
    public sealed partial class MainPage : Page
    {
        StoreFront _store;
        bool isShowingMovies;

        public MainPage()
        {
            this.InitializeComponent();

            _store = new StoreFront();
        }
        

        private async void GrdItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                if (isShowingMovies)
                {
                    var selectedItem = (MovieLightJson)e.AddedItems[0];
                    await _store.LoadMovie(selectedItem.ImdbId);
                    grdDetails.DataContext = _store.Movie;
                    grdDetails.Visibility = Visibility.Visible;

                    await _store.LoadSimilarMovies(_store.Movie);
                    grdSimilarItems.ItemsSource = _store.MoviesSimilar;
                }
                else
                {
                    var selectedItem = (ShowLightJson)e.AddedItems[0];
                    await _store.LoadTVShow(selectedItem.ImdbId);
                    grdDetails.DataContext = _store.Show;
                    grdDetails.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex){
                Debug.WriteLine("problem selecting a movie/show (GrdItems_SelectionChanged)");
            }
        }

        private async void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = grdItems.ChildrenBreadthFirst().OfType<ScrollViewer>().First();
            scrollViewer.ViewChanged += onGrdItemsViewChanged;

            var myVideos = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Videos);
            await _store.InitializeFileSystem(myVideos.SaveFolder.Path);
            await _store.LoadStore();
        }
        bool isCurrentlyLoading = false;
        private async void onGrdItemsViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            //Debug.WriteLine( sv.VerticalOffset);
            //var totalHeight = sv.VerticalOffset + sv.ViewportHeight;

            var scrollPointToTriggerNextPage =  sv.ScrollableHeight - 300;

            if (sv.VerticalOffset < scrollPointToTriggerNextPage || isCurrentlyLoading)
            {
                return;
            }
            isCurrentlyLoading = true;
            
            if (isShowingMovies)
            {
                Debug.WriteLine($"loading page {_store.currentMoviePage}");
                await _store.LoadMovies();
            }
            else
            {
                Debug.WriteLine($"loading page {_store.currentShowPage}");
                await _store.LoadTVShows();
            }
            isCurrentlyLoading = false;
        }

        private void ButMovies_Click(object sender, RoutedEventArgs e)
        {
            isShowingMovies = true;
            grdItems.ItemTemplate = dtMovie;
            grdItems.ItemsSource = _store.Movies;
        }

        private void ButShows_Click(object sender, RoutedEventArgs e)
        {
            isShowingMovies = false;
            grdItems.ItemTemplate = dtTV;
            grdItems.ItemsSource = _store.Shows;
        }

        private void ButCloseDetails_Click(object sender, RoutedEventArgs e)
        {
            grdDetails.DataContext = null;
            grdDetails.Visibility = Visibility.Collapsed;
        }

        private async void ButWatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            //_store.Movie.YtTrailerCode
            if (isShowingMovies)
            {
                var trailerUrl = await _store.LoadMovieTrailer(_store.Movie.ImdbId);
                meTrailer.Source = new Uri(trailerUrl);
                grdTrailer.Visibility = Visibility.Visible;
            }
            else
            {
                var trailerUrl = await _store.LoadShowTrailer(_store.Show.TmdbId);
                meTrailer.Source = new Uri(trailerUrl);
                grdTrailer.Visibility = Visibility.Visible;
            }
        }

        private void ButCloseTrailer_Click(object sender, RoutedEventArgs e)
        {
            meTrailer.Source = null;
            grdTrailer.Visibility = Visibility.Collapsed;
        }

        private async void ButWatch_Click(object sender, RoutedEventArgs e)
        {
            var myVideos = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Videos);
            var xFolder = await myVideos.SaveFolder.CreateFolderAsync("X", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var fileName = isShowingMovies ? _store.Movie.ImdbId : _store.Show.TmdbId.ToString();
            var newTorrentFile = await xFolder.CreateFileAsync($"{fileName}.torrent", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            using (var newFileStream = await newTorrentFile.OpenStreamForWriteAsync()) {

                if (isShowingMovies)
                {
                    //await _store.WatchMovie(_store.Movie, $"{xFolder.Path}\\{fileName}.torrent",  newFileStream);
                }
                else
                {

                }

            }


        }

        private void ButResizeTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (grdTrailer.Width == 400) {
                grdTrailer.HorizontalAlignment = HorizontalAlignment.Stretch;
                grdTrailer.VerticalAlignment = VerticalAlignment.Stretch;
                grdTrailer.Margin = new Thickness(0);
                grdTrailer.Width = double.NaN;
                grdTrailer.Height = double.NaN;
            }
            else {
                grdTrailer.HorizontalAlignment = HorizontalAlignment.Right;
                grdTrailer.VerticalAlignment = VerticalAlignment.Bottom;
                grdTrailer.Margin = new Thickness(50);
                grdTrailer.Width = 400d;
                grdTrailer.Height = 300d;
            }
        }
    }


}
