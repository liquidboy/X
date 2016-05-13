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
    public struct ExtensionManifest : IExtensionManifest
    {
        public string Abstract { get; set; }

        public string AssemblyName { get; set; }

        public bool CanUninstall { get; set; }

        public string ContentControl { get; set; }

        public string DisplayName { get; set; }

        public ExtensionInToolbarPositions FoundInToolbarPositions { get; set; }

        public string IconUrl { get; set; }

        public bool IsExtEnabled { get; set; }
        
        public bool IsUWPExtension { get; set; }

        public ExtensionInToolbarPositions LaunchInDockPositions { get; set; }

        public string Publisher { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public Guid UniqueID { get; set; }

        public string Version { get; set; }

        public ExtensionManifest(IExtensionManifest data)
        {
            this.Abstract = data.Abstract;
            this.AssemblyName = data.AssemblyName;
            this.CanUninstall = data.CanUninstall;
            this.ContentControl = data.ContentControl;
            this.DisplayName = data.DisplayName;
            this.FoundInToolbarPositions = data.FoundInToolbarPositions;
            this.IconUrl = data.IconUrl;
            this.IsExtEnabled = data.IsExtEnabled;
            this.LaunchInDockPositions = data.LaunchInDockPositions;
            this.Publisher = data.Publisher;
            this.Title = data.Title;
            this.UniqueID = data.UniqueID;
            this.Version = data.Version;
            this.Path = data.Path;
            this.IsUWPExtension = data.IsUWPExtension;
        }

    }
    
}
