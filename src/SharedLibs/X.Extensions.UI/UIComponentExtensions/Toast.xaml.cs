using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WeakEvent;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.Extensions;

namespace X.Extensions.UIComponentExtensions
{
    public sealed partial class Toast : UserControl, IExtension
    {
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.UIComponent;
        public string Path { get; set; }


        public Toast()
        {
            this.InitializeComponent();
            this.Visibility = Visibility.Collapsed;

            ExtensionManifest = new ExtensionManifest("Inline Toasts", string.Empty, "Sample Extensions", "1.0", "Show toast messages inline within the browser", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);

        }
        
        public void RecieveMessage(object message)
        {
            if (message is InlineToastEventArgs)
            {
                sbFadeInThenOut.Stop();
                tbText.Text = ((InlineToastEventArgs)message).Text;
                sbFadeInThenOut.Begin();
            }
        }

        public void OnPaneLoad()
        {

        }

        public void OnPaneUnload()
        {

        }

        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = true;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }



    }
}
