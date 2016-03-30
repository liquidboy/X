using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.GitX
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Git X", "ms-appx:///Extensions/ThirdParty/gitx/gitx.png", "Sample Extensions", "1.0", "GitHub Client", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Right) { ContentControl = "X.Extensions.ThirdParty.GitX.Content", AssemblyName= "X.Extensions.ThirdParty.GitX" , IsExtEnabled = false };
        }
    }
}
