//using Microsoft.EntityFrameworkCore;
using NuGet;
using Popcorn.Comparers;
using Popcorn.Helpers;
using Popcorn.Models.Bandwidth;
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
    public class StoreFront
    {
        private readonly MovieService _movieService;
        private readonly ShowService _showService;
        private IDownloadService<MovieJson> _movieDownloadService;
        private readonly ICacheService _cacheService;

        protected readonly SemaphoreSlim LoadingSemaphore = new SemaphoreSlim(1, 1);
        protected CancellationTokenSource[] CancellationLoading { get; private set; }

        
        public ObservableCollection<MovieLightJson> Movies { get; set; }
        public ObservableCollection<MovieLightJson> MoviesSimilar { get; set; }
        public MovieJson Movie { get; set; }
        public int currentMoviePage = 1;
        public int MoviesCount { get; set; }


        public ObservableCollection<ShowLightJson> Shows { get; set; }
        public ObservableCollection<ShowLightJson> ShowsSimilar { get; set; }
        public ShowJson Show { get; set; }
        public int currentShowPage = 1;
        public int ShowsCount { get; set; }

        private enum CancellationTokenTypes {
            Movies,
            MoviesSimilar,
            Shows,
            ShowsSimilar,
            TotalTypes
        }

        public StoreFront() {
            Movies = new ObservableCollection<MovieLightJson>();
            MoviesSimilar = new ObservableCollection<MovieLightJson>();
            Shows = new ObservableCollection<ShowLightJson>();
            ShowsSimilar = new ObservableCollection<ShowLightJson>();
            DownloadProgress = new DownloadStatus();
            CurrentDownloadingMove = new DownloadMovie();

            _cacheService = new CacheService("X");
            //using (var db = new BloggingContext()) {
            //    db.Database.Migrate();
            //}

            var tmdbService = new TmdbService();
            _movieService = new MovieService(tmdbService);
            _showService = new ShowService(tmdbService);
            
            CancellationLoading = new CancellationTokenSource[(int)CancellationTokenTypes.TotalTypes];
            for (int counter = 0; counter < (int)CancellationTokenTypes.TotalTypes; counter++) {
                CancellationLoading[counter] = new CancellationTokenSource();
            }
        }

        public async Task InitializeFileSystem(string movieUrl) {
            _cacheService.LocalPath = movieUrl;
            _movieDownloadService = new DownloadMovieService<MovieJson>(_cacheService);
        }

        public async Task LoadStore(bool reset = false) {
            await LoadMovies();
            await LoadTVShows();
        }


        public enum SortBy {

            movieGreatest,
            moviePopular,
            movieRecent,

            showPopular,
            showGreatest,
            showRecent,
            showUpdated
        }


        private string getSortBy(SortBy sortBy) {

            switch (sortBy) {
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

        public async Task LoadMovies(bool reset = false, int noItemsPerPage = 40, SortBy sortBy = SortBy.moviePopular) {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();
            var results = await _movieService.GetMoviesAsync(currentMoviePage,
                            noItemsPerPage,
                            0d,
                            getSortBy(sortBy),
                            GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            Movies.AddRange(results.movies.Except(Movies, new MovieLightComparer()));
            MoviesCount = results.nbMovies;
            currentMoviePage++;
            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            }

            LoadingSemaphore.Release();
        }

        public async Task LoadTVShows(bool reset = false, int noItemsPerPage = 40, SortBy sortBy = SortBy.showPopular)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            geResultsWatcher.Start();
            var results = await _showService.GetShowsAsync(currentShowPage,
                            noItemsPerPage,
                            0d,
                            getSortBy(sortBy),
                            GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            Shows.AddRange(results.shows.Except(Shows, new ShowLightComparer()));
            ShowsCount = results.nbShows;
            currentShowPage++;
            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            }

            LoadingSemaphore.Release();
        }

        public async Task LoadMovie(string imdbId) {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();

            Movie = await _movieService.GetMovieAsync(imdbId, GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            }

            LoadingSemaphore.Release();
        }
        
        public async Task LoadTVShow(string imdbId)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            geResultsWatcher.Start();

            Show = await _showService.GetShowAsync(imdbId, GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            }

            LoadingSemaphore.Release();
        }

        public async Task<string> LoadMovieTrailer(string imdbId) {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();
            var trailer = await _movieService.GetMovieTrailerAsync(imdbId,
                            GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            }
            LoadingSemaphore.Release();

            return trailer;
        }

        public async Task<string> LoadShowTrailer(int tmdbId)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            geResultsWatcher.Start();
            var trailer = await _showService.GetShowTrailerAsync(tmdbId,
                            GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            }
            LoadingSemaphore.Release();

            return trailer;
        }


        public async Task LoadSimilarMovies(MovieJson movie)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.MoviesSimilar).Token);

            geResultsWatcher.Start();

            MoviesSimilar.Clear();
            var results = await _movieService.GetMoviesSimilarAsync(movie, GetCancellationTokenSource(CancellationTokenTypes.MoviesSimilar).Token);
            MoviesSimilar.AddRange(results);

            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.MoviesSimilar).Token);
            }

            LoadingSemaphore.Release();
        }
        
        public async Task WatchMovie(MovieJson movie, string torrentPath, Stream torrentStream, bool reset = false)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();
            
            //var torrentUrl = movie.WatchInFullHdQuality
            //                ? movie.Torrents?.FirstOrDefault(torrent => torrent.Quality == "1080p")?.Url
            //                : movie.Torrents?.FirstOrDefault(torrent => torrent.Quality == "720p")?.Url;
            var torrentUrl = movie.Torrents?.FirstOrDefault(torrent => torrent.Quality == "720p")?.Url;


            //var result = await DownloadFileHelper.DownloadFileTaskAsync(torrentUrl, _cacheService.MovieTorrentDownloads + Movie.ImdbId + ".torrent");
            var result = await DownloadFileHelper.DownloadStreamTaskAsync(torrentUrl, torrentStream);

            var reportDownloadProgress = new Progress<double>(ReportMovieDownloadProgress);
            var reportDownloadRate = new Progress<BandwidthRate>(ReportMovieDownloadRate);
            var reportNbPeers = new Progress<int>(ReportNbPeers);
            var reportNbSeeders = new Progress<int>(ReportNbSeeders);


            await _movieDownloadService.Download(Movie, TorrentType.File, MediaType.Movie, torrentPath,
                                0, 0, reportDownloadProgress,
                                reportDownloadRate, reportNbSeeders, reportNbPeers, () => { CurrentDownloadingMove.Source = new Uri(Movie.FilePath); }, () => { },
                                GetCancellationTokenSource(CancellationTokenTypes.Movies));



            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            //Movies = results.movies;
            //MoviesCount = results.nbMovies;
            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            }

            LoadingSemaphore.Release();
        }

        private void ReportNbSeeders(int value) => DownloadProgress.Seeders = value;

        private void ReportNbPeers(int value) => DownloadProgress.Peers = value;

        private void ReportMovieDownloadRate(BandwidthRate value) => DownloadProgress.DownloadRate = value.DownloadRate;

        private void ReportMovieDownloadProgress(double value) => DownloadProgress.Progress = value;

        public DownloadStatus DownloadProgress { get; set; }
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
