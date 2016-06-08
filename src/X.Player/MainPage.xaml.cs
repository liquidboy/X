using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Player
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        public async void ProcessArguments(string arguments, string tileId)
        {
          
            await InitExtensions();
            
        }


        async Task InitExtensions()
        {
            LoadThirdPartyExtensions(new List<ExtensionManifest>{
                X.Extensions.FirstParty.Settings.Installer.GetManifest(),
            });

            await X.Services.Extensions.ExtensionsService.Instance.PopulateAllUWPExtensions();
            UpdateUWPExtensionsWithStateSavedData(X.Services.Extensions.ExtensionsService.Instance.GetUWPExtensions());

        }

        void UnInitExtensions()
        {
            X.Services.Extensions.ExtensionsService.Instance.UnloadExtensions();
        }

        //exactly same as x.browser
        private void LoadThirdPartyExtensions(List<ExtensionManifest> thirdPartyExtensions)
        {
            var extensionsInStorage = X.Services.Data.StorageService.Instance.Storage.RetrieveList<Services.Data.ExtensionManifestDataModel>();
            foreach (var ext in thirdPartyExtensions)
            {
                var found = extensionsInStorage.Where(x => x.Uid == ext.TitleHashed).ToList();
                if (found != null && found.Count() > 0)
                {
                    ext.IsExtEnabled = found.First().IsExtEnabled;
                    ext.LaunchInDockPositions = (ExtensionInToolbarPositions)found.First().LaunchInDockPositions;
                    ext.FoundInToolbarPositions = (ExtensionInToolbarPositions)found.First().FoundInToolbarPositions;
                }
                X.Services.Extensions.ExtensionsService.Instance.Install(ext);
            }
        }

        //exactly same as x.browser
        private void UpdateUWPExtensionsWithStateSavedData(IEnumerable<IExtensionLite> thirdPartyExtensions)
        {
            var extensionsInStorage = X.Services.Data.StorageService.Instance.Storage.RetrieveList<Services.Data.ExtensionManifestDataModel>();
            foreach (var ext in thirdPartyExtensions)
            {
                var hashedUid = ((X.Services.Extensions.ExtensionManifest)ext.Manifest).TitleHashed;
                var found = extensionsInStorage.Where(x => x.Uid == hashedUid).ToList();
                if (found != null && found.Count() > 0)
                {
                    ext.Manifest.IsExtEnabled = found.First().IsExtEnabled;
                    ext.Manifest.LaunchInDockPositions = (ExtensionInToolbarPositions)found.First().LaunchInDockPositions;
                    ext.Manifest.FoundInToolbarPositions = (ExtensionInToolbarPositions)found.First().FoundInToolbarPositions;
                }
            }

        }
    }
}
