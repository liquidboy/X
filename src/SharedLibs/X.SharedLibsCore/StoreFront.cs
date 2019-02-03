//using Microsoft.EntityFrameworkCore;
using GalaSoft.MvvmLight;
using NuGet;
using Popcorn.Comparers;
using Popcorn.Helpers;
using Popcorn.Models.Bandwidth;
using Popcorn.Models.Episode;
using Popcorn.Models.Media;
using Popcorn.Models.Movie;
using Popcorn.Models.Shows;
using Popcorn.Services.Cache;
using Popcorn.Services.Download;
using Popcorn.Services.Movies.Movie;
using Popcorn.Services.Shows.Show;
using Popcorn.Services.Tmdb;
using Popcorn.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using X.SharedLibsCore.Storage;

namespace X.SharedLibsCore
{
    public partial class StoreFront
    {
        public readonly ICacheService _cacheService;
        protected readonly SemaphoreSlim LoadingSemaphore = new SemaphoreSlim(1, 1);
        protected CancellationTokenSource[] CancellationLoading { get; private set; }

        private enum CancellationTokenTypes
        {
            Movies,
            MoviesSimilar,
            Shows,
            ShowsSimilar,
            TotalTypes
        }


        public ObservableCollection<Downloader> Downloaders { get; set; }
        
        public class Downloader
        {
            public string Key { get; set; }

            public Uri ThumbUri { get; set; }

            public DownloadMovieService<MovieJson> DownloadServiceMovie { get; set; }
            public DownloadShowService<EpisodeShowJson> DownloadServiceShow { get; set; }

            public void ReportNbSeeders(int value) => DownloadProgress.Seeders = value;

            public void ReportNbPeers(int value) => DownloadProgress.Peers = value;

            public void ReportMovieDownloadRate(BandwidthRate value) => DownloadProgress.DownloadRate = value.DownloadRate;

            public void ReportMovieDownloadProgress(double value) => DownloadProgress.Progress = value;

            public DownloadStatus DownloadProgress { get; set; }

            public Downloader() {
                DownloadProgress = new DownloadStatus();
            }
        }

        public StoreFront()
        {
            Downloaders = new ObservableCollection<Downloader>();

            var tmdbService = new TmdbService();
            _movieService = new MovieService(tmdbService);
            _showService = new ShowService(tmdbService);

            SetupMovies();
            SetupShows();

            //DownloadProgress = new DownloadStatus();
            CurrentDownloadingMove = new DownloadMovie();

            _cacheService = new CacheService("X");
            
            CancellationLoading = new CancellationTokenSource[(int)CancellationTokenTypes.TotalTypes];
            for (int counter = 0; counter < (int)CancellationTokenTypes.TotalTypes; counter++)
            {
                CancellationLoading[counter] = new CancellationTokenSource();
            }
        }

        public async Task InitializeFileSystem(string movieUrl)
        {
            _cacheService.LocalPath = movieUrl;
            await InitializeMovie();
            await InitializeShow();


        }

        public async Task LoadStore(bool reset = false)
        {
            await LoadMovies();
            await LoadTVShows();
        }


        public enum SortBy
        {

            movieGreatest,
            moviePopular,
            movieRecent,

            showPopular,
            showGreatest,
            showRecent,
            showUpdated
        }


        private string getSortBy(SortBy sortBy)
        {

            switch (sortBy)
            {
                case SortBy.movieGreatest: return "download_count";
                case SortBy.moviePopular: return "seeds";
                case SortBy.movieRecent: return "year";

                case SortBy.showPopular: return "watching";
                case SortBy.showGreatest: return "votes";
                case SortBy.showRecent: return "year";
                case SortBy.showUpdated: return "date_added";
            }

            return "";
        }
        
        public DownloadMovie CurrentDownloadingMove { get; set; }

        private CancellationTokenSource GetCancellationTokenSource(CancellationTokenTypes type) {
            return CancellationLoading[(int)type];
        }
    }
    
    public class DownloadStatus : INotifyPropertyChanged {
        private int nbSeeders;
        public int Seeders { get { return nbSeeders; } set { nbSeeders = value;NotifyPropertyChanged(); } }
        private int nbPeers;
        public int Peers { get { return nbPeers; } set { nbPeers = value; NotifyPropertyChanged(); } }
        private double nbDownloadRate;
        public double DownloadRate { get { return nbDownloadRate; } set { nbDownloadRate = value; NotifyPropertyChanged(); } }
        private double nbProgres;
        public double Progress { get { return nbProgres; } set { nbProgres = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class DownloadMovie : INotifyPropertyChanged {
        private Uri _Source;
        public Uri Source { get { return _Source; } set { _Source = value; NotifyPropertyChanged(); } }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
