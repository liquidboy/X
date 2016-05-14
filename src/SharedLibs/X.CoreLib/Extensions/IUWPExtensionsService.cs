using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppExtensions;

namespace CoreLib.Extensions
{
    public interface IUWPExtensionsService
    {
        void InitializeCatalog();
        Task PopulateAllUWPExtensions();
        Task RemoveUWPExtensions(Package package);
        Task LoadUWPExtensions(Package package);
        Task UnloadUWPExtensions(Package package);
        Task LoadUWPExtension(AppExtension ext);
        IExtensionLite GetExtensionByAppExtensionUniqueId(string uniqueId);
        IEnumerable<IExtensionLite> GetUWPExtensions();
    }
}
