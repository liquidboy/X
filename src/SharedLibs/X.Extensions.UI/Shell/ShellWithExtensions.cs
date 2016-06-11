using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WeakEvent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using X.Extensions.UIComponentExtensions;
using X.UI.Chrome;

namespace X.Extensions.UI
{
    public sealed class ShellWithExtensions : Control, IExtensionHost
    {
        bool hasInitialized = false;
        Shell ctlShell;
        ExtensionsIconBarTop ctlExtensionsBarTop;
        ExtensionsIconBarBottom ctlExtensionsBarBottom;
        ExtensionsIconBarLeft ctlExtensionsBarLeft;
        ExtensionsIconBarRight ctlExtensionsBarRight;


        public object ContentMain
        {
            get { return (object)GetValue(ContentMainProperty); }
            set { SetValue(ContentMainProperty, value); }
        }

        public static readonly DependencyProperty ContentMainProperty = DependencyProperty.Register("ContentMain", typeof(object), typeof(ShellWithExtensions), new PropertyMetadata(null));







        public ShellWithExtensions()
        {
            this.DefaultStyleKey = typeof(ShellWithExtensions);
            this.Loaded += Shell_Loaded; ;
            this.Unloaded += Shell_Unloaded; ;
        }

        private void Shell_Unloaded(object sender, RoutedEventArgs e)
        {
            UnInitExtensions();
        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            oneTimeInit();
        }

        protected override void OnApplyTemplate()
        {
            oneTimeInit();
            base.OnApplyTemplate();
        }
        public async void oneTimeInit()
        {
            if (hasInitialized) return;

            if (ctlShell == null) ctlShell = GetTemplateChild("ctlShell") as Shell;

            if (ctlShell != null)
            {
                ctlExtensionsBarTop = GetTemplateChild("ctlExtensionsBarTop") as ExtensionsIconBarTop;
                ctlExtensionsBarBottom = GetTemplateChild("ctlExtensionsBarBottom") as ExtensionsIconBarBottom;
                ctlExtensionsBarLeft = GetTemplateChild("ctlExtensionsBarLeft") as ExtensionsIconBarLeft;
                ctlExtensionsBarRight = GetTemplateChild("ctlExtensionsBarRight") as ExtensionsIconBarRight;

                await InitExtensions();
            }

            if (ctlShell != null) hasInitialized = true;
        }

        


        /// <summary>
        /// EXTENSIONS BITS BELOW
        /// </summary>

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.UIComponent;
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public string Path { get; set; }



        private bool _isExtEnabled = true;
        public bool IsExtEnabled { get { return _isExtEnabled; } set { _isExtEnabled = value; } }

        private bool _canUninstall = false;
        public bool CanUninstall { get { return _canUninstall; } set { _canUninstall = value; } }


        bool isRunning = false;
        public async Task InitExtensions()
        {
            if (isRunning) return;
            isRunning = true;

            ExtensionManifest = new ExtensionManifest("X.Player", string.Empty, "X.Player", "1.0", "Generic X Extension Player", ExtensionInToolbarPositions.None, ExtensionInToolbarPositions.None);
            await X.Services.Extensions.ExtensionsService.Instance.Install(this);

            X.Extensions.UI.Shared.ExtensionUtils.LoadThirdPartyExtensions(new List<ExtensionManifest>{
                X.Extensions.FirstParty.Settings.Installer.GetManifest(),
            });

            await X.Services.Extensions.ExtensionsService.Instance.PopulateAllUWPExtensions();
            X.Extensions.UI.Shared.ExtensionUtils.UpdateUWPExtensionsWithStateSavedData(X.Services.Extensions.ExtensionsService.Instance.GetUWPExtensions());

            ctlExtensionsBarTop.InstallMyself();
            ctlExtensionsBarBottom.InstallMyself();
            ctlExtensionsBarLeft.InstallMyself();
            ctlExtensionsBarRight.InstallMyself();

            isRunning = false;
        }



