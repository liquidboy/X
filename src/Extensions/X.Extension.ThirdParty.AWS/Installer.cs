using CoreLib.Extensions;

namespace X.Extension.ThirdParty.Aws
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Aws Management", "ms-appx:///Extensions/ThirdParty/Aws/Aws.png", "Jose Fajardo", "1.0", "Manage your AWS account", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull) { ContentControl = "X.Extensions.ThirdParty.Aws.Content", AssemblyName= "X.Extensions.ThirdParty.Aws", IsExtEnabled = false };
        }
    }
}
