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
    }
}
