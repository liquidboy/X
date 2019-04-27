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

        public SavedAsset() { }
        public SavedAsset(string name, DateTime createdDate, DateTime lastUpdated, string grouping)
        {
            Name = name;
            CreatedDate = createdDate;
            LastUpdated = lastUpdated;
            Grouping = grouping;
        }
    }
}
