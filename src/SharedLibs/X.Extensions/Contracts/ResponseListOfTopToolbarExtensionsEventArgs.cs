using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions
{
    public class ResponseListOfTopToolbarExtensionsEventArgs : BaseEventArgs
    {
        public List<dynamic> ExtensionsMetadata;
    }
}
