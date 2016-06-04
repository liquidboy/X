using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class SketchPageLayerXamlFragmentDataModel : BaseDataModel, ISketchPageLayerXamlFragmentDataModel, ISqliteBase
    {
        public string Index1 { get; set; }
        public int SketchPageLayerId { get; set; }
        public string Xaml { get; set; }
        public string Namespaces { get; set; }
        public string Resources { get; set; }
        public string Data { get; set; }
        public string DataType { get; set; }
    }
}
