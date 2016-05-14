using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.Extensions.Popups;
using WeakEvent;
using Windows.UI.Core;
using Windows.ApplicationModel.AppExtensions;
using X.Services.Settings;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel;

namespace X.Services.Extensions
{
    public class ExtensionsService : ISender, IExtensionsService, IUWPExtensionsService
    {
        List<ExtensionLite> _extensions = new List<ExtensionLite>();
        private string _contract = "X.Extensions";  //our ecosystem of extensions all have this contract "name"
        private CoreDispatcher _dispatcher;
        private AppExtensionCatalog _catalog;

        private readonly WeakEventSource<EventArgs> _OnInstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnInstallExtension
        {
            add { _OnInstallExtensionSource.Subscribe(value); }
            remove { _OnInstallExtensionSource.Unsubscribe(value); }
        }

        private readonly WeakEventSource<EventArgs> _OnDidInstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnDidInstallExtension
        {
            add { _OnDidInstallExtensionSource.Subscribe(value); }
            remove { _OnDidInstallExtensionSource.Unsubscribe(value); }
        }
        
        private readonly WeakEventSource<EventArgs> _OnUninstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnUninstallExtension
        {
            add { _OnUninstallExtensionSource.Subscribe(value); }
            remove { _OnUninstallExtensionSource.Unsubscribe(value); }
        }

        private readonly WeakEventSource<EventArgs> _OnDidUninstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnDidUninstallExtension
        {
            add { _OnDidUninstallExtensionSource.Subscribe(value); }
            remove { _OnDidUninstallExtensionSource.Unsubscribe(value); }
        }


        public ExtensionsService() {
            CreateDefaultExtensions();
            InitializeCatalog();
        }


        public void SendMessage(object msg, ExtensionType type)
        {
            var foundExts = _extensions.Where(x => x.ExtensionType == type);
            foreach (var ext in foundExts) {
                ext.Extension.RecieveMessage(msg);
            }
        }



        private void CreateDefaultExtensions() {
            Install(new HandleNewWindowAsInlineLink()).Wait();
            Install(new HandleNavigationFailedAsInlineToast()).Wait();
            Install(new OSToast()).Wait();
        }

        public IExtensionManifest GetExtensionMetadata(Guid guid)
        {
            foreach (var el in _extensions)
            {
                if (el.Manifest.UniqueID == guid) {

                    return new ExtensionManifest(el.Manifest);
                }
            }

            return null;
        }

        public List<IExtensionManifest> GetExtensionsMetadata() {

            List<IExtensionManifest> ret = new List<IExtensionManifest>();
            
            foreach (var el in _extensions)
            {
                ret.Add( new ExtensionManifest(el.Manifest));
            }

            return ret;
        }
        
        public List<IExtensionManifest> GetToolbarExtensionsMetadata(ExtensionInToolbarPositions position)
        {

            List<IExtensionManifest> ret = new List<IExtensionManifest>();

            foreach (var el in _extensions)
            {
                if(el.Manifest.FoundInToolbarPositions == position)
                    ret.Add( new ExtensionManifest(el.Manifest));   
            }

            return ret;
        }


        private void DoSendMessage(object sender, EventArgs e)
        {
            var senderExt = (IExtension)sender;
            IEnumerable<ExtensionLite> foundExts = null;
            Guid guidToUse = senderExt.ExtensionManifest.UniqueID;

            //if (e is CloseExtensionEventArgs) guidToUse = ((CloseExtensionEventArgs)e).ExtensionUniqueGuid;

            if (e is IReceiver)
                foundExts = _extensions.Where(x => x.ExtensionType == ((IReceiver)e).ReceiverType && x.Manifest?.UniqueID != guidToUse);

            if (foundExts != null) foreach (var ext in foundExts) ext.Extension?.RecieveMessage(e);

        }


        private async Task<bool> validate(string zipPath, IExtension extension) {

            return true;
        }

        public void UnloadExtensions() {
            foreach (var ext in _extensions)
            {
                ext.Extension.SendMessage -= DoSendMessage;
            }

            _extensions.Clear();
        }


        public async Task<IExtensionManifest> Install(IExtensionManifest extension)
        {
            if (extension == null) return null;
            //await validate(string.Empty, extension);
            //OnInstallExtension?.Invoke(extension, EventArgs.Empty);
            //extension.SendMessage += DoSendMessage;
            _extensions.Add(new ExtensionLite() { Extension = null, Manifest = extension, ExtensionType = ExtensionType.UIComponentLazy });
            //OnDidInstallExtension?.Invoke(extension, EventArgs.Empty);
            return extension;
        }

