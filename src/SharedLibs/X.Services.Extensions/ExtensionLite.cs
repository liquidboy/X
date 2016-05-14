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
    public class ExtensionLite
    {
        public IExtensionManifest Manifest;
        public ExtensionType ExtensionType;
        public IExtension Extension;
        public bool IsShowingExtensionPanel;



        public string AppExtensionUniqueId;
        public AppExtension AppExtension;
        private IPropertySet _valueset;


        public ExtensionLite() {

        }

        public ExtensionLite(AppExtension ext, IPropertySet properties) : this() {
            AppExtension = ext;
            _valueset = properties;
            AppExtensionUniqueId = ext.AppInfo.AppUserModelId + "!" + ext.Id;

            Manifest = new ExtensionManifest();
            Manifest.AppExtensionUniqueID = AppExtensionUniqueId;
            foreach (var prop in properties)
            {
                switch (prop.Key)
                {
                    case "Title": Manifest.Title = GetValueFromProperty(prop.Value); break;
                    case "IconUrl": Manifest.IconUrl = GetValueFromProperty(prop.Value); break;
                    case "Publisher": Manifest.Publisher = GetValueFromProperty(prop.Value); break;
                    case "Version": Manifest.Version = GetValueFromProperty(prop.Value); break;
                    case "Abstract": Manifest.Abstract = GetValueFromProperty(prop.Value); break;
                    case "FoundInToolbarPositions": Manifest.FoundInToolbarPositions = (ExtensionInToolbarPositions)Enum.Parse(typeof(ExtensionInToolbarPositions), GetValueFromProperty(prop.Value)); break;
                    case "LaunchInDockPositions": Manifest.LaunchInDockPositions = (ExtensionInToolbarPositions)Enum.Parse(typeof(ExtensionInToolbarPositions), GetValueFromProperty(prop.Value)); break;
                    case "ContentControl": Manifest.ContentControl = GetValueFromProperty(prop.Value); break;
                    case "AssemblyName": Manifest.AssemblyName = GetValueFromProperty(prop.Value); break;
                    case "IsExtEnabled": Manifest.IsExtEnabled = bool.Parse(GetValueFromProperty(prop.Value)); AppSettings.AppExtensionEnabled = Manifest.IsExtEnabled; break;
                    case "IsUWPExtension": Manifest.IsUWPExtension = bool.Parse(GetValueFromProperty(prop.Value)); break;
                }
            }

        }

        private string GetValueFromProperty(object propertyValue)
        {
            return (string)((PropertySet)propertyValue).Values.First();
        }

        public async Task<ValueSet> MakeCommandCall(string commandCall, string serviceName) {
            using (var connection = new AppServiceConnection())
            {
                connection.AppServiceName = serviceName;
                connection.PackageFamilyName = AppExtension.Package.Id.FamilyName;
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
        

        public async Task UpdateUWPExtension(AppExtension ext)
        {
            // ensure this is the same uid
            string identifier = ext.AppInfo.AppUserModelId + "!" + ext.Id;
            if (identifier != this.AppExtensionUniqueId)
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
            this.AppExtension = ext;
            this._valueset = properties;
            //this._logo = logo;
            /*
            // get version from properties
            if (_props.ContainsKey("Version"))
            {
                this._version = this._props["Version"] as string;
            }
            else
            { */
            //this._version = "Unknown";
            //}

            // load it
            await LoadUWPExtension();
        }

        public async void UnloadUWPExtension()
        { }

        public async Task LoadUWPExtension()
        { }
    }
    
}
