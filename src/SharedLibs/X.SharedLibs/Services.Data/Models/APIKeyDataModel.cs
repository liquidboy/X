using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class APIKeyDataModel : BaseDataModel, IDataModel, ISqliteBase
    {
        public string Index1 { get; set; }

        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string APICallbackUrl { get; set; }
        public string APIName { get; set; }
        public string Type { get; set; }
        public string DeveloperUri { get; set; }
        
    }
}
