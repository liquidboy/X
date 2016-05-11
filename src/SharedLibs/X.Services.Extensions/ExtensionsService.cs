using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using X.Extensions.Popups;
using System.Reflection;
using WeakEvent;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppExtensions;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media.Imaging;
using X.Services.Settings;
using Windows.Storage;
using Windows.UI.Core;

namespace X.Services.Extensions
{


    public class ExtensionsService : ISender, IExtensionsService
    {
        List<ExtensionLite> _extensions = new List<ExtensionLite>();

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

    public class ExtensionsFullService : IExtensionsFullService
    {
        private ObservableCollection<IExtensionFull> _extensions;
        private string _contract;
        private CoreDispatcher _dispatcher;
        private AppExtensionCatalog _catalog;

        public ExtensionsFullService(string contract)
        {
            // extensions list   
            _extensions = new ObservableCollection<IExtensionFull>();

            // catalog & contract
            _contract = contract;
            _catalog = AppExtensionCatalog.Open(_contract);

            // using a method that uses the UI Dispatcher before initializing will throw an exception
            _dispatcher = null;
        }

        public ObservableCollection<IExtensionFull> Extensions
        {
            get { return _extensions; }
        }

        public string Contract
        {
            get { return _contract; }
        }

        // this sets up UI dispatcher and does initial extension scan
        public void Initialize()
        {
            // check that we haven't already been initialized
            if (_dispatcher != null)
            {
                throw new ExtensionManagerException("Extension Manager for " + this.Contract + " is already initialized.");
            }

            // thread that initializes the extension manager has the dispatcher
            _dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;

            // set up extension management events
            _catalog.PackageInstalled += Catalog_PackageInstalled;
            _catalog.PackageUninstalling += Catalog_PackageUninstalling;
            _catalog.PackageUpdating += Catalog_PackageUpdating;
            _catalog.PackageUpdated += Catalog_PackageUpdated;
            _catalog.PackageStatusChanged += Catalog_PackageStatusChanged;

            // Scan all extensions
            FindAllExtensions();
        }


        public async void FindAllExtensions()
        {
            // make sure we have initialized
            if (_dispatcher == null)
            {
                throw new ExtensionManagerException("Extension Manager for " + this.Contract + " is not initialized.");
            }

            // load all the extensions currently installed
            IReadOnlyList<AppExtension> extensions = await _catalog.FindAllAsync();
            foreach (AppExtension ext in extensions)
            {
                // load this extension
                await LoadExtension(ext);
            }
        }


        private async void Catalog_PackageInstalled(AppExtensionCatalog sender, AppExtensionPackageInstalledEventArgs args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                foreach (AppExtension ext in args.Extensions)
                {
                    await LoadExtension(ext);
                }
            });
        }

