using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.OneDriveX
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("OneDriveX Management", "ms-appx:///Extensions/ThirdParty/OneDriveX/OneDriveX.png", "Jose Fajardo", "1.0", "Manage your OneDriveX account", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull) { ContentControl = "X.Extensions.ThirdParty.OneDriveX.Content", AssemblyName= "X.Extensions.ThirdParty.OneDriveX", IsExtEnabled = false };
        }
    }
}
