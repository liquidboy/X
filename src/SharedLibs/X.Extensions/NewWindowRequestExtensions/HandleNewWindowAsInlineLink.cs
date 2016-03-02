using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakEvent;

namespace X.Extensions.Popups
{
    public class HandleNewWindowAsInlineLink : IExtension
    {
        public HandleNewWindowAsInlineLink() {

            ExtensionManifest = new ExtensionManifest("New Window Interceptor", string.Empty, "Sample Extensions", "1.0", "Listen to a webview's 'NewWindowRequested' event and pass it on to other extensions. This can be used by popup blockers etc.", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);

        }

        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.WVNewWindowRequest;
        public string Path { get; set; }

        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = true;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }
        
        public void RecieveMessage(object message)
        {
            if (message is Uri)
            {
                var toastMessage = "Popup detected. Loading as inline page!";
                _SendMessageSource?.Raise(this, new OSToastEventArgs() { Text = toastMessage, ReceiverType = ExtensionType.OSShell });
                _SendMessageSource?.Raise(this, new InlineToastEventArgs() { Text= toastMessage, ReceiverType = ExtensionType.UIComponent });
                _SendMessageSource?.Raise(this, new LoadWebViewEventArgs() { Uri = (Uri)message, ReceiverType = ExtensionType.UIComponent });
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
