using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public interface IExtension
    {
        IExtensionManifest ExtensionManifest { get; set; }

        event EventHandler<EventArgs> SendMessage;
        
        //IGalleryInformation GalleryInformation { get; set; }

        string Path { get; set; }


        ExtensionType ExtensionType { get; set; }
        void RecieveMessage(object message);
        void OnPaneLoad();
        void OnPaneUnload();
        
    }
}
