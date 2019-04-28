using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.FileExplorer
{
    public class SavedAsset : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Grouping { get; set; }
        public string FileType { get; set; }
        public long FileSize{ get; set; }
        public bool HasThumb { get; set; }
        public bool IsInCloud { get; set; }

        public SavedAsset() { }
        public SavedAsset(string name, DateTime createdDate, DateTime lastUpdated, string grouping, string fileType, long fileSize, bool hasThumb)
        {
            Name = name;
            CreatedDate = createdDate;
            LastUpdated = lastUpdated;
            Grouping = grouping;
            FileType = fileType;
            FileSize = fileSize;
            HasThumb = hasThumb;
            IsInCloud = false; // only once it's stored in the cloud does it become "global"
        }
    }
}
