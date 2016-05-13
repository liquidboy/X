using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppExtensions;

namespace CoreLib.Extensions
{
    public interface IExtensionsFullService
    {
        ObservableCollection<IExtensionFull> Extensions { get; }
        string Contract { get; }
        Task Initialize();
        Task FindAllExtensions();

        Task LoadExtension(AppExtension ext);
        Task LoadExtensions(Package package);
        Task UnloadExtensions(Package package);
        Task RemoveExtensions(Package package);
        void RemoveExtension(IExtensionFull ext);
    }
}
