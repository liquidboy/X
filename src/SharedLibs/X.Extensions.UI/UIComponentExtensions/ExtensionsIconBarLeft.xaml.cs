using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeakEvent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using X.Browser;
using X.Services.Data;
using X.UI.Toolbar;

namespace X.Extensions.UIComponentExtensions
{
    public sealed partial class ExtensionsIconBarLeft : UserControl, IExtension
    {
        public ExtensionsIconBarLeft()
        {
            this.InitializeComponent();

            ExtensionManifest = new ExtensionManifest("Left Extensions Toolbar", string.Empty, "Sample Extensions", "1.0", "A UI to manage all the installed extensions in the Left Toolbar", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);
        }

        private void LoadExtensions()
        {
            _SendMessageSource?.Raise(this, new RequestListOfLeftToolbarExtensionsEventArgs() { ReceiverType = ExtensionType.UIComponent });
            
        }

        public async void InstallMyself() {
            
            await X.Services.Extensions.ExtensionsService.Instance.Install(this);

            LoadExtensions();

        }

        public void UnloadExtensions() {


        }




        //IEXTENSION
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.UIComponent;
        public string Path { get; set; }


        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = false;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }

        private IList<IExtensionManifest> _extensions;

        public void RecieveMessage(object message)
        {
            if (message is ResponseListOfLeftToolbarExtensionsEventArgs)
            {
                spExtensions.ClearAll();
                var ea = (ResponseListOfLeftToolbarExtensionsEventArgs)message;
                _extensions = ea.ExtensionsMetadata;
                //RefreshExtensionsFromStorage();
                foreach (var ext in _extensions) if (ext.IsExtEnabled) spExtensions.AddItem(ext.IconBitmap != null ? "bitmap" : ext.IconUrl, 20, ext.UniqueID, ext.IconBitmap);
                if (_extensions.Where(x => x.IsExtEnabled).Count() > 0) this.Width = 40;
                else this.Width = 0;
            }
        }


        public void OnPaneLoad()
        {

        }

        public void OnPaneUnload()
        {

        }


        private void NewIcon_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _SendMessageSource?.Raise(this, new LaunchExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ((ImageButton)sender).ExtensionUniqueId });
        }

        private void spExtensions_Click(object sender, RoutedEventArgs e)
        {
            if (e is ToolbarRoutedEventArgs)
            {
                _SendMessageSource?.Raise(this, new LaunchExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ((ToolbarRoutedEventArgs)e).UniqueGuid });
            }
        }
    }
}
