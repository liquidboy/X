using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class SketchPageLayerDataModel : BaseDataModel, ISketchPageLayerDataModel, ISqliteBase
    {
        public string Index1 { get; set; }

        public int SketchPageId { get; set; }
    }
}
