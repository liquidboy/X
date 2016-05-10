using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.FirstParty.Settings
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Settings Management", "ms-appx:///Extensions/FirstParty/Settings/Settings.png", "Jose Fajardo", "1.0", "Manage your Settings", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right) { ContentControl = "X.Extensions.FirstParty.Settings.Content", AssemblyName= "X.Extensions.FirstParty.Settings", IsExtEnabled = false };
        }
    }
}
