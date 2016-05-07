using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public interface IExtensionsService
    {
        event EventHandler<EventArgs> OnInstallExtension;
        event EventHandler<EventArgs> OnDidInstallExtension;
        event EventHandler<EventArgs> OnUninstallExtension;
        event EventHandler<EventArgs> OnDidUninstallExtension;
        
        Task<IExtension> Install(IExtension extension);
        Task<IExtensionManifest> Install(IExtensionManifest extension);
        IExtension Install(string zipPath);
        void UnInstall(IExtension extension);
        IExtension[] GetInstalled();

        IExtensionManifest GetExtensionMetadata(Guid guid);
        List<IExtensionManifest> GetExtensionsMetadata();
        List<IExtensionManifest> GetToolbarExtensionsMetadata(ExtensionInToolbarPositions position);

    }
}
