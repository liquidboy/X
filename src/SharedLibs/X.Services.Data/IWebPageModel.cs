using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public interface IWebPageModel : ISqliteBase
    {
        bool HasFocus { get; set; }
        string DisplayTitle { get; set; }
        string FaviconUri { get; set; }
        bool ShowPadlock { get; set; }


        string Uri { get; set; }

    }
}
