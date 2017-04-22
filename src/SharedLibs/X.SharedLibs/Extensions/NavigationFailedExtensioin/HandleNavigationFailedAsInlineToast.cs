using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakEvent;

namespace X.Extensions.Popups
{
    public class HandleNavigationFailedAsInlineToast : IExtension
    {
        public HandleNavigationFailedAsInlineToast() {

            ExtensionManifest = new ExtensionManifest("Navigation Failed Interceptor", string.Empty, "Sample Extensions", "1.0", "Listen to a webview's 'NavigationFailed' event and pass it on to other extensions. etc.", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);

        }

        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.WVNavigationFailed;
        public string Path { get; set; }

        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = true;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }
        
        public void RecieveMessage(object message)
        {
            if (message is Viewer.ContentViewEventArgs)
            {
                var toastMessage = $"Navigation failed with error '{ ((Viewer.ContentViewEventArgs)message).ExtraDetails1 }' !";
                _SendMessageSource?.Raise(this, new InlineToastEventArgs() { Text= toastMessage, ReceiverType = ExtensionType.UIComponent });
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
