using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public interface ISketchPageDataModel : ISqliteBase
    {
        string Title { get; set; }
        string Abstract { get; set; }
        int SketchId { get; set; }

        double Width { get; set; }
        double Height { get; set; }
        double Top { get; set; }
        double Left { get; set; }

    }
}
