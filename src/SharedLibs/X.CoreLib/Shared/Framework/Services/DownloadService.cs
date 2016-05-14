using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using System.Collections.Generic;
using SumoNinjaMonkey.Framework.Services;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using System.Threading;

namespace X.CoreLib.Shared.Services
{

    public class DownloadService
    {
        private static DownloadService _downloadService = null;

        public event EventHandler DownloadCountChanged;

        //private int _downloadCount = 0;
        //public int DownloadCount {
        //    get { return _downloadCount; }
        //    set { _downloadCount = value; if (DownloadCountChanged != null)DownloadCountChanged(_downloadCount, EventArgs.Empty); }
        //}

        


        public static DownloadService Current
        {
            get
            {
                if (DownloadService._downloadService == null)
                {
                    DownloadService._downloadService = new DownloadService();
                }
                return DownloadService._downloadService;
            }
        }



        private DownloadService()
        {
        }

        private struct DownloadRequest{
            public string AggregateId { get; set; }
            public string Url { get; set; }
            public string BackupUrl { get; set; }
            public string FileName { get; set; }
            public int Type { get; set; }
            public string StorageFolder { get; set; }
        }

        private CancellationTokenSource _OneDownloadAtATimeCancellationToken;

        private Queue<DownloadRequest> _downloadVideoRequests = new Queue<DownloadRequest>();
        private Queue<DownloadRequest> _downloadPictureRequests = new Queue<DownloadRequest>();
        //private bool _isDownloadingVideo = false;
        //private bool _isDownloadingPicture = false;

        public async Task Downloader(string aggregateId, string url, string backupUrl, string fileName, int type, CancellationToken cancellationToken, string storageFolder = "ModernCSFx")
        {
            if (type == 1) //video download
            {
                //_downloadVideoRequests.Enqueue(new DownloadRequest()
                //{
                //    Url = url,
                //    AggregateId = aggregateId,
                //    FileName = fileName,
                //    StorageFolder = storageFolder,
                //    Type = type,
                //    BackupUrl = backupUrl
                //});
                var dr = new DownloadRequest()
                {
                    Url = url,
                    AggregateId = aggregateId,
                    FileName = fileName,
                    StorageFolder = storageFolder,
                    Type = type,
                    BackupUrl = backupUrl
                };
                await AttemptToDownloadVideoAsync(dr, cancellationToken);
            }
            else if (type == 2) //picture download
            {
                //_downloadPictureRequests.Enqueue(new DownloadRequest()
                //{
                //    Url = url,
                //    AggregateId = aggregateId,
                //    FileName = fileName,
                //    StorageFolder = storageFolder,
                //    Type = type,
                //    BackupUrl = backupUrl
                //});

                var dr = new DownloadRequest()
                {
                    Url = url,
                    AggregateId = aggregateId,
                    FileName = fileName,
                    StorageFolder = storageFolder,
                    Type = type,
                    BackupUrl = backupUrl
                };
                await AttemptToDownloadPictureAsync(dr, cancellationToken);
            }

        }

        private async Task AttemptToDownloadVideoAsync(DownloadRequest dr, CancellationToken cancellationToken)
        {
            //if (!_isDownloadingVideo && _downloadVideoRequests.Count() > 0)
            //{
            //DownloadCount++;
            //_isDownloadingVideo = true;
            //await ExecuteDownload(_downloadVideoRequests.Dequeue()).AsAsyncAction().AsTask(MakeIdemniPotentAsyncCall());
            await ExecuteDownload(dr, cancellationToken);
            //}
        }

        private async Task AttemptToDownloadPictureAsync(DownloadRequest dr, CancellationToken cancellationToken)
        {
            try {
                //if (!_isDownloadingPicture && _downloadPictureRequests.Count() > 0)
                //{
                //DownloadCount++;
                //_isDownloadingPicture = true;
                //await ExecuteDownload(_downloadPictureRequests.Dequeue()).AsAsyncAction().AsTask(MakeIdemniPotentAsyncCall());
                await ExecuteDownload(dr, cancellationToken).AsAsyncAction().AsTask(cancellationToken);
                //}
            }
            catch (Exception ex) {

            }

        }


