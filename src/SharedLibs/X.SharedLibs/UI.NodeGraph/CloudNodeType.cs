using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.UI.NodeGraph
{
    public class CloudNodeType : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }

        public CloudNodeType() { }
        public CloudNodeType(string name, DateTime createdDate, DateTime lastUpdated)
        {
            Name = name;
            CreatedDate = createdDate;
            LastUpdated = lastUpdated;
        }
    }
}