        public void UnInitExtensions()
        {
            X.Services.Extensions.ExtensionsService.Instance.UnloadExtensions();
            ctlExtensionsBarTop.UnloadExtensions();
            ctlExtensionsBarLeft.UnloadExtensions();
            ctlExtensionsBarRight.UnloadExtensions();
            ctlExtensionsBarBottom.UnloadExtensions();
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
            else if (message is RequestListOfBottomToolbarExtensionsEventArgs)
            {
                var extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Bottom);
                _SendMessageSource?.Raise(this, new ResponseListOfBottomToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfLeftToolbarExtensionsEventArgs)
            {
                var extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Left);
                _SendMessageSource?.Raise(this, new ResponseListOfLeftToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestListOfRightToolbarExtensionsEventArgs)
            {
                var extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Right);
                _SendMessageSource?.Raise(this, new ResponseListOfRightToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is RequestRefreshToolbarExtensionsEventArgs)
            {

                var extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Top);
                _SendMessageSource?.Raise(this, new ResponseListOfTopToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });

                extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Bottom);
                _SendMessageSource?.Raise(this, new ResponseListOfBottomToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });

                extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Left);
                _SendMessageSource?.Raise(this, new ResponseListOfLeftToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });

                extensions = X.Services.Extensions.ExtensionsService.Instance.GetToolbarExtensionsMetadata(ExtensionInToolbarPositions.Right);
                _SendMessageSource?.Raise(this, new ResponseListOfRightToolbarExtensionsEventArgs() { ExtensionsMetadata = extensions, ReceiverType = ExtensionType.UIComponent });
            }
            else if (message is LaunchExtensionEventArgs)
            {
                var extGuid = ((LaunchExtensionEventArgs)message).ExtensionUniqueGuid;
                LaunchExtension(extGuid);
            }
            else if (message is CloseExtensionEventArgs)
            {
                var extGuid = ((CloseExtensionEventArgs)message).ExtensionUniqueGuid;
                CloseExtension(extGuid);
            }
        }

        public void LaunchExtension(Guid extGuid)
        {
            var extMetaData = X.Services.Extensions.ExtensionsService.Instance.GetExtensionMetadata(extGuid);
            var newExtensionInstance = X.Services.Extensions.ExtensionsService.Instance.CreateInstance(extMetaData);

            if (newExtensionInstance != null)
            {
                if (extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Left)
                    ctlShell.DockedExtensionLeft.Children.Insert(ctlShell.DockedExtensionLeft.Children.Count, (UserControl)newExtensionInstance);
                else if (extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Top)
                    ctlShell.DockedExtensionTop.Children.Insert(ctlShell.DockedExtensionTop.Children.Count, (UserControl)newExtensionInstance);
                else if (extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Right)
                    ctlShell.DockedExtensionRight.Children.Insert(0, (UserControl)newExtensionInstance);
                else if (extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.Bottom)
                    ctlShell.DockedExtensionBottom.Children.Insert(ctlShell.DockedExtensionBottom.Children.Count, (UserControl)newExtensionInstance);
                else if (extMetaData.LaunchInDockPositions == ExtensionInToolbarPositions.BottomFull)
                    ctlShell.DockedExtensionBottomFull.Children.Insert(ctlShell.DockedExtensionBottomFull.Children.Count, (UserControl)newExtensionInstance);

                newExtensionInstance.OnPaneLoad();
            }
        }

        public void CloseExtension(Guid extGuid)
        {
            X.Extensions.UI.Shared.ExtensionUtils.DeleteFromCollection(ctlShell.DockedExtensionBottomFull.Children, extGuid);
            X.Extensions.UI.Shared.ExtensionUtils.DeleteFromCollection(ctlShell.DockedExtensionBottom.Children, extGuid);
            X.Extensions.UI.Shared.ExtensionUtils.DeleteFromCollection(ctlShell.DockedExtensionTop.Children, extGuid);
            X.Extensions.UI.Shared.ExtensionUtils.DeleteFromCollection(ctlShell.DockedExtensionLeft.Children, extGuid);
            X.Extensions.UI.Shared.ExtensionUtils.DeleteFromCollection(ctlShell.DockedExtensionRight.Children, extGuid);
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
