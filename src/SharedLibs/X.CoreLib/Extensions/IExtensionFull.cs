using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppExtensions;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media.Imaging;

namespace CoreLib.Extensions
{
    public interface IExtensionFull
    {
        BitmapImage Logo { get; }
        string UniqueId { get; }
        string Version { get; }
        bool Offline { get; }
        AppExtension AppExtension { get; }
        bool Enabled { get; set; }
        bool Loaded { get; set; }
        Task Update(AppExtension ext);
        Task Load();
        Task Enable();
        void Unload();
        void Disable();
        IExtensionManifest Manifest {get;}
        Task<ValueSet> MakeCommandCall(string commandCall, string serviceName);
    }
}
