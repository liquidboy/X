using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Windows.Foundation;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using SumoNinjaMonkey.Framework;
using X.CoreLib.SQLite;

namespace X.CoreLib.Shared.Framework.Services.DataEntity
{
    public class BaseClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Type { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
