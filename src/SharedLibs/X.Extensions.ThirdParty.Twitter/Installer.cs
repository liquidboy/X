using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.Twitter
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Twitter", "ms-appx:///Extensions/ThirdParty/Twitter/Twitter.png", "Jose Fajardo", "1.0", "Twitter", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left) { ContentControl = "X.Extensions.ThirdParty.Twitter.Content", AssemblyName= "X.Extensions.ThirdParty.Twitter", IsExtEnabled = false };
        }
    }
}
