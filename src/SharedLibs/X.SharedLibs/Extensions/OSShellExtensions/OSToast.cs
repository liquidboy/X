using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakEvent;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace X.Extensions.Popups
{
    public class OSToast : IExtension
    {
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.OSShell;

        public string Path { get; set; }

        public OSToast() {
            ExtensionManifest = new ExtensionManifest("OS Toasts", string.Empty, "Sample Extensions", "1.0", "Bubble toasts to the OS which appear in the Action Center and as a floating Toast", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);

        }
        
        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = true;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }
        
        public void RecieveMessage(object message)
        {
            if (message is OSToastEventArgs)
            {
                X.Services.Toast.Service.CreateToast(((OSToastEventArgs)message).Text);
            }
        }

        public void OnPaneLoad()
        {

        }

        public void OnPaneUnload()
        {

        }
    }
}
