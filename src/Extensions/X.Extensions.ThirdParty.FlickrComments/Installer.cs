using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.FlickrComments
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("FlickrComments", "ms-appx:///Extensions/ThirdParty/FlickrComments/Flickr.png", "FlickrComments", "1.0", "Flickr Comments", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right) { ContentControl = "X.Extensions.ThirdParty.FlickrComments.Content", AssemblyName= "X.Extensions.ThirdParty.FlickrComments", IsExtEnabled = false };
        }
    }
}
