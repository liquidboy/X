using Microsoft.EntityFrameworkCore;
using Popcorn.Models.Movie;
using Popcorn.Models.Shows;
using Popcorn.Services.Movies.Movie;
using Popcorn.Services.Shows.Show;
using Popcorn.Services.Tmdb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X.SharedLibsCore.Storage
{
    public class Store
    {
        MovieService _movieService;
        ShowService _showService;

        protected readonly SemaphoreSlim LoadingSemaphore = new SemaphoreSlim(1, 1);
        protected CancellationTokenSource[] CancellationLoading { get; private set; }

        private enum CancellationTokenTypes {
            Movies,
            Shows,
            TotalTypes
        }

        public Store() {
            
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

        public async Task LoadStore(bool reset = false) {
            await LoadMovies();
            await LoadTVShows();
        }

        public IEnumerable<MovieLightJson> Movies { get; set; }
        public MovieJson Movie { get; set; }
        public int MoviesCount { get; set; }
        public IEnumerable<ShowLightJson> Shows { get; set; }
        public ShowJson Show { get; set; }
        public int ShowsCount { get; set; }

        public async Task LoadMovies(bool reset = false) {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();
            var results = await _movieService.GetMoviesAsync(0,
                            30,
                            0d,
                            "seeds",
                            GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            Movies = results.movies;
            MoviesCount = results.nbMovies;
            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);
            }

            LoadingSemaphore.Release();
        }

        public async Task LoadTVShows(bool reset = false)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            geResultsWatcher.Start();
            var results = await _showService.GetShowsAsync(0,
                            30,
                            0d,
                            "seeds",
                            GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            Shows = results.shows;
            ShowsCount = results.nbShows;
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
