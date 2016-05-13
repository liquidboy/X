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
        public async Task Initialize()
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
            await FindAllExtensions();
        }


        public async Task FindAllExtensions()
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
                    ExtensionFull nExt = new ExtensionFull(ext, properties, logo);

                    // Add it to extension list
                    _extensions.Add(nExt);

                    // load it
                    await nExt.Load();
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
                    instance = new ExtensionsFullService("X.Extensions");
                }
                return instance;
            }
        }
    }
    
}
