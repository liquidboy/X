using System;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.NeonShell.Data
{
    public class Screen : BaseEntity
    {
        public string Title { get; set; }
        public DateTime DateStamp { get; set; }
        public Guid ScreenID { get; set; }
    }

    //public class ScreenContext : SQLiteDataEntity<Screen> { }
}
