using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using X.Extensions.Popups;
using System.Reflection;
using WeakEvent;

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
}
