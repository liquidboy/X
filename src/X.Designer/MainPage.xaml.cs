using Popcorn.Models.Movie;
using Popcorn.Models.Shows;
using System;
using System.Collections.Generic;
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
using X.SharedLibsCore.Storage;

namespace X.Designer
{
    public sealed partial class MainPage : Page
    {
        Store _store;
        bool isShowingMovies;

        public MainPage()
        {
            this.InitializeComponent();

            _store = new Store();
        }
        

        private async void GrdItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isShowingMovies)
            {
                var selectedItem = (MovieLightJson)e.AddedItems[0] ;
                await _store.LoadMovie(selectedItem.ImdbId);
            }
            else {
                var selectedItem = (ShowLightJson)e.AddedItems[0];
                await _store.LoadTVShow(selectedItem.ImdbId);
            }

            
        }

        private async void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            await _store.LoadStore();
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
    }
}