        private async void Catalog_PackageUpdated(AppExtensionCatalog sender, AppExtensionPackageUpdatedEventArgs args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                foreach (AppExtension ext in args.Extensions)
                {
                    await LoadExtension(ext);
                }
            });
        }

        // package is updating, so just unload the extensions
        private async void Catalog_PackageUpdating(AppExtensionCatalog sender, AppExtensionPackageUpdatingEventArgs args)
        {
            await UnloadExtensions(args.Package);
        }

        // package is removed, so unload all the extensions in the package and remove it
        private async void Catalog_PackageUninstalling(AppExtensionCatalog sender, AppExtensionPackageUninstallingEventArgs args)
        {
            await RemoveExtensions(args.Package);
        }

        // package status has changed, could be invalid, licensing issue, app was on USB and removed, etc
        private async void Catalog_PackageStatusChanged(AppExtensionCatalog sender, AppExtensionPackageStatusChangedEventArgs args)
        {
            // get package status
            if (!(args.Package.Status.VerifyIsOK()))
            {
                // if it's offline unload only
                if (args.Package.Status.PackageOffline)
                {
                    await UnloadExtensions(args.Package);
                }

                // package is being serviced or deployed
                else if (args.Package.Status.Servicing || args.Package.Status.DeploymentInProgress)
                {
                    // ignore these package status events
                }

                // package is tampered or invalid or some other issue, remove the extensions
                else
                {
                    await RemoveExtensions(args.Package);
                }

            }
            // if package is now OK, attempt to load the extensions
            else
            {
                // try to load any extensions associated with this package
                await LoadExtensions(args.Package);
            }
        }


        // loads an extension
        public async Task LoadExtension(AppExtension ext)
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
            IExtensionFull existingExt = _extensions.Where(e => e.UniqueId == identifier).FirstOrDefault();

            // new extension
            if (existingExt == null)
            {
                // get extension properties
                IPropertySet properties = await ext.GetExtensionPropertiesAsync();

                // get logo 
                var filestream = await (ext.AppInfo.DisplayInfo.GetLogo(new Windows.Foundation.Size(1, 1))).OpenReadAsync();
                BitmapImage logo = new BitmapImage();
                logo.SetSource(filestream);

                // create new extension
                ExtensionFull nExt = new ExtensionFull(ext, properties, logo);

                // Add it to extension list
                _extensions.Add(nExt);

                // load it
                await nExt.Load();
            }
            // update
            else
            {
                // unload the extension
                existingExt.Unload();

                // update the extension
                await existingExt.Update(ext);
            }
        }

        // loads all extensions associated with a package - used for when package status comes back
        public async Task LoadExtensions(Package package)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName).ToList().ForEach(async e => { await e.Load(); });
            });
        }

        // unloads all extensions associated with a package - used for updating and when package status goes away
        public async Task UnloadExtensions(Package package)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName).ToList().ForEach(e => { e.Unload(); });
            });
        }

        // removes all extensions associated with a package - used when removing a package or it becomes invalid
        public async Task RemoveExtensions(Package package)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName).ToList().ForEach(e => { e.Unload(); _extensions.Remove(e); });
            });
        }


        public async void RemoveExtension(IExtensionFull ext)
        {
            await _catalog.RequestRemovePackageAsync(ext.AppExtension.Package.Id.FullName);
        }








        // For exceptions using the Extension Manager
        public class ExtensionManagerException : Exception
        {
            public ExtensionManagerException() { }

            public ExtensionManagerException(string message) : base(message) { }

            public ExtensionManagerException(string message, Exception inner) : base(message, inner) { }
        }










        //=========================
        //singleton
        //=========================
        private static ExtensionsFullService instance;

        public static ExtensionsFullService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExtensionsFullService("xbrowser");
                }
                return instance;
            }
        }
    }

    public struct ExtensionLite
    {
        public IExtensionManifest Manifest;
        public ExtensionType ExtensionType;
        public IExtension Extension;
        public bool IsShowingExtensionPanel;
    }

    public struct ExtensionManifest : IExtensionManifest
    {
        public string Abstract { get; set; }

        public string AssemblyName { get; set; }

        public bool CanUninstall { get; set; }

        public string ContentControl { get; set; }

        public string DisplayName { get; set; }

        public ExtensionInToolbarPositions FoundInToolbarPositions { get; set; }

        public string IconUrl { get; set; }

        public bool IsExtEnabled { get; set; }

        public ExtensionInToolbarPositions LaunchInDockPositions { get; set; }

        public string Publisher { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public Guid UniqueID { get; set; }

        public string Version { get; set; }

        public ExtensionManifest(IExtensionManifest data)
        {
            this.Abstract = data.Abstract;
            this.AssemblyName = data.AssemblyName;
            this.CanUninstall = data.CanUninstall;
            this.ContentControl = data.ContentControl;
            this.DisplayName = data.DisplayName;
            this.FoundInToolbarPositions = data.FoundInToolbarPositions;
            this.IconUrl = data.IconUrl;
            this.IsExtEnabled = data.IsExtEnabled;
            this.LaunchInDockPositions = data.LaunchInDockPositions;
            this.Publisher = data.Publisher;
            this.Title = data.Title;
            this.UniqueID = data.UniqueID;
            this.Version = data.Version;
            this.Path = data.Path;
        }

    }

    public class ExtensionFull : IExtensionFull
    {
        private AppExtension _extension;
        private IPropertySet _valueset;
        private string _version;
        //private bool _enabled;
        private bool _supported;
        private bool _offline;
        private string _uniqueId;
        private BitmapImage _logo;
        private readonly object _sync = new object();

        public ExtensionFull(AppExtension ext, IPropertySet properties, BitmapImage logo)
        {
            _extension = ext;
            _valueset = properties;
            _offline = false;
            _supported = false;
            _logo = logo;
            _version = "Unknown";
            _uniqueId = ext.AppInfo.AppUserModelId + "!" + ext.Id;
        }

        public BitmapImage Logo
        {
            get { return _logo; }
        }

        public string UniqueId
        {
            get { return _uniqueId; }
        }

        public string Version
        {
            get { return _version; }
        }

        public bool Offline
        {
            get { return _offline; }
        }

        public AppExtension AppExtension
        {
            get { return _extension; }
        }

        public bool Enabled
        {
            get
            {
                return AppSettings.AppExtensionEnabled;
            }

            set
            {
                if (value != AppSettings.AppExtensionEnabled)
                {
                    if (value)
                    {
                        AppSettings.AppExtensionEnabled = true;
                        Enable();
                    }
                    else
                    {
                        AppSettings.AppExtensionEnabled = false;
                        Disable();
                    }
                }
            }
        }

        public bool Loaded
        {
            get
            {
                return AppSettings.AppExtensionLoaded;
            }

            set
            {
                AppSettings.AppExtensionLoaded = value;
            }
        }
        
        public async Task Update(AppExtension ext)
        {
            // ensure this is the same uid
            string identifier = ext.AppInfo.AppUserModelId + "!" + ext.Id;
            if (identifier != this.UniqueId)
            {
                return;
            }

            // get extension properties
            ValueSet properties = await ext.GetExtensionPropertiesAsync() as ValueSet;

            // get logo 
            var filestream = await (ext.AppInfo.DisplayInfo.GetLogo(new Windows.Foundation.Size(1, 1))).OpenReadAsync();
            BitmapImage logo = new BitmapImage();
            logo.SetSource(filestream);

            // update the extension
            this._extension = ext;
            this._valueset = properties;
            this._logo = logo;
            /*
            // get version from properties
            if (_props.ContainsKey("Version"))
            {
                this._version = this._props["Version"] as string;
            }
            else
            { */
            this._version = "Unknown";
            //}

            // load it
            await Load();
        }


        public async Task Load()
        {
            // if it's not enabled or already loaded, don't load it
            if (!Enabled || Loaded)
            {
                return;
            }

            // make sure package is OK to load
            if (!_extension.Package.Status.VerifyIsOK())
            {
                return;
            }

            // Extension is not loaded and enabled - load it
            StorageFolder folder = await _extension.GetPublicFolderAsync();
            if (folder != null)
            {

                // load file from extension package
                String fileName = @"SanFranciscoSights.json";
                StorageFile file = await folder.GetFileAsync(fileName);
                //var extensionsTrip = await SeedDataFactory.CreateSampleTrip(file);
                //foreach (var sight in extensionsTrip.Sights)
                //{
                //    sight.TripId = AppSettings.LastTripId;
                //}

                //var dataModelService = DataModelServiceFactory.CurrentDataModelService();

                //await dataModelService.InsertSights(extensionsTrip.Sights);
            }
            Loaded = true;
            _offline = false;

        }

        public async Task Enable()
        {
            // indicate desired state is enabled
            Enabled = true;

            // load the extension
            await Load();
        }

        public async void Unload()
        {
            // This is broken in 14291 - worked in 14273!
            //StorageFolder folder = await _extension.GetPublicFolderAsync();
            // HACK - temporarily set folder to the Models folder in the package - have put a copy of the SanFranciscoSights.json in there
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Models");

            if (folder == null)
                return; //nothing to unload

            // load file from extension package
            string fileName = @"SanFranciscoSights.json";
            StorageFile file = await folder.GetFileAsync(fileName);
            //var extensionsTrip = await SeedDataFactory.CreateSampleTrip(file);

            //var modelService = DataModelServiceFactory.CurrentDataModelService();
            //var currentTrip = await modelService.LoadTripAsync(AppSettings.LastTripId);

            // unload it
            lock (_sync)
            {
                if (Loaded)
                {
                    //foreach (var sight in extensionsTrip.Sights)
                    //{
                    //    var importedSight = currentTrip.Sights.Single(s => s.Name == sight.Name);
                    //    if (!importedSight.IsMySight)
                    //        modelService.DeleteSightAsync(importedSight);
                    //}

                    // see if package is offline
                    if (!_extension.Package.Status.VerifyIsOK() && !_extension.Package.Status.PackageOffline)
                    {
                        _offline = true;
                    }

                    Loaded = false;
                }
            }
        }

        public void Disable()
        {
            // only disable if it is enabled
            if (Enabled)
            {
                // set desired state to disabled
                Enabled = false;
                // unload the app
                Unload();
            }
        }
    }
}
