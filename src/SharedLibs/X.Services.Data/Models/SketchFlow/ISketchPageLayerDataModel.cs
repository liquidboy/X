using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public interface ISketchPageLayerDataModel : ISqliteBase
    {
        int SketchPageId { get; set; }

    }
}