        public async Task<IExtension> Install(IExtension extension)
        {
            await validate(string.Empty, extension);
            _OnInstallExtensionSource?.Raise(extension, EventArgs.Empty);
            extension.SendMessage += DoSendMessage;
            _extensions.Add(new ExtensionLite() { Extension = extension, Manifest = extension.ExtensionManifest, ExtensionType = extension.ExtensionType });
            _OnDidInstallExtensionSource?.Raise(extension, EventArgs.Empty);
            return extension;
        }

        public IExtension CreateInstance(IExtensionManifest md)
        {

            var found = _extensions.Where(x => x.Manifest.UniqueID == md.UniqueID).FirstOrDefault();

            if (found.Extension == null)
            {
                var newElExt = new Services.ThirdParty._Template(found.Manifest);
                newElExt.SendMessage += DoSendMessage;

                if (md.LaunchInDockPositions == ExtensionInToolbarPositions.Left || md.LaunchInDockPositions == ExtensionInToolbarPositions.Right)
                    newElExt.Width = 350;
                else newElExt.Height = 200;

                found.Extension = newElExt;
            }

            if (found.Extension != null && found.IsShowingExtensionPanel) return null;

            found.IsShowingExtensionPanel = true;

            return found.Extension;
        }
        
        public IExtension Install(string zipPath)
        {
            throw new NotImplementedException();
        }

        public void UnInstall(IExtension extension)
        {
            _OnUninstallExtensionSource?.Raise(extension, EventArgs.Empty);
            extension?.OnPaneUnload();
            extension.SendMessage -= DoSendMessage;
            _extensions.Remove(_extensions.Where(x=>x.Manifest.UniqueID == extension.ExtensionManifest.UniqueID).FirstOrDefault());
            _OnDidUninstallExtensionSource?.Raise(extension, EventArgs.Empty);
        }

        public bool UninstallInstance(Guid instanceUniqueId)
        {

            var found = _extensions.Where(x => x.Manifest.UniqueID == instanceUniqueId).FirstOrDefault();
            if (found.Extension != null) {
                found.Extension.OnPaneUnload();
                found.Extension.ExtensionManifest = null;
                found.Extension.SendMessage -= DoSendMessage;
                found.Extension = null;
                found.IsShowingExtensionPanel = false;
            }

            //foreach (var el in _extensions)
            //{
            //    if (el.Extension?.ExtensionManifest.UniqueID == instanceUniqueId)
            //    {
            //        el.Extension.SendMessage -= DoSendMessage;
            //        el.Extension.ExtensionManifest = null;
            //        _extensions.Remove(el);
            //        break;
            //    }
            //}
            return true;
        }

        public IExtension[] GetInstalled()
        {
            throw new NotImplementedException();
        }

        public void UpdateExtension(IExtensionManifest manifest) {
            var found = _extensions.Where(x => x.Manifest.UniqueID == manifest.UniqueID).FirstOrDefault();
            //if (found != null) {
                found.Manifest.IsExtEnabled = manifest.IsExtEnabled;
                found.Manifest.LaunchInDockPositions = manifest.LaunchInDockPositions;
                found.Manifest.FoundInToolbarPositions = manifest.FoundInToolbarPositions;
            //}
        }

        public void InitializeCatalog()
        {
            _catalog = AppExtensionCatalog.Open(_contract);

            AppSettings.Clear();
            if (_dispatcher != null) throw new Exception("Extension Manager for " + this._contract + " is already initialized.");

            // thread that initializes the extension manager has the dispatcher
            _dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;

            // set up extension management events
            _catalog.PackageInstalled += Catalog_PackageInstalled;
            _catalog.PackageUninstalling += Catalog_PackageUninstalling;
            _catalog.PackageUpdating += Catalog_PackageUpdating;
            _catalog.PackageUpdated += Catalog_PackageUpdated;
            _catalog.PackageStatusChanged += Catalog_PackageStatusChanged;
            
        }

        public async Task PopulateAllUWPExtensions()
        {
            if (_dispatcher == null) throw new Exception("Extension Manager for " + this._contract + " is not initialized.");
            
            IReadOnlyList<AppExtension> extensions = await _catalog.FindAllAsync();
            foreach (AppExtension ext in extensions)
                await LoadUWPExtension(ext);
            
        }
        