        //private async Task AttemptToDownloadVideo()
        //{
        //    if (!_isDownloadingVideo && _downloadVideoRequests.Count() > 0)
        //    {
        //        //DownloadCount++;
        //        _isDownloadingVideo = true;
        //        await ExecuteDownload(_downloadVideoRequests.Dequeue());
        //    }
        //}

        //private async Task AttemptToDownloadPicture()
        //{
        //    if (!_isDownloadingPicture && _downloadPictureRequests.Count() > 0)
        //    {
        //        //DownloadCount++;
        //        _isDownloadingPicture = true;
        //        await ExecuteDownload(_downloadPictureRequests.Dequeue());
        //    }
        //}

        private async Task ExecuteDownload(DownloadRequest request, CancellationToken ct)
        {

            string[] partsUrl = request.Url.Split(".".ToCharArray());
            string fileNameToUse = request.FileName.Replace("\"", "").Replace(" ", "").Replace("'", "").Replace(".", "");

            if (request.Type == 1) //video download
            {
                string fileUri = request.FileName + ".mp4";
                string folderUri = request.StorageFolder;
                StorageFolder folder = Windows.Storage.KnownFolders.VideosLibrary;
                var appFolder = await folder.CreateFolderAsync(folderUri, CreationCollisionOption.OpenIfExists);

                StorageFile file = await FileExists(folderUri, fileUri, 1).AsAsyncOperation().AsTask(ct);
                
                bool isNewDownload = false;
                if (file == null)
                {
                    isNewDownload = true;
                    file = await appFolder.CreateFileAsync(fileUri, CreationCollisionOption.ReplaceExisting);

                    SendSystemWideMessage("DASHBOARD", "", action: "SEND INFORMATION NOTIFICATION", text1: "Download started '" + fileUri + "'", int1: 2);
                    using (var newFileStream = await file.OpenStreamForWriteAsync())
                    {
                        await SaveUrlContentToStorage(request.Url, newFileStream, ct);
                    }

                    
                }


                if (isNewDownload) SendSystemWideMessage("DASHBOARD", "", action: "SEND INFORMATION NOTIFICATION", text1: "Download complete '" + fileUri + "'", int1: 2);

                var vp = await file.Properties.GetVideoPropertiesAsync();
                AppDatabase.Current.UpdateUIElementStateField(request.AggregateId, "Width", vp.Width, sendAggregateUpdateMessage: false);
                AppDatabase.Current.UpdateUIElementStateField(request.AggregateId, "Height", vp.Height, sendAggregateUpdateMessage: false);
                AppDatabase.Current.UpdateUIElementStateField(request.AggregateId, "udfBool1", true); //file has been downloaded so tell the world its ok to start using it

                //_isDownloadingVideo = false;
                //DownloadCount--;

                //await AttemptToDownloadVideoAsync();
            }
            else if (request.Type == 2) //image
            {


                string fileUri = fileNameToUse + "." + partsUrl[partsUrl.Length - 1];
                string folderUri = request.StorageFolder;
                StorageFolder folder = Windows.Storage.KnownFolders.PicturesLibrary;
                var appFolder = await folder.CreateFolderAsync(folderUri, CreationCollisionOption.OpenIfExists);

                bool fileFound = false;
                try
                {
                    var foundFile = await appFolder.GetFileAsync(fileUri).AsTask(ct); 
                    using (var fs = await foundFile.OpenStreamForReadAsync().AsAsyncOperation().AsTask(ct))
                    {
                        fileFound = true;
                        fs.Dispose();   

                    }

                    foundFile = null;
                }
                catch (FileNotFoundException ex)
                {

                }
                catch (Exception ex)
                {
                    //unhandled exception

                    return;
                }


                //StorageFile file = await FileExists(folderUri, fileUri, request.Type);
                bool isNewDownload = false;

                if (!fileFound)
                {

                    isNewDownload = true;
                    var file = await appFolder.CreateFileAsync(fileUri, CreationCollisionOption.ReplaceExisting).AsTask(ct);
                    
                    //SendSystemWideMessage("DASHBOARD", "", action: "SEND INFORMATION NOTIFICATION", text1: "Download started '" + fileUri + "'", int1: 2);
                    using (var newFileStream = await file.OpenStreamForWriteAsync().AsAsyncOperation().AsTask(ct))
                    {
                        await SaveUrlContentToStorage(request.Url, newFileStream, ct).AsAsyncAction().AsTask(ct);
                    }

                    //var bp = await file.GetBasicPropertiesAsync().AsTask(ct);
                    //if (bp.Size < 1000)
                    //{
                    //    file = await appFolder.CreateFileAsync(fileUri, CreationCollisionOption.ReplaceExisting).AsTask(ct);
                    //    using (var newFileStream = await file.OpenStreamForWriteAsync().AsAsyncOperation().AsTask(ct))
                    //    {
                    //        await SaveUrlContentToStorage(request.BackupUrl, newFileStream, ct);

                    //        newFileStream.Dispose();

                    //    }
                    //}

                    file = null;

                }
                

                //if (isNewDownload) SendSystemWideMessage("DASHBOARD", "", action: "SEND INFORMATION NOTIFICATION", text1: "Download complete '" + fileUri + "'", int1: 2);



                //var vp = await file.Properties.GetImagePropertiesAsync();
                //AppDatabase.Current.UpdateUIElementStateField(aggregateId, "Width", vp.Width, sendAggregateUpdateMessage: false);
                //AppDatabase.Current.UpdateUIElementStateField(aggregateId, "Height", vp.Height, sendAggregateUpdateMessage: false);
                //AppDatabase.Current.UpdateUIElementStateField(aggregateId, "udfBool1", true); //file has been downloaded so tell the world its ok to start using it

                //_isDownloadingPicture = false;
                //DownloadCount--;

                //await AttemptToDownloadPictureAsync();
            }

            
        }

