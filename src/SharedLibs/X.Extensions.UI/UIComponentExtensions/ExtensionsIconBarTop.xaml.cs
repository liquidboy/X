using CoreLib.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WeakEvent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using X.Browser;
using X.Services.Data;
using X.Services.ThirdParty;
using X.UI.Toolbar;

namespace X.Extensions.UIComponentExtensions
{
    public sealed partial class ExtensionsIconBarTop : UserControl, IExtension
    {
        public ObservableCollection<ExtensionViewModel> Extensions { get; set; }
        
        public ExtensionsIconBarTop()
        {
            this.InitializeComponent();
            ExtensionManifest = new ExtensionManifest("Bottom Extensions Toolbar", string.Empty, "Sample Extensions", "1.0", "A UI to manage all the installed extensions in the Top Toolbar", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);
            layoutRoot.DataContext = this;
        }

        private void LoadExtensions()
        {
            _SendMessageSource?.Raise(this, new RequestListOfTopToolbarExtensionsEventArgs() { ReceiverType = ExtensionType.UIComponent });
            
        }

        public async void InstallMyself() {
            
            await X.Services.Extensions.ExtensionsService.Instance.Install(this);
            
            var el = new _Template(new ExtensionManifest("Installed Extensions UI", "ms-appx:///Extensions/UIComponentExtensions/InstalledExtensionList/InstalledExtensionList.png", "Sample Extensions", "1.0", "A UI to manage all the installed extensions", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.Right) { ContentControl = "X.Extensions.UIComponentExtensions.InstalledExtensionList", AssemblyName = "X.Extensions.UI" });
            X.Services.Extensions.ExtensionsService.Instance.Install(el);
            butExtensionStore.ExtensionUniqueId = el.ExtensionManifest.UniqueID;
            
            LoadExtensions();

        }

        public void UnloadExtensions()
        {

            //foreach (var ext in spExtensions.Children)
            //{
            //    ext.PointerReleased -= NewIcon_PointerReleased;
            //}

            //spExtensions.Children.Clear();
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
            if (message is ResponseListOfTopToolbarExtensionsEventArgs)
            {
                var ea = (ResponseListOfTopToolbarExtensionsEventArgs)message;

                if (Extensions == null) Extensions = new ObservableCollection<ExtensionViewModel>();
                else Extensions.Clear();

                var extensionsInStorage = X.Services.Data.StorageService.Instance.Storage.RetrieveList<ExtensionManifestDataModel>();
                foreach (var ext in ea.ExtensionsMetadata) {
                    var uid = FlickrNet.UtilityMethods.MD5Hash(ext.Title);
                    var found = extensionsInStorage.Where(x => x.Uid == uid).ToList();
                    if (found != null && found.Count() > 0)
                    {
                        if (found.First().IsExtEnabled) spExtensions.AddItem(ext.IconUrl, 20, Guid.Parse((string)ext.UniqueID));
                    }
                    else spExtensions.AddItem(ext.IconUrl, 20, Guid.Parse((string)ext.UniqueID));
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
            _SendMessageSource?.Raise(this, new LaunchExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ((ImageButton)sender).ExtensionUniqueId });
        }



        private void butExtensionStore_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _SendMessageSource?.Raise(this, new LaunchExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ((ImageButton)butExtensionStore).ExtensionUniqueId });
            //LoadExtensions();
        }

        private void spExtensions_Click(object sender, RoutedEventArgs e)
        {
            if (e is ToolbarRoutedEventArgs) {
                _SendMessageSource?.Raise(this, new LaunchExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ((ToolbarRoutedEventArgs)e).UniqueGuid });
            }

        }




    }
}
