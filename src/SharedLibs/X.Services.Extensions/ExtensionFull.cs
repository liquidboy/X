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
using Windows.ApplicationModel.AppService;
using System.Diagnostics;

namespace X.Services.Extensions
{
    
    public class ExtensionFull : IExtensionFull
    {
        private AppExtension _extension;
        private IPropertySet _valueset;
        private IExtensionManifest _manifest;
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

            _manifest = new ExtensionManifest();
            foreach (var prop in properties) {
                switch (prop.Key) {
                    case "Title": _manifest.Title = GetValueFromProperty(prop.Value); break;
                    case "IconUrl": _manifest.IconUrl = GetValueFromProperty(prop.Value); break;
                    case "Publisher": _manifest.Publisher = GetValueFromProperty(prop.Value); break;
                    case "Version": _manifest.Version = GetValueFromProperty(prop.Value); break;
                    case "Abstract": _manifest.Abstract = GetValueFromProperty(prop.Value); break;
                    case "FoundInToolbarPositions": _manifest.FoundInToolbarPositions = (ExtensionInToolbarPositions)Enum.Parse( typeof(ExtensionInToolbarPositions), GetValueFromProperty(prop.Value)); break;
                    case "LaunchInDockPositions": _manifest.LaunchInDockPositions = (ExtensionInToolbarPositions)Enum.Parse(typeof(ExtensionInToolbarPositions), GetValueFromProperty(prop.Value)); break;
                    case "ContentControl": _manifest.ContentControl = GetValueFromProperty(prop.Value); break;
                    case "AssemblyName": _manifest.AssemblyName = GetValueFromProperty(prop.Value); break;
                    case "IsExtEnabled": _manifest.IsExtEnabled = bool.Parse(GetValueFromProperty(prop.Value)); AppSettings.AppExtensionEnabled = _manifest.IsExtEnabled; break;
                    case "IsUWPExtension": _manifest.IsUWPExtension = bool.Parse(GetValueFromProperty(prop.Value)); break;
                }
            }
            
        }

        private string GetValueFromProperty(object propertyValue) {
            return (string)((PropertySet)propertyValue).Values.First();
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
        
        public IExtensionManifest Manifest
        {
            get { return _manifest; }
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

        public async Task<ValueSet> MakeCommandCall(string commandCall, string serviceName) {
            using (var connection = new AppServiceConnection())
            {
                connection.AppServiceName = serviceName; 
                connection.PackageFamilyName = _extension.Package.Id.FamilyName;
                var status = await connection.OpenAsync();
                if (status != AppServiceConnectionStatus.Success)
                {
                    Debug.WriteLine("Failed app service connection");
                }
                else
                {
                    var request = new ValueSet();
                    request.Add("Command", commandCall);
                    AppServiceResponse response = await connection.SendMessageAsync(request);
                    if (response.Status == Windows.ApplicationModel.AppService.AppServiceResponseStatus.Success)
                    {
                        var message = response.Message as ValueSet;

                        return message;
                    }
                }

            }

            return null;
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

