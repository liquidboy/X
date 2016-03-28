using CoreLib.Extensions;
using System;
using WeakEvent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using X.UI.Toolbar;

namespace X.Extensions.UIComponentExtensions
{
    public sealed partial class ExtensionsIconBarBottom : UserControl, IExtension
    {
        
        public ExtensionsIconBarBottom()
        {
            this.InitializeComponent();

            ExtensionManifest = new ExtensionManifest("Bottom Extensions Toolbar", string.Empty, "Sample Extensions", "1.0", "A UI to manage all the installed extensions in the Bottom Toolbar", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);

        }

        private void LoadExtensions()
        {
            _SendMessageSource?.Raise(this, new RequestListOfBottomToolbarExtensionsEventArgs() { ReceiverType = ExtensionType.UIComponent });
            
        }

        public async void InstallMyself() {
            
            await X.Services.Extensions.ExtensionsService.Instance.Install(this);

            LoadExtensions();

        }

        public void UnloadExtensions()
        {

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
        
        public void RecieveMessage(object message)
        {
            if (message is ResponseListOfBottomToolbarExtensionsEventArgs)
            {
                var ea = (ResponseListOfBottomToolbarExtensionsEventArgs)message;
                
                foreach (var ext in ea.ExtensionsMetadata) {

                    spExtensions.AddItem(ext.IconUrl, 20, Guid.Parse((string)ext.UniqueID));
                }

                if (ea.ExtensionsMetadata.Count > 0) {
                    this.Height = 40;
                }
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
            _SendMessageSource?.Raise(this, new LaunchExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent , ExtensionUniqueGuid = ((ImageButton)sender).ExtensionUniqueId });
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
