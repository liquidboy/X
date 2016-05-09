using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.Facebook
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Facebook", "ms-appx:///Extensions/ThirdParty/Facebook/Facebook.png", "Jose Fajardo", "1.0", "Facebook", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right) { ContentControl = "X.Extensions.ThirdParty.Facebook.Content", AssemblyName= "X.Extensions.ThirdParty.Facebook", IsExtEnabled = false };
        }
    }
}
