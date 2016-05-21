using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public interface ISketchPageLayerXamlFragmentDataModel : ISqliteBase
    {
        int SketchPageLayerId { get; set; }
        string Xaml { get; set; }

    }
}
