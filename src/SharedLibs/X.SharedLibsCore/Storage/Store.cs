using Microsoft.EntityFrameworkCore;
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
        protected CancellationTokenSource CancellationLoadingMovies { get; private set; }

        public Store() {
            
            using (var db = new BloggingContext()) {
                db.Database.Migrate();
            }

            var tmdbService = new TmdbService();
            _movieService = new MovieService(tmdbService);
            _showService = new ShowService(tmdbService);

            CancellationLoadingMovies = new CancellationTokenSource();
        }

        public async Task LoadStore(bool reset = false) {
            //await LoadMovies();
            await LoadShows();
        }

        public async Task LoadMovies(bool reset = false) {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(CancellationLoadingMovies.Token);

            geResultsWatcher.Start();
            var results = await _movieService.GetMoviesAsync(0,
                            30,
                            0d,
                            "seeds",
                            CancellationLoadingMovies.Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, CancellationLoadingMovies.Token);
            }
        }

        public async Task LoadShows(bool reset = false)
        {
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(CancellationLoadingMovies.Token);

            geResultsWatcher.Start();
            var results = await _showService.GetShowsAsync(0,
                            30,
                            0d,
                            "seeds",
                            CancellationLoadingMovies.Token);
            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;
            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, CancellationLoadingMovies.Token);
            }
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
