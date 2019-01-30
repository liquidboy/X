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
        
        private readonly ShowService _showService;
        
        public ObservableCollection<ShowLightJson> Shows { get; set; }
        public ObservableCollection<ShowLightJson> ShowsSimilar { get; set; }
        public ShowJson Show { get; set; }
        public int currentShowPage = 1;
        public int ShowsCount { get; set; }
        
        public void SetupShows()
        {
            Shows = new ObservableCollection<ShowLightJson>();
            ShowsSimilar = new ObservableCollection<ShowLightJson>();        
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

        public async Task InitializeShow() {
            var downloader = new Downloader() { Key = "show", DownloadServiceShow = new DownloadShowService<EpisodeShowJson>(_cacheService) };
            Downloaders.Add(downloader);
        }

        public async Task WatchEpisode(EpisodeShowJson episode, string torrentTemp, Stream torrentStream, bool reset = false)
        {
            var downloader = Downloaders.Where(x => x.Key == "show").FirstOrDefault();
            var geResultsWatcher = new Stopwatch();
            await LoadingSemaphore.WaitAsync(GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);

            geResultsWatcher.Start();

            downloader.ThumbUri = new Uri(Show.Images.Poster);

            var torrentUrl = episode.Torrents?.Torrent_480p.Url;
            if (torrentUrl == null) torrentUrl = episode.Torrents?.Torrent_720p.Url;
            if (torrentUrl == null) torrentUrl = episode.Torrents?.Torrent_1080p.Url;

            //var result = await DownloadFileHelper.DownloadStreamTaskAsync(torrentTemp, torrentStream);

            var reportDownloadProgress = new Progress<double>(downloader.ReportMovieDownloadProgress);
            var reportDownloadRate = new Progress<BandwidthRate>(downloader.ReportMovieDownloadRate);
            var reportNbPeers = new Progress<int>(downloader.ReportNbPeers);
            var reportNbSeeders = new Progress<int>(downloader.ReportNbSeeders);
            
            downloader.DownloadServiceShow.Download(episode, TorrentType.Magnet, MediaType.Show, torrentUrl, 0, 0, reportDownloadProgress, reportDownloadRate, reportNbSeeders, reportNbPeers, () => { CurrentDownloadingMove.Source = new Uri(episode.FilePath); }, () => { }, GetCancellationTokenSource(CancellationTokenTypes.Shows));

            geResultsWatcher.Stop();
            var ellapsedTime = geResultsWatcher.ElapsedMilliseconds;

            if (reset && ellapsedTime < 500)
            {
                // Wait for VerticalOffset to reach 0 (animation lasts 500ms)
                await Task.Delay(500 - (int)ellapsedTime, GetCancellationTokenSource(CancellationTokenTypes.Shows).Token);
            }

            LoadingSemaphore.Release();
        }
        
    }
    
}
