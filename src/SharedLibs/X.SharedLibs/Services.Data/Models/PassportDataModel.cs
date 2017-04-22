using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class PassportDataModel : BaseDataModel, IDataModel, ISqliteBase
    {
        public string Index1 { get; set; }

        public string PassType { get; set; }

        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string Verifier { get; set; }
        public DateTime TokenExpiry { get; set; }


        public string FullName { get; set; }
        public string ScreenName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int APIKeyFKID { get; set; }

    }
}