        private async void Catalog_PackageInstalled(AppExtensionCatalog sender, AppExtensionPackageInstalledEventArgs args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                foreach (AppExtension ext in args.Extensions) await LoadUWPExtension(ext);
            });
        }

        private async void Catalog_PackageUpdated(AppExtensionCatalog sender, AppExtensionPackageUpdatedEventArgs args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                foreach (AppExtension ext in args.Extensions) await LoadUWPExtension(ext);
            });
        }

        // package is updating, so just unload the extensions
        private async void Catalog_PackageUpdating(AppExtensionCatalog sender, AppExtensionPackageUpdatingEventArgs args)
        {
            await UnloadUWPExtensions(args.Package);
        }

        // package is removed, so unload all the extensions in the package and remove it
        private async void Catalog_PackageUninstalling(AppExtensionCatalog sender, AppExtensionPackageUninstallingEventArgs args)
        {
            await RemoveUWPExtensions(args.Package);
        }

        // package status has changed, could be invalid, licensing issue, app was on USB and removed, etc
        private async void Catalog_PackageStatusChanged(AppExtensionCatalog sender, AppExtensionPackageStatusChangedEventArgs args)
        {
            // get package status
            if (!(args.Package.Status.VerifyIsOK()))
            {
                // if it's offline unload only
                if (args.Package.Status.PackageOffline) await UnloadUWPExtensions(args.Package);

                // package is being serviced or deployed
                else if (args.Package.Status.Servicing || args.Package.Status.DeploymentInProgress)
                {
                    // ignore these package status events
                }

                // package is tampered or invalid or some other issue, remove the extensions
                else
                {
                    await RemoveUWPExtensions(args.Package);
                }

            }
            // if package is now OK, attempt to load the extensions
            else
            {
                // try to load any extensions associated with this package
                await LoadUWPExtensions(args.Package);
            }
        }

        // removes all extensions associated with a package - used when removing a package or it becomes invalid
        public async Task RemoveUWPExtensions(Package package)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName).ToList().ForEach(e => { e.UnloadUWPExtension(); _extensions.Remove(e); });
            });
        }

        // loads all extensions associated with a package - used for when package status comes back
        public async Task LoadUWPExtensions(Package package)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName).ToList().ForEach(async e => { await e.LoadUWPExtension(); });
            });
        }

        // unloads all extensions associated with a package - used for updating and when package status goes away
        public async Task UnloadUWPExtensions(Package package)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName).ToList().ForEach(e => { e.UnloadUWPExtension(); });
            });
        }

        public async Task LoadUWPExtension(AppExtension ext)
        {
            // get unique identifier for this extension
            string identifier = ext.AppInfo.AppUserModelId + "!" + ext.Id;

            // load the extension if the package is OK
            if (!(ext.Package.Status.VerifyIsOK()
                    // This is where we'd normally do signature verfication, but don't care right now
                    //&& extension.Package.SignatureKind == PackageSignatureKind.Store
                    ))
            {
                // if this package doesn't meet our requirements
                // ignore it and return
                return;
            }



            // if its already existing then this is an update
            var existingExt = _extensions.Where(e => e.AppExtensionUniqueId == identifier).FirstOrDefault();

            // new extension
            if (existingExt == null)
            {
                try
                {
                    // get extension properties
                    IPropertySet properties = await ext.GetExtensionPropertiesAsync();
                    //IPropertySet properties = null;

                    // get logo 
                    var filestream = await (ext.AppInfo.DisplayInfo.GetLogo(new Windows.Foundation.Size(1, 1))).OpenReadAsync();
                    BitmapImage logo = new BitmapImage();
                    logo.SetSource(filestream);

                    // create new extension
                    var nExt = new ExtensionLite(ext, properties);

                    // Add it to extension list
                    _extensions.Add(nExt);

                    // load it
                    //await nExt.Load();
                }
                catch (Exception ex)
                {
                    //chances are if it fails retrieving properties that the app was added with no properties .. Uninstall the app and reinstall it and hopefully the latest metadata will be there
                }
            }
            // update
            else
            {
                // unload the extension
                existingExt.UnloadUWPExtension();

                // update the extension
                await existingExt.UpdateUWPExtension(ext);
            }
        }

        public IExtensionLite GetExtensionByAppExtensionUniqueId(string uniqueId)
        {
            return _extensions.Where(x => x.AppExtensionUniqueId == uniqueId).FirstOrDefault();
        }

        public IEnumerable<IExtensionLite> GetUWPExtensions()
        {
            return _extensions.Where(x => x.Manifest.IsUWPExtension);
        }



        //=========================
        //singleton
        //=========================
        private static ExtensionsService instance;

        public static ExtensionsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExtensionsService();
                }
                return instance;
            }
        }
    }
}
