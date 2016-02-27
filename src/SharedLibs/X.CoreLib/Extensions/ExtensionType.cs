using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public enum ExtensionType
    {
        OSShell,

        UIComponent,
        UIComponentLazy,

        WVDOMContentLoaded,
        WVLoadCompleted,
        WVLongRunningScriptDetected,
        WVNewWindowRequest,
        WVNavigationCompleted,
        WVNavigationFailed,
        WVNavigationStarting,
        WVScriptNotify
    }
}
