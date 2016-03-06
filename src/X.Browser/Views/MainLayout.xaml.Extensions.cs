using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakEvent;
using X.Extensions.ThirdParty;

namespace X.Browser.Views
{
    partial class MainLayout
    {
        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.UIComponent;
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public string Path { get; set; }



        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = false;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }



        void InitExtensions()
        {
            ExtensionManifest = new ExtensionManifest("Browser Shell", string.Empty, "Sample Extensions", "1.0", "The chrome of the browser is itself an extension. Enabling/Disabling this will affect ALL extensions.", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);




            App.ExtensionsSvc.Install(this);
            //App.ExtensionsSvc.Install(ctlToast);


            //Find a way to reflect this in
            Installer.GetExtensionManifests().ForEach(x => { App.ExtensionsSvc.Install(x); });
            App.ExtensionsSvc.Install(X.Extensions.ThirdParty.GitX.Installer.GetManifest());


            ctlExtensionsBarTop.InstallMyself(App.ExtensionsSvc); // does Install + LoadExtensions
            ctlExtensionsBarLeft.InstallMyself(App.ExtensionsSvc); // does Install + LoadExtensions
            ctlExtensionsBarRight.InstallMyself(App.ExtensionsSvc); // does Install + LoadExtensions
            ctlExtensionsBarBottom.InstallMyself(App.ExtensionsSvc); // does Install + LoadExtensions

            //Messenger.Default.Register<ShowInstalledExtensionsMessage>(this, ShowInstalledExtensionsMessage);

        }

        void UnInitExtensions()
        {
            App.ExtensionsSvc.UnloadExtensions();
            ctlExtensionsBarTop.UnloadExtensions();
            ctlExtensionsBarLeft.UnloadExtensions();
            ctlExtensionsBarRight.UnloadExtensions();
            ctlExtensionsBarBottom.UnloadExtensions();
        }

        public void RecieveMessage(object message)
        {
        }


        
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }


        public void OnPaneLoad()
        {

        }

        public void OnPaneUnload()
        {

        }
    }
}
