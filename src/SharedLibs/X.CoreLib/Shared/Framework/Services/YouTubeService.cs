using SumoNinjaMonkey.Framework.Collections;
using SumoNinjaMonkey.Framework.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace X.CoreLib.Shared.Services
{
    public class YouTubeService
    {
        

        private static YouTubeService youtubeInstance = null;
        private static object lo = new object();
        private static List<string> _queueOfRequests;

        public ObservableCollection<YoutubeDataItem> StandardFeedResults { get; set; }
        //public ObservableCollection<YoutubeDataItem> SearchFeedResults { get; set; }

        public static YouTubeService Current
        {
            get
            {
                YouTubeService result;
                lock (lo)
                {
                    if (YouTubeService.youtubeInstance == null)
                    {
                        YouTubeService.youtubeInstance = new YouTubeService();
                    }
                    result = YouTubeService.youtubeInstance;
                }
                return result;

                
            }

            
        }

        
        public async Task Init()
        {
            
            if (_queueOfRequests == null) _queueOfRequests = new List<string>();
            if (StandardFeedResults == null) StandardFeedResults = new ObservableCollection<YoutubeDataItem>();
            //if (SearchFeedResults == null) SearchFeedResults = new ObservableCollection<YoutubeDataItem>();
            
            //await RefreshDataFromCache("top_favorites");
           
            
            //await RefreshDataFromYouTube("top_favorites");
            //AppDatabase.Current.DeleteYoutubePersistedItems("top_favorites");
            //AppDatabase.Current.DeleteYoutubePersistedItems(DateTime.UtcNow);
            //AppDatabase.Current.DeleteYoutubeHistoryItems(DateTime.UtcNow);
            
            return;
        }

        public async Task RefreshDataFromCache(string grouping)
        {
            
            
            LoggingService.LogInformation("RefreshDataFromCache'", "YouTubeService.RefreshDataFromCache");

            var foundPlaying = StandardFeedResults.Where(x => !string.IsNullOrEmpty(x.UIStateFull));




            if (foundPlaying != null & foundPlaying.Count() > 0)
            {
                //StandardFeedResults.ToList().RemoveAll(x => x != foundPlaying.First());
                
                var tempUIDs = StandardFeedResults.Where(x => string.IsNullOrEmpty(x.UIStateFull)).Select(x => x.Uid).ToArray();
                string foundUID = foundPlaying.First().Uid;
                for (int i = 0; i < tempUIDs.Count(); i++)
                {
                    //if (tempUIDs[i] != foundUID) 
                    StandardFeedResults.Remove(StandardFeedResults.Where(x => x.Uid == tempUIDs[i] && string.IsNullOrEmpty(x.UIStateFull)).First());
                }
            }
            else
                StandardFeedResults.Clear();
            


            var found = AppDatabase.Current.RetrieveYoutubePersistedItemByGrouping(grouping);
            if (found != null && found.Count > 0)
            {

                //StandardFeedResults.ToList().RemoveAll(x => x._Grouping == grouping);
                
                foreach (var item in found)
                {
                    var ni = new YoutubeDataItem(item.Uid, item.Title, item.Subtitle, item.Image, item.Description, item.VideoId, null);
                    ni.ImagePath = item.ImagePath;
                    ni._Grouping = grouping;
                    ni.NewUID = item.NewUID;
                    StandardFeedResults.Add(ni);
                }

                return;
            }

        }

        public async Task RefreshDataFromYouTube(string grouping)
        {
            if (_queueOfRequests.Contains(grouping)) return;

            _queueOfRequests.Add(grouping);


            LoggingService.LogInformation("RefreshDataFromYouTube'", "YouTubeService.RefreshDataFromYouTube");
            
            var result = await YouTubeService.Current.GetStandardfeedAsync("favoriten", grouping);
            if (result != null)
            {
                //add new ones only
                foreach (var item in result.Items)
                {
                    var found = StandardFeedResults.Where(x=>x.Uid == item.Uid);
                    if (found == null || found.Count() == 0)
                    {
                        StandardFeedResults.Add(item);
                    }
                    
                }
                

                //delete cached data
                //AppDatabase.Current.DeleteYoutubePersistedItems(grouping);
               
                //persist to store
                var listToSave = StandardFeedResults.Select(x => new YoutubePersistedItem()
                {
                    Uid = x.Uid,
                    Description = x.Description,
                    Grouping = grouping,
                    ImagePath = x.ImagePath,
                    VideoLink = x.Link.AbsoluteUri.ToString(),
                    VideoId = x.videoID,
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    Timestamp = DateTime.UtcNow,
                    NewUID = Guid.NewGuid().ToString()

                }).ToList();


                foreach (var item in listToSave)
                {
                    if (!AppDatabase.Current.DoesYouTubePersistedItemExist(item.Uid))
                    {
                        AppDatabase.Current.AddUpdateYoutubePersistedItem(item);
                    }
                }
            }

            _queueOfRequests.Remove(grouping);

            return;
        }

        public async Task RefreshHistoryDataFromCache(string grouping)
        {
            try
            {
                //await Task.Run(() =>
                //{
                //await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                //{

                LoggingService.LogInformation("RefreshDataFromCache'", "YouTubeService.RefreshDataFromCache");

                var foundPlaying = StandardFeedResults.Where(x => !string.IsNullOrEmpty(x.UIStateFull));


                if (foundPlaying != null & foundPlaying.Count() > 0)
                {
                    //StandardFeedResults.ToList().RemoveAll(x => x != foundPlaying.First());
                    var tempUIDs = StandardFeedResults.Where(x => string.IsNullOrEmpty(x.UIStateFull)).Select(x => x.Uid).ToArray();
                    string foundUID = foundPlaying.First().Uid;
                    for (int i = 0; i < tempUIDs.Count(); i++)
                    {
                        //if (tempUIDs[i] != foundUID) 
                        StandardFeedResults.Remove(StandardFeedResults.Where(x => x.Uid == tempUIDs[i] && string.IsNullOrEmpty(x.UIStateFull)).First());
                    }
                }
                else
                    StandardFeedResults.Clear();


                var found = AppDatabase.Current.RetrieveYoutubeHistoryItemByGrouping(grouping);
                if (found != null && found.Count > 0)
                {
                    //StandardFeedResults.ToList().RemoveAll(x => x._Grouping == grouping);
                    found.Reverse();

                    foreach (var item in found)
                    {
                        var ytdi = new YoutubeDataItem(item.Uid, item.Title, item.Subtitle, item.ImagePath, item.Description, item.VideoId, null);

                        ytdi._Grouping = grouping;

                        //if (StandardFeedResults.Count() == 0)
                        StandardFeedResults.Add(ytdi);
                        //else
                        //    StandardFeedResults.Insert(0, ytdi);
                    }

                    return;
                }

                //});

                //});
            }
            catch(Exception ex) {
                AlertService.LogAlertMessage("Error retrieving History from Cache", ex.Message);
            }

            return;
        }

        public async Task<List<YoutubeDataItem>> GetRandomFromHistory(string grouping, int howMany = 5)
        {
            List<YoutubeDataItem> ret = new List<YoutubeDataItem>(); 
            try
            {
                LoggingService.LogInformation("RandomUserFavourites5'", "YouTubeService.RandomUserFavourites5");

                var found = AppDatabase.Current.RetrieveYoutubeHistoryItemByGrouping(grouping);
                if (found != null && found.Count > 0)
                {

                    Random rnd = new Random();
                    var randomizedList = from item in found.ToList()
                                         orderby rnd.Next()
                                         select item;

                    foreach (var item in randomizedList.Take(howMany))
                    {
                        var ytdi = new YoutubeDataItem(item.Uid, item.Title, item.Subtitle, item.ImagePath, item.Description, item.VideoId, null, localID: item.Id.ToString());
                        ytdi._Grouping = grouping;
                        ret.Add(ytdi);
                    }


                }

            }
            catch (Exception ex)
            {
                AlertService.LogAlertMessage("Error retrieving History from Cache", ex.Message);
            }

            return ret;
        }

        public async Task<YoutubeDataItem> GetFromHistory(string id)
        {
            YoutubeDataItem ret = null;
            try
            {
                LoggingService.LogInformation("GetFromHistory'", "YouTubeService.GetFromHistory");

                var found = AppDatabase.Current.RetrieveYoutubeHistoryItemByID(id);
                if (found != null && found.Count > 0)
                {

                    var item = found.First();

                    var ytdi = new YoutubeDataItem(item.Uid, item.Title, item.Subtitle, item.ImagePath, item.Description, item.VideoId, null);
                    ret = ytdi;
                    
                }

            }
            catch (Exception ex)
            {
                AlertService.LogAlertMessage("Error retrieving History from Cache", ex.Message);
            }

            return ret;
        }

        public async Task RefreshSearchFromYouTube(string Uid, string Keyword, int MaxResults, int Page = 0)
        {
            if (_queueOfRequests.Contains(Uid)) return ;

            _queueOfRequests.Add(Uid);

            await GetYoutubeDataGroupAsync(Uid, Keyword, MaxResults, Page);

            _queueOfRequests.Remove(Uid);
        }

        private async Task GetYoutubeDataGroupAsync(string Uid, string Keyword, int MaxResults, int Page = 0)
        {
            string text = string.Concat(new object[]
	        {
		        "http://gdata.youtube.com/feeds/api/videos?max-results=",
		        MaxResults,
		        "&v=2&q=",
		        Keyword,
                "&start-index=",
                ((Page * MaxResults) + 1)
	        });
            new Uri(text);
            YoutubeDataGroup result;
            try
            {
                YoutubeDataItem item = null;
                YoutubeDataGroup youtubeDataGroup = new YoutubeDataGroup(Uid, "Search: " + Keyword, Keyword, "ms-appx:///Assets/Darkgray.png", "");
                WebRequest webRequest = WebRequest.Create(text);
                webRequest.Method = "GET";
                WebResponse webResponse = await webRequest.GetResponseAsync();
                XDocument xDocument = XDocument.Load(webResponse.GetResponseStream());
                XmlReader xmlReader = xDocument.CreateReader();
                int num = 0;
                string title = string.Empty;
                string str = string.Empty;
                string str2 = string.Empty;
                string description = string.Empty;
                //SearchFeedResults.Clear();
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        string text2 = string.Empty;
                        string expr_1BE = xmlReader.Name;
                        if (expr_1BE != null)
                        {
                            if (!(expr_1BE == "yt:videoid"))
                            {
                                if (!(expr_1BE == "title"))
                                {
                                    if (!(expr_1BE == "media:credit"))
                                    {
                                        if (!(expr_1BE == "media:description"))
                                        {
                                            if (expr_1BE == "yt:duration")
                                            {
                                                int num2 = Convert.ToInt32(xmlReader["seconds"]);
                                                if (num2 > 60)
                                                {
                                                    int num3 = num2 / 60;
                                                    int num4 = num2 % 60;
                                                    if (num4 < 10)
                                                    {
                                                        str2 = num3.ToString() + ":0" + num4.ToString();
                                                    }
                                                    else
                                                    {
                                                        str2 = num3.ToString() + ":" + num4.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    str2 = "0:" + num2.ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            description = xmlReader.ReadElementContentAsString();
                                        }
                                    }
                                    else
                                    {
                                        str = xmlReader["yt:display"];
                                    }
                                }
                                else
                                {
                                    title = xmlReader.ReadElementContentAsString();
                                }
                            }
                            else
                            {
                                text2 = xmlReader.ReadElementContentAsString();
                                string imagePath = "http://i.ytimg.com/vi/" + text2 + "/hqdefault.jpg";
                                item = new YoutubeDataItem(num.ToString(), title, "by " + str + " | " + str2, imagePath, description, text2, youtubeDataGroup);
                                youtubeDataGroup.Items.Add(item);
                                StandardFeedResults.Add(item);
                            }
                        }
                    }
                    num++;
                }
                result = youtubeDataGroup;
            }
            catch (Exception)
            {
                result = null;
            }
            return;
        }

        private async Task<YoutubeDataItem> GetYoutubeDataItemAsync(string Uid, string videoID, YoutubeDataGroup group)
        {
            string text = "http://gdata.youtube.com/feeds/api/videos/" + videoID + "?v=2";
            new Uri(text);
            YoutubeDataItem result;
            try
            {
                YoutubeDataItem youtubeDataItem = null;
                WebRequest webRequest = WebRequest.Create(text);
                webRequest.Method = "GET";
                WebResponse webResponse = await webRequest.GetResponseAsync();
                XDocument xDocument = XDocument.Load(webResponse.GetResponseStream());
                XmlReader xmlReader = xDocument.CreateReader();
                int num = 0;
                string title = string.Empty;
                string str = string.Empty;
                string str2 = string.Empty;
                string description = string.Empty;
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        string text2 = string.Empty;
                        string expr_165 = xmlReader.Name;
                        if (expr_165 != null)
                        {
                            if (!(expr_165 == "yt:videoid"))
                            {
                                if (!(expr_165 == "title"))
                                {
                                    if (!(expr_165 == "media:credit"))
                                    {
                                        if (!(expr_165 == "media:description"))
                                        {
                                            if (expr_165 == "yt:duration")
                                            {
                                                int num2 = Convert.ToInt32(xmlReader["seconds"]);
                                                if (num2 > 60)
                                                {
                                                    int num3 = num2 / 60;
                                                    int num4 = num2 % 60;
                                                    if (num4 < 10)
                                                    {
                                                        str2 = num3.ToString() + ":0" + num4.ToString();
                                                    }
                                                    else
                                                    {
                                                        str2 = num3.ToString() + ":" + num4.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    str2 = "0:" + num2.ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            description = xmlReader.ReadElementContentAsString();
                                        }
                                    }
                                    else
                                    {
                                        str = xmlReader["yt:display"];
                                    }
                                }
                                else
                                {
                                    title = xmlReader.ReadElementContentAsString();
                                }
                            }
                            else
                            {
                                text2 = xmlReader.ReadElementContentAsString();
                                string imagePath = "http://i.ytimg.com/vi/" + text2 + "/hqdefault.jpg";
                                youtubeDataItem = new YoutubeDataItem(text2, title, "by " + str + " | " + str2, imagePath, description, text2, group);
                            }
                        }
                    }
                    num++;
                }
                result = youtubeDataItem;
            }
            catch //(Exception ex)
            {
                result = null;
            }
            return result;
        }

        private async Task<YoutubeDataGroup> GetStandardfeedAsync(string Uid, string standardfeed)
        {
            string text = "http://gdata.youtube.com/feeds/api/standardfeeds/" + standardfeed + "?v=2&max-results=25";
            new Uri(text);
            string title = string.Empty;
            string empty = string.Empty;
            switch (standardfeed)
            {
                case "top_rated":
                    title = "Top Rated";
                    break;
                case "top_favorites":
                    title = "Favorites";
                    break;
                case "most_viewed":
                    title = "Most Viewed";
                    break;
                case "most_shared":
                    title = "Most Shared";
                    break;
                case "most_popular":
                    title = "Most Popular";
                    break;
                case "most_recent":
                    title = "Most Recent";
                    break;
                case "most_discussed":
                    title = "Most Discussed";
                    break;
                case "most_responded":
                    title = "Most Responded";
                    break;
                case "recently_featured":
                    title = "Recently Featured";
                    break;
                case "on_the_web":
                    title = "Trends";
                    break;
            }
            YoutubeDataGroup result;
            try
            {
                YoutubeDataItem item = null;
                YoutubeDataGroup youtubeDataGroup = new YoutubeDataGroup(Uid, title, empty, "ms-appx:///Assets/Darkgray.png", "");
                string empty2 = string.Empty;
                WebRequest webRequest = WebRequest.Create(text);
                webRequest.Method = "GET";
                WebResponse webResponse = await webRequest.GetResponseAsync();
                XDocument xDocument = XDocument.Load(webResponse.GetResponseStream());
                XmlReader xmlReader = xDocument.CreateReader();
                int num2 = 0;
                string text2 = string.Empty;
                string str = string.Empty;
                string str2 = string.Empty;
                string description = string.Empty;
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        string text3 = string.Empty;
                        string expr_31D = xmlReader.Name;
                        if (expr_31D != null)
                        {
                            if (!(expr_31D == "yt:videoid"))
                            {
                                if (!(expr_31D == "title"))
                                {
                                    if (!(expr_31D == "media:credit"))
                                    {
                                        if (!(expr_31D == "media:description"))
                                        {
                                            if (expr_31D == "yt:duration")
                                            {
                                                int num3 = Convert.ToInt32(xmlReader["seconds"]);
                                                if (num3 > 60)
                                                {
                                                    int num4 = num3 / 60;
                                                    int num5 = num3 % 60;
                                                    if (num5 < 10)
                                                    {
                                                        str2 = num4.ToString() + ":0" + num5.ToString();
                                                    }
                                                    else
                                                    {
                                                        str2 = num4.ToString() + ":" + num5.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    str2 = "0:" + num3.ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            description = xmlReader.ReadElementContentAsString();
                                        }
                                    }
                                    else
                                    {
                                        str = xmlReader["yt:display"];
                                    }
                                }
                                else
                                {
                                    text2 = xmlReader.ReadElementContentAsString();
                                }
                            }
                            else
                            {
                                text3 = xmlReader.ReadElementContentAsString();
                                string text4 = "http://i.ytimg.com/vi/" + text3 + "/hqdefault.jpg";
                                if (empty2 == string.Empty)
                                {
                                    youtubeDataGroup.SetImage(text4);
                                }
                                item = new YoutubeDataItem(Uid + num2.ToString() + text2, text2, "by " + str + " | " + str2, text4, description, text3, youtubeDataGroup);
                                item._Grouping = standardfeed;
                                youtubeDataGroup.Items.Add(item);
                            }
                        }
                    }
                    num2++;
                }
                webRequest.Abort();
                webResponse.Dispose();
                webRequest = null;
                webResponse = null;
                xmlReader = null;
                xDocument = null;
                item = null;
                result = youtubeDataGroup;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        private async Task<YoutubeDataGroup> GetUserFavoriteFeedAsync(string Uid, string user)
        {
            string text = "http://gdata.youtube.com/feeds/api/users/" + user + "/favorites?v=2&max-results=25";
            new Uri(text);
            YoutubeDataGroup result;
            try
            {
                YoutubeDataItem item = null;
                YoutubeDataGroup youtubeDataGroup = new YoutubeDataGroup(Uid, user.ToUpper(), "", "ms-appx:///Assets/Darkgray.png", "");
                WebRequest webRequest = WebRequest.Create(text);
                webRequest.Method = "GET";
                WebResponse webResponse = await webRequest.GetResponseAsync();
                XDocument xDocument = XDocument.Load(webResponse.GetResponseStream());
                XmlReader xmlReader = xDocument.CreateReader();
                int num = 0;
                string title = string.Empty;
                string str = string.Empty;
                string str2 = string.Empty;
                string description = string.Empty;
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        string text2 = string.Empty;
                        string expr_193 = xmlReader.Name;
                        if (expr_193 != null)
                        {
                            if (!(expr_193 == "yt:videoid"))
                            {
                                if (!(expr_193 == "title"))
                                {
                                    if (!(expr_193 == "media:credit"))
                                    {
                                        if (!(expr_193 == "media:description"))
                                        {
                                            if (!(expr_193 == "yt:duration"))
                                            {
                                                if (expr_193 == "gd:rating")
                                                {
                                                    float.Parse(xmlReader["average"].ToString());
                                                }
                                            }
                                            else
                                            {
                                                int num2 = Convert.ToInt32(xmlReader["seconds"]);
                                                if (num2 > 60)
                                                {
                                                    int num3 = num2 / 60;
                                                    int num4 = num2 % 60;
                                                    if (num4 < 10)
                                                    {
                                                        str2 = num3.ToString() + ":0" + num4.ToString();
                                                    }
                                                    else
                                                    {
                                                        str2 = num3.ToString() + ":" + num4.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    str2 = "0:" + num2.ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            description = xmlReader.ReadElementContentAsString();
                                        }
                                    }
                                    else
                                    {
                                        str = xmlReader["yt:display"];
                                    }
                                }
                                else
                                {
                                    title = xmlReader.ReadElementContentAsString();
                                }
                            }
                            else
                            {
                                text2 = xmlReader.ReadElementContentAsString();
                                string imagePath = "http://i.ytimg.com/vi/" + text2 + "/hqdefault.jpg";
                                item = new YoutubeDataItem(Uid + num.ToString(), title, "by " + str + " | " + str2, imagePath, description, text2, youtubeDataGroup);
                                youtubeDataGroup.Items.Add(item);
                            }
                        }
                    }
                    num++;
                }
                webRequest.Abort();
                webResponse.Dispose();
                webRequest = null;
                webResponse = null;
                xmlReader = null;
                xDocument = null;
                item = null;
                result = youtubeDataGroup;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        
        

        



    }





    public class YoutubeDataCommon : BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");
        private string _Uid = string.Empty;
        private string _title = string.Empty;
        private string _subtitle = string.Empty;
        private string _description = string.Empty;
        private ImageSource _image;
        private string _imagePath;
        public string Uid
        {
            get
            {
                return this._Uid;
            }
            set
            {
                base.SetProperty<string>(ref this._Uid, value, "Uid");
            }
        }
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                base.SetProperty<string>(ref this._title, value, "Title");
            }
        }
        public string ImagePath
        {
            get
            {
                return this._imagePath;
            }
            set
            {
                base.SetProperty<string>(ref this._imagePath, value, "ImagePath");
            }
        }
        public string Subtitle
        {
            get
            {
                return this._subtitle;
            }
            set
            {
                base.SetProperty<string>(ref this._subtitle, value, "Subtitle");
            }
        }
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                base.SetProperty<string>(ref this._description, value, "Description");
            }
        }
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    if (this._imagePath.Contains("http://"))
                    {
                        Uri url = new Uri(this._imagePath);
                        this.LoadImageFromUriAsync(url);
                    }
                    else
                    {
                        this._image = new BitmapImage(new Uri(YoutubeDataCommon._baseUri, this._imagePath));
                    }
                }
                return this._image;
            }
            set
            {
                this._imagePath = null;
                base.SetProperty<ImageSource>(ref this._image, value, "Image");
            }
        }
        public YoutubeDataCommon(string Uid, string title, string subtitle, string imagePath, string description)
        {
            this._Uid = Uid;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }
        private async void LoadImageFromUriAsync(Uri url)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage result = httpClient.GetAsync(url).Result;
            byte[] array = await result.Content.ReadAsByteArrayAsync();
            InMemoryRandomAccessStream inMemoryRandomAccessStream = new InMemoryRandomAccessStream();
            DataWriter dataWriter = new DataWriter(inMemoryRandomAccessStream.GetOutputStreamAt(0uL));
            dataWriter.WriteBytes(array);
            await dataWriter.StoreAsync();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(inMemoryRandomAccessStream);
            this._image = bitmapImage;
            httpClient.Dispose();
            result.Dispose();
            array = null;
            inMemoryRandomAccessStream.Dispose();
            dataWriter.Dispose();
            bitmapImage = null;
        }
        public void SetImage(string path)
        {
            this._image = null;
            this._imagePath = path;
            base.OnPropertyChanged("Image");
        }
    }


    public class YoutubeDataGroup : YoutubeDataCommon
    {
        private ObservableCollection<YoutubeDataItem> _items = new ObservableCollection<YoutubeDataItem>();
        public ObservableCollection<YoutubeDataItem> Items
        {
            get
            {
                return this._items;
            }
        }
        public IEnumerable<YoutubeDataItem> TopItems
        {
            get
            {
                return this._items.Take(9);
            }
        }
        public YoutubeDataGroup(string Uid, string title, string subtitle, string imagePath, string description)
            : base(Uid, title, subtitle, imagePath, description)
        {
        }
    }


    public class YoutubeDataItem : YoutubeDataCommon
    {
        private string _localId = string.Empty;
        private string _videoID = string.Empty;
        private Uri _link;
        private YoutubeDataGroup _group;
        public string localID
        {
            get
            {
                return this._localId;
            }
            set
            {
                base.SetProperty<string>(ref this._localId, value, "localID");
            }
        }
        public string videoID
        {
            get
            {
                return this._videoID;
            }
            set
            {
                base.SetProperty<string>(ref this._videoID, value, "videoID");
            }
        }
        public Uri Link
        {
            get
            {
                return this._link;
            }
        }
        public YoutubeDataGroup Group
        {
            get
            {
                return this._group;
            }
            set
            {
                base.SetProperty<YoutubeDataGroup>(ref this._group, value, "Group");
            }
        }
        public YoutubeDataItem(string Uid, string title, string subtitle, string imagePath, string description, string videoID, YoutubeDataGroup group, string localID = "")
            : base(Uid, title, subtitle, imagePath, description)
        {
            this._localId = localID;
            this._videoID = videoID;
            this._group = group;
            this._link = new Uri("http://www.youtube.com/embed/" + videoID + "?rel=0&autoplay=1");
        }


        private int _uiState;
        public int UIState
        {
            get
            {
                return this._uiState;
            }
            set
            {
                base.SetProperty<int>(ref this._uiState, value, "UIState");
            }
        }

        private string _uiStateFull;
        public string UIStateFull
        {
            get
            {
                return this._uiStateFull;
            }
            set
            {
                base.SetProperty<string>(ref this._uiStateFull, value, "UIStateFull");
            }
        }

        public string _Grouping;
        
        
        private string _newUID;
        public string NewUID
        {
            get
            {
                return this._newUID;
            }
            set
            {
                base.SetProperty<string>(ref this._newUID, value, "NewUID");
            }
        }
    }




    
}
