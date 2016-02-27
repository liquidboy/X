using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Framework.Data
{
    public abstract class BaseModel
    {
        public string UniqueID { get; set; }
        public int CacheID { get; set; }
    }
}
