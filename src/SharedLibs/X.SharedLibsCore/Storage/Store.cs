using Microsoft.EntityFrameworkCore;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X.SharedLibsCore.Storage
{
    public class Store
    {
        private readonly MovieService _movieService;
        private readonly ShowService _showService;
        private IDownloadService<MovieJson> _movieDownloadService;
        private readonly ICacheService _cacheService;

        protected readonly SemaphoreSlim LoadingSemaphore = new SemaphoreSlim(1, 1);
        protected CancellationTokenSource[] CancellationLoading { get; private set; }

        
        public ObservableCollection<MovieLightJson> Movies { get; set; }
        public MovieJson Movie { get; set; }
        public int currentMoviePage = 1;
        public int MoviesCount { get; set; }


        public ObservableCollection<ShowLightJson> Shows { get; set; }
        public ShowJson Show { get; set; }
        public int currentShowPage = 1;
        public int ShowsCount { get; set; }

        private enum CancellationTokenTypes {
            Movies,
            Shows,
            TotalTypes
        }

        public Store() {
            Movies = new ObservableCollection<MovieLightJson>();
            Shows = new ObservableCollection<ShowLightJson>();

            _cacheService = new CacheService("X");
            using (var db = new BloggingContext()) {
                db.Database.Migrate();
            }

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


        

        public async Task LoadMovies(bool reset = false, int noItemsPerPage = 40) {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();
            var results = await _movieService.GetMoviesAsync(currentMoviePage,
                            noItemsPerPage,
                            0d,
                            "seeds",
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

        public async Task LoadTVShows(bool reset = false, int noItemsPerPage = 40)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            geResultsWatcher.Start();
            var results = await _showService.GetShowsAsync(currentShowPage,
                            noItemsPerPage,
                            0d,
                            "seeds",
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
                                reportDownloadRate, reportNbSeeders, reportNbPeers, () => { }, () => { },
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

        int NbSeeders = 0;
        private void ReportNbSeeders(int value) => NbSeeders = value;

        int NbPeers = 0;
        private void ReportNbPeers(int value) => NbPeers = value;

        double MovieDownloadRate;
        private void ReportMovieDownloadRate(BandwidthRate value) => MovieDownloadRate = value.DownloadRate;

        double MovieDownloadProgress;
        private void ReportMovieDownloadProgress(double value) => MovieDownloadProgress = value;



        private CancellationTokenSource GetCancellationTokenSource(CancellationTokenTypes type) {
            return CancellationLoading[(int)type];
        }
    }

    

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
