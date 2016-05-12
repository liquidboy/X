using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using X.Extensions.Popups;
using System.Reflection;
using WeakEvent;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppExtensions;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media.Imaging;
using X.Services.Settings;
using Windows.Storage;
using Windows.UI.Core;

namespace X.Services.Extensions
{
    public struct ExtensionLite
    {
        public IExtensionManifest Manifest;
        public ExtensionType ExtensionType;
        public IExtension Extension;
        public bool IsShowingExtensionPanel;
    }
    
}