        private async Task<IReadOnlyList<StorageFile>> GetFilesAsync(string subFolder, int type = 0)
        {
            //var folder = ApplicationData.Current.LocalFolder;
            var parts = subFolder.Split("\\".ToCharArray());
            StorageFolder folderToUse = null;
            foreach (var part in parts)
            {
                StorageFolder tempFolder = null;
                if (folderToUse == null)
                {
                    if (type == 1) tempFolder = await KnownFolders.VideosLibrary.GetFolderAsync(part);
                    else if (type == 2) tempFolder = await KnownFolders.PicturesLibrary.GetFolderAsync(part);
                }
                else tempFolder = await folderToUse.GetFolderAsync(part);

                folderToUse = tempFolder;
            }
            //var folder = await KnownFolders.VideosLibrary.GetFolderAsync(subFolder);
            return await folderToUse.GetFilesAsync(CommonFileQuery.OrderByName)
                               .AsTask().ConfigureAwait(false);
        }

        //type 0 = video, 1 = picture
        private async Task<StorageFile> FileExists(string subFolder, string filename, int type = 0)
        {

            var files = await GetFilesAsync(subFolder, type);

            var file = files.FirstOrDefault(x => x.Name == filename);
            if (file != null)
            {
                return file;
                //return "ms-appdata:///local/" + filename;
            }

            files = null;
            return null;
        }


        private async Task SaveUrlContentToStorage(string url, Stream stream, CancellationToken ct)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                
                using (WebResponse response = await request.GetResponseAsync().AsAsyncOperation().AsTask(ct))
                {
                    using (Stream rs = response.GetResponseStream())
                    {
                        await rs.CopyToAsync(stream).AsAsyncAction().AsTask(ct);
                        rs.Dispose();
                    }
                    response.Dispose();
                }

                stream.Dispose();
                request = null;
                return;
            }
            catch (WebException ex)
            {
                return;
            }
        }



        public void SendSystemWideMessage(string identifier, string content, string sourceId = "", string action = "", string url1 = "", string aggregateId = "", string text1 = "", string text2 = "", int int1 = 2)
        {
            LoggingService.LogInformation("system message ... " + content, "DownloadService.SendSystemWideMessage");
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage(content) { Identifier = identifier, SourceId = sourceId, Url1 = url1, Action = action, AggregateId = aggregateId, Text1 = text1, Text2 = text2, Int1 = int1 });
        }



    }
}
