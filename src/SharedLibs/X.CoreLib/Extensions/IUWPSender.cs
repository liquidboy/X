using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace CoreLib.Extensions
{
    public interface IUWPSender
    {
        Task<IEnumerable<ValueSet>> MakeCall(string commandCall = "UI", string serviceName = "Call");
        IExtensionLite FindExtensionLiteInstance(string appExtensionDisplayName);
    }
}
