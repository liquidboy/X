using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakEvent;
using Windows.UI.Xaml.Controls;
using X.Browser.ViewModels;
using X.Extensions;
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
            if (message is LoadWebViewEventArgs)
            {
                //doing this kills the binding to it hence breaks the tabs
                //wvMain.Source = ((LoadWebViewEventArgs)message).Uri;


                var vm = (BrowserVM)this.DataContext;
                var uri = ((LoadWebViewEventArgs)message).Uri;

                TabViewModel tempTab = new TabViewModel()
                {
                    DisplayTitle = uri.Host,
                    FaviconUri = "http://" + uri.Host + "//favicon.ico",
                    HasFocus = false,
                    Uri = uri.OriginalString,
                    Id = 999,
                    Uid = Guid.NewGuid().ToString(),
                    PrimaryBackgroundColor = "Black",
                    PrimaryForegroundColor = "White",
                    PrimaryFontFamily = "Segoe UI"
                };

                vm.SelectedTab = tempTab;
                vm.ExposedNotifyPropertyChanged("SelectedTab");
            }
            else if (message is RequestListOfInstalledExtensionsEventArgs)
            {
                var extensions = App.ExtensionsSvc.GetExtensionsMetadata();
                _SendMessageSource?.Raise(this, new ResponseListOfInstalledExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfTopToolbarExtensionsEventArgs)
            {
                var extensions = App.ExtensionsSvc.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Top);
                _SendMessageSource?.Raise(this, new ResponseListOfTopToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfBottomToolbarExtensionsEventArgs)
            {
                var extensions = App.ExtensionsSvc.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Bottom);
                _SendMessageSource?.Raise(this, new ResponseListOfBottomToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfLeftToolbarExtensionsEventArgs)
            {
                var extensions = App.ExtensionsSvc.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Left);
                _SendMessageSource?.Raise(this, new ResponseListOfLeftToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfRightToolbarExtensionsEventArgs)
            {
                var extensions = App.ExtensionsSvc.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Right);
                _SendMessageSource?.Raise(this, new ResponseListOfRightToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is LaunchExtensionEventArgs)
            {
                var extGuid = ((LaunchExtensionEventArgs)message).ExtensionUniqueGuid;
                var extMetaData = App.ExtensionsSvc.GetExtensionMetadata(extGuid);
                var newExtensionInstance = App.ExtensionsSvc.CreateInstance(extMetaData);

                if (newExtensionInstance != null)
                {
                    if ((string)extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Left.ToString())
                        grdDockedExtensionLeft.Children.Insert(grdDockedExtensionLeft.Children.Count, (UserControl)newExtensionInstance);
                    else if ((string)extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Top.ToString())
                        grdDockedExtensionTop.Children.Insert(grdDockedExtensionTop.Children.Count, (UserControl)newExtensionInstance);
                    else if ((string)extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Right.ToString())
                        grdDockedExtensionRight.Children.Insert(0, (UserControl)newExtensionInstance);
                    else if ((string)extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Bottom.ToString())
                        grdDockedExtensionBottom.Children.Insert(grdDockedExtensionBottom.Children.Count, (UserControl)newExtensionInstance);
                    else if ((string)extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.BottomFull.ToString())
                        grdDockedExtensionBottomFull.Children.Insert(grdDockedExtensionBottomFull.Children.Count, (UserControl)newExtensionInstance);

                    newExtensionInstance.OnPaneLoad();
                }


            }
            else if (message is CloseExtensionEventArgs)
            {
                var extGuid = ((CloseExtensionEventArgs)message).ExtensionUniqueGuid;
                var md = App.ExtensionsSvc.GetExtensionMetadata(extGuid);

                foreach (dynamic child in grdDockedExtensionBottomFull.Children)
                {
                    if (((Guid)child.ExtensionManifest.UniqueID).ToString() == extGuid.ToString())
                    {
                        App.ExtensionsSvc.UninstallInstance((Guid)child.ExtensionManifest.UniqueID);
                        grdDockedExtensionBottomFull.Children.Remove(child);
                    }
                }

                foreach (dynamic child in grdDockedExtensionBottom.Children)
                {
                    if (((Guid)child.ExtensionManifest.UniqueID).ToString() == extGuid.ToString())
                    {
                        App.ExtensionsSvc.UninstallInstance((Guid)child.ExtensionManifest.UniqueID);
                        grdDockedExtensionBottom.Children.Remove(child);
                    }
                }

                foreach (dynamic child in grdDockedExtensionTop.Children)
                {
                    if (((Guid)child.ExtensionManifest.UniqueID).ToString() == extGuid.ToString())
                    {
                        App.ExtensionsSvc.UninstallInstance((Guid)child.ExtensionManifest.UniqueID);
                        grdDockedExtensionTop.Children.Remove(child);
                    }
                }

                foreach (dynamic child in grdDockedExtensionLeft.Children)
                {
                    if (((Guid)child.ExtensionManifest.UniqueID).ToString() == extGuid.ToString())
                    {
                        App.ExtensionsSvc.UninstallInstance((Guid)child.ExtensionManifest.UniqueID);
                        grdDockedExtensionLeft.Children.Remove(child);
                    }
                }

                foreach (dynamic child in grdDockedExtensionRight.Children)
                {
                    if (((Guid)child.ExtensionManifest.UniqueID).ToString() == extGuid.ToString())
                    {
                        App.ExtensionsSvc.UninstallInstance((Guid)child.ExtensionManifest.UniqueID);
                        grdDockedExtensionRight.Children.Remove(child);
                    }
                }

            }
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
