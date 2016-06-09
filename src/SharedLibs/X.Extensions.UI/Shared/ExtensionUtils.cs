using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.UI.Shared
{
    public static class ExtensionUtils
    {
        //exactly same as x.browser
        public static void LoadThirdPartyExtensions(List<ExtensionManifest> thirdPartyExtensions)
        {
            var extensionsInStorage = X.Services.Data.StorageService.Instance.Storage.RetrieveList<Services.Data.ExtensionManifestDataModel>();
            foreach (var ext in thirdPartyExtensions)
            {
                var found = extensionsInStorage.Where(x => x.Uid == ext.TitleHashed).ToList();
                UpdateManifest(found, ext);
                X.Services.Extensions.ExtensionsService.Instance.Install(ext);
            }
        }

        //exactly same as x.browser
        public static void UpdateUWPExtensionsWithStateSavedData(IEnumerable<IExtensionLite> thirdPartyExtensions)
        {
            var extensionsInStorage = X.Services.Data.StorageService.Instance.Storage.RetrieveList<Services.Data.ExtensionManifestDataModel>();
            foreach (var ext in thirdPartyExtensions)
            {
                var hashedUid = ((X.Services.Extensions.ExtensionManifest)ext.Manifest).TitleHashed;
                var found = extensionsInStorage.Where(x => x.Uid == hashedUid).ToList();
                UpdateManifest(found, ext.Manifest);
            }
        }

        //exactly same as x.browser
        private static void UpdateManifest(IList<Services.Data.ExtensionManifestDataModel> found, IExtensionManifest ext)
        {
            if (found != null && found.Count() > 0)
            {
                ext.IsExtEnabled = found.First().IsExtEnabled;
                ext.LaunchInDockPositions = (ExtensionInToolbarPositions)found.First().LaunchInDockPositions;
                ext.FoundInToolbarPositions = (ExtensionInToolbarPositions)found.First().FoundInToolbarPositions;
            }
        }
        
    }
}
