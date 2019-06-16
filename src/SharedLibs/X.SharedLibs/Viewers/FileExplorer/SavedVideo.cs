using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.SharedLibs.Viewers.FileExplorer
{
    public class SavedVideo : BaseEntity
    {
        public string VideoId { get; set; }
        public string Author { get; set; }
        public DateTime UploadDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long DurationTicks { get; set; }
        public string ThumbnailMediumResUrl { get; set; }
        
        public string Grouping { get; set; }

        public SavedVideo() { }
        public SavedVideo(string videoId, string author, DateTime uploadDate, string title, string description,long durationTicks, string thumbnailMediumResUrl, string grouping)
        {
            VideoId = videoId;
            Author = author;
            UploadDate = uploadDate;
            Title = title;
            Description = description;
            DurationTicks = durationTicks;
            ThumbnailMediumResUrl = thumbnailMediumResUrl;
            Grouping = grouping;
        }
    }
}
