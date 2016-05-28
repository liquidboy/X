using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppExtensions;
using Windows.Foundation.Collections;

namespace CoreLib.Extensions
{
    public interface IExtensionLite
    {
        IExtensionManifest Manifest { get; set; }
        ExtensionType ExtensionType { get; set; }
        IExtension Extension { get; set; } //stores an internal PERSONAL representation of an extension (not UWP extension) - LEGACY
        bool IsShowingExtensionPanel { get; set; }

        string AppExtensionUniqueId { get; set; }
        AppExtension AppExtension { get; set; }  // stores the UWP representation of an extension

        Task<ValueSet> MakeUWPCommandCall(string commandCall, string serviceName);
        Task UpdateUWPExtension(AppExtension ext);
        void UnloadUWPExtension();
        Task LoadUWPExtension();
    }
}
