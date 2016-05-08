using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.Office365
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Office365 Management", "ms-appx:///Extensions/ThirdParty/Office365/Office365.png", "Jose Fajardo", "1.0", "Manage your Office365 account", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull) { ContentControl = "X.Extensions.ThirdParty.Office365.Content", AssemblyName= "X.Extensions.ThirdParty.Office365", IsExtEnabled = false };
        }
    }
}
