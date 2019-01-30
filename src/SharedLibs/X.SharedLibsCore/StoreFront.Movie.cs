//using Microsoft.EntityFrameworkCore;
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
        private readonly MovieService _movieService;

        

        public ObservableCollection<MovieLightJson> Movies { get; set; }
        public ObservableCollection<MovieLightJson> MoviesSimilar { get; set; }
        public MovieJson Movie { get; set; }
        public int currentMoviePage = 1;
        public int MoviesCount { get; set; }




        private void SetupMovies() {
            Movies = new ObservableCollection<MovieLightJson>();
            MoviesSimilar = new ObservableCollection<MovieLightJson>();
        }
                
        public async Task LoadMovies(bool reset = false, int noItemsPerPage = 40, SortBy sortBy = SortBy.moviePopular)
        {
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
        
        public async Task LoadMovie(string imdbId)
        {
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
        

        public async Task<string> LoadMovieTrailer(string imdbId)
        {
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


        public async Task InitializeMovie()
        {
            var downloader = new Downloader() { Key = "movie", DownloadServiceMovie = new DownloadMovieService<MovieJson>(_cacheService) };
            Downloaders.Add(downloader);
        }

        public async Task WatchMovie(MovieJson movie, string torrentPath, Stream torrentStream, bool reset = false)
        {
            var downloader = Downloaders.Where(x => x.Key == "movie").FirstOrDefault();
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Movies).Token);

            geResultsWatcher.Start();

            downloader.ThumbUri = new Uri(Movie.PosterImage);

            var torrentUrl = movie.Torrents?.FirstOrDefault(torrent => torrent.Quality == "720p")?.Url;


            //var result = await DownloadFileHelper.DownloadFileTaskAsync(torrentUrl, _cacheService.MovieTorrentDownloads + Movie.ImdbId + ".torrent");
            var result = await DownloadFileHelper.DownloadStreamTaskAsync(torrentUrl, torrentStream);

            var reportDownloadProgress = new Progress<double>(downloader.ReportMovieDownloadProgress);
            var reportDownloadRate = new Progress<BandwidthRate>(downloader.ReportMovieDownloadRate);
            var reportNbPeers = new Progress<int>(downloader.ReportNbPeers);
            var reportNbSeeders = new Progress<int>(downloader.ReportNbSeeders);


            downloader.DownloadServiceMovie.Download(Movie, TorrentType.File, MediaType.Movie, torrentPath, 0, 0, reportDownloadProgress, reportDownloadRate, reportNbSeeders, reportNbPeers, () => { CurrentDownloadingMove.Source = new Uri(Movie.FilePath); }, () => { }, GetCancellationTokenSource(CancellationTokenTypes.Movies));
            
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
        
    }
    
}
