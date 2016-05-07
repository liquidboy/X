using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty.JsRTChakraCoreX
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("JsRT ChakraCore Shell", "ms-appx:///Extensions/ThirdParty/JsRTChakraCoreX/JsRTChakraCoreX.png", "Javascript Runtime ChakraCore X", "1.0", "Javascript Console", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Right) { ContentControl = "X.Extensions.ThirdParty.JsRTChakraCoreX.Content", AssemblyName= "X.Extensions.ThirdParty.JsRTChakraCoreX", IsExtEnabled = false };
        }
    }
}


//https://github.com/DerFlatulator/ChakraREPL
