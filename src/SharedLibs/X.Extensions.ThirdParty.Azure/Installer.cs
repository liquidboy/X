using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.Azure
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Azure Management", "ms-appx:///Extensions/ThirdParty/Azure/Azure.png", "Jose Fajardo", "1.0", "Manage your Azure account", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull) { ContentControl = "X.Extensions.ThirdParty.Azure.Content", AssemblyName= "X.Extensions.ThirdParty.Azure", IsExtEnabled = false };
        }
    }
}
