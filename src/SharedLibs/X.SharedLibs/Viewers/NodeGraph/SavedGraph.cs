using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.NodeGraph
{
    public class SavedGraph : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Category { get; set; }

        public SavedGraph() { }
        public SavedGraph(string name, DateTime createdDate, DateTime lastUpdated, string category)
        {
            Name = name;
            CreatedDate = createdDate;
            LastUpdated = lastUpdated;
            Category = category;
        }
    }
}
