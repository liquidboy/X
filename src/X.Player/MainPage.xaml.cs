using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace X.Player
{
    public sealed partial class MainPage : Page, IExtensionHost
    {
        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.UIComponent;
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public string Path { get; set; }



        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = false;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }


        public MainPage()
        {
            this.InitializeComponent();
        }
        
        public async void ProcessArguments(string arguments, string tileId)
        {
            await InitExtensions();
        }
        
        public async Task InitExtensions()
        {
            ExtensionManifest = new ExtensionManifest("X.Player", string.Empty, "X.Player", "1.0", "Generic X Extension Player", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);
            await X.Services.Extensions.ExtensionsService.Instance.Install(this);

            X.Extensions.UI.Shared.ExtensionUtils.LoadThirdPartyExtensions(new List<ExtensionManifest>{
                X.Extensions.FirstParty.Settings.Installer.GetManifest(),
            });

            await X.Services.Extensions.ExtensionsService.Instance.PopulateAllUWPExtensions();
            X.Extensions.UI.Shared.ExtensionUtils.UpdateUWPExtensionsWithStateSavedData(X.Services.Extensions.ExtensionsService.Instance.GetUWPExtensions());

            ctlExtensionsBarTop.InstallMyself(); 
        }
        
        public void UnInitExtensions()
        {
            X.Services.Extensions.ExtensionsService.Instance.UnloadExtensions();
            ctlExtensionsBarTop.UnloadExtensions();
        }

        public void CloseExtension(Guid extGuid)
        {
            throw new NotImplementedException();
        }

        public void LaunchExtension(Guid extGuid)
        {
            throw new NotImplementedException();
        }

        public void RecieveMessage(object message)
        {
            if (message is RequestListOfInstalledExtensionsEventArgs)
            {
                var extensions = X.Services.Extensions.ExtensionsService.Instance.GetExtensionsMetadata();
                _SendMessageSource?.Raise(this, new ResponseListOfInstalledExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfTopToolbarExtensionsEventArgs)
            {
                var extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Top);
                _SendMessageSource?.Raise(this, new ResponseListOfTopToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
        }

        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }


        public void OnPaneLoad() { }

        public void OnPaneUnload() { }
    }
}
