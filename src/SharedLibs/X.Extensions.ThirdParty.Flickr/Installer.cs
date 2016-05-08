using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.Flickr
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("FlickrX", "ms-appx:///Extensions/ThirdParty/Flickr/Flickr.png", "FlickrX", "1.0", "Flickr X", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left) { ContentControl = "X.Extensions.ThirdParty.Flickr.Content", AssemblyName= "X.Extensions.ThirdParty.Flickr", IsExtEnabled = false };
        }
    }
}
