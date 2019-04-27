using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.FileExplorer
{
    public class SavedFolder : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Grouping { get; set; }

        public SavedFolder() { }
        public SavedFolder(string name, DateTime createdDate, DateTime lastUpdated, string grouping)
        {
            Name = name;
            CreatedDate = createdDate;
            LastUpdated = lastUpdated;
            Grouping = grouping;
        }
    }
}
