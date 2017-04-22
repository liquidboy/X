using CoreLib.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using X.Services.Data;

namespace X.Browser
{
    public class ExtensionViewModel : ViewModelBase, IExtensionManifest
    {
        public int Id { get; set; }
        public Guid UniqueID { get; set; } 
        public string Title { get; set; }
        public string TitleHashed { get { return FlickrNet.UtilityMethods.MD5Hash(Title); } private set { } }
        public string Path { get { return "//"; } set { } }
        public string IconUrl { get; set; }
        public BitmapImage IconBitmap { get; set; }
        public object Icon {
            get {
                if (IconUrl == "bitmap") return IconBitmap;
                else return IconUrl;
            }
            set { }
        }
        public string Publisher { get; set; }
        public string Version { get; set; }
        public string ContentControl { get; set; }
        public string AssemblyName { get; set; }
        public string DisplayName { get { return Title; } set { } }
        public string Abstract { get; set; }
        public bool IsExtEnabled { get; set; } = true;
        public bool IsUWPExtension { get; set; } = false;
        public bool CanUninstall { get; set; } = true;
        public ExtensionInToolbarPositions FoundInToolbarPositions { get; set; }
        public ExtensionInToolbarPositions LaunchInDockPositions { get; set; }
        public string AppExtensionUniqueID { get; set; }
        public int Size { get; set; }

        public void Load(IExtensionManifest extensionManifest) {
            UniqueID = extensionManifest.UniqueID;
            Title = extensionManifest.Title;
            Path = extensionManifest.Path;
            IconUrl = extensionManifest.IconUrl;
            Publisher = extensionManifest.Publisher;
            Version = extensionManifest.Version;
            IsExtEnabled = extensionManifest.IsExtEnabled;
            IsUWPExtension = extensionManifest.IsUWPExtension;
            CanUninstall = extensionManifest.CanUninstall;

            FoundInToolbarPositions = extensionManifest.FoundInToolbarPositions;
            LaunchInDockPositions = extensionManifest.LaunchInDockPositions;
            AppExtensionUniqueID = extensionManifest.AppExtensionUniqueID;

            IconUrl = extensionManifest.IconUrl;
            IconBitmap = extensionManifest.IconBitmap;
        }

        public void Load(ExtensionManifestDataModel extensionManifest) {
            Id = extensionManifest.Id;
            IsExtEnabled = extensionManifest.IsExtEnabled;
            LaunchInDockPositions = (ExtensionInToolbarPositions)extensionManifest.LaunchInDockPositions;
            FoundInToolbarPositions = (ExtensionInToolbarPositions)extensionManifest.FoundInToolbarPositions;
        }

        public void ExternalRaisePropertyChanged(string propName) { RaisePropertyChanged(propName); }


    }
}
