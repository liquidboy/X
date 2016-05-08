using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.Aws
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Aws Management", "ms-appx:///Extensions/ThirdParty/Aws/Aws.png", "Jose Fajardo", "1.0", "Manage your AWS account", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull) { ContentControl = "X.Extensions.ThirdParty.Aws.Content", AssemblyName= "X.Extensions.ThirdParty.Aws", IsExtEnabled = false };
        }
    }
}
